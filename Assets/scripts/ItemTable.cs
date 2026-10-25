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
        if (PlayerInventory.Instance == null) return false;
        
        return true;
    }

    public void Interact(GameObject player)
    {
        // 1. GetComponent 대신 싱글톤 패턴 활용
        PlayerInventory inventory = PlayerInventory.Instance;
        if (inventory == null || !CanInteract(inventory)) return;

        // 2. 소모할 아이템이 필요한데, 플레이어가 "손에 들고(selectItem) 있지 않은" 경우
        // (인벤토리에 있더라도 선택하지 않았으면 상호작용 실패 처리)
        if (consumeItemData != null && inventory.selectItem != consumeItemData)
        {
            if (ItemPopupManager.Instance != null)
            {
                // 아이템이 없거나 들고있지 않을 때 힌트 팝업
                ItemPopupManager.Instance.CantInterect(hintText);
            }
            return;
        }

        // 3. 소모할 아이템은 없는데(null), hintText에 메시지가 적혀있는 경우 -> 예/아니오 팝업
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

        // 4. 정상적으로 소모할 아이템을 "손에 들고" 있는 경우 (확인 팝업)
        if (consumeItemData != null)
        {
            if (ItemPopupManager.Instance != null)
            {
                ItemPopupManager.Instance.ShowConfirm(
                    $"{consumeItemData.itemName}을(를) 사용하시겠습니까?",
                    () => {
                        // UseItem 호출 시 인벤토리에서 소모하고 selectItem도 null로 알아서 초기화 됨
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
            // 5. 소모할 아이템도 없고 hintText도 없다면 곧바로 상호작용 실행
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