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
        if (consumeItemData != null)
        {
            return inventory.items.Contains(consumeItemData);
        }
        return true;
    }

    public void Interact(GameObject player)
    {
        PlayerInventory inventory = player.GetComponent<PlayerInventory>();
        if (inventory == null || !CanInteract(inventory)) return;

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