using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ItemTable : MonoBehaviour, IInteractable
{
    [SerializeField] private bool ifdestroy = true;
    [SerializeField] private ItemData giveItemData;
    [SerializeField] private ItemData consumeItemData;
    [SerializeField] private string hintText;

    public bool isInteracted = false;

    public event Action OnInteracted;

    public bool CanInteract(PlayerInventory inventory)
    {
        if (isInteracted) return false;
        if (inventory == null) return false;
        
        return true;
    }

    public void Interact(GameObject player)
    {
        PlayerInventory inventory = player.GetComponent<PlayerInventory>();
        if (inventory == null || !CanInteract(inventory)) return;

        // 1. 소모할 아이템이 필요한데, 플레이어 인벤토리에 없는 경우
        if (consumeItemData != null && !inventory.items.Contains(consumeItemData))
        {
            if (ItemPopupManager.Instance != null)
            {
                // 요청하신 CantInterect 함수 사용
                ItemPopupManager.Instance.CantInterect(hintText);
            }
            return;
        }

        // 2. 소모할 아이템은 없는데(null), hintText에 메시지가 적혀있는 경우 -> ShowConfirm 팝업 띄우기
        if (consumeItemData == null && !string.IsNullOrEmpty(hintText))
        {
            if (ItemPopupManager.Instance != null)
            {
                ItemPopupManager.Instance.ShowConfirm(
                    hintText,
                    () => {
                        // [예] 버튼을 눌렀을 때 상호작용 실행
                        ExecuteInteraction(inventory);
                    }
                );
            }
            return;
        }

        // 3. 정상적으로 소모할 아이템이 존재하여 사용하는 경우 (확인 팝업)
        if (consumeItemData != null)
        {
            if (ItemPopupManager.Instance != null)
            {
                ItemPopupManager.Instance.ShowConfirm(
                    $"{consumeItemData.itemName}을(를) 사용하시겠습니까?",
                    () => {
                        if (inventory.UseItem(consumeItemData))
                        {
                            ExecuteInteraction(inventory);
                        }
                    }
                );
            }
        }
        else
        {
            // 4. 소모할 아이템도 없고 hintText도 없다면 곧바로 상호작용 실행
            ExecuteInteraction(inventory);
        }
    }

    private void ExecuteInteraction(PlayerInventory inventory)
    {
        if (giveItemData != null)
        {
            inventory.AddItem(giveItemData);
        }

        isInteracted = true;

        OnInteracted?.Invoke();

        if (ifdestroy)
        {
            Destroy(gameObject);
        }
    }
}