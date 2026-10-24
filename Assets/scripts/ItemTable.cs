using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ItemTable : MonoBehaviour, IInteractable
{
    [SerializeField] private bool ifdestroy = true;
    [SerializeField] private ItemData giveItemData;
    [SerializeField] private ItemData consumeItemData;

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

        if (consumeItemData != null)
        {
            if (!inventory.items.Contains(consumeItemData)) // 지금 이거에 상호작용하기 위한 아이템이 존재하는지가 false라면
            {
                ShowNoItemMessage(); // 얘 호출(그 상호작용할수 없다라고 뜨게하는거)
                return;
            }

            
            if (ItemPopupManager.Instance != null)
            {
                ItemPopupManager.Instance.ShowConfirm(
                    $"{consumeItemData.itemName}을(를) 사용하시겠습니까?",
                    () =>
                    {
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
            ExecuteInteraction(inventory);
        }
    }

    private void ShowNoItemMessage()
    {
        if (ItemPopupManager.Instance != null)
        {
            ItemPopupManager.Instance.ShowConfirm( // 위에있는 알림창 재탕
                    "사용할수 없다.", // 일단 임시로 이렇게 해둔거 나중에 바꿔야할듯
                    () => { }
            );
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