using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ItemTable : MonoBehaviour, IInteractable
{
    public enum InteractionType
    {
        GiveItem,
        ConsumeItem
    }

    [Header("Interaction Setting")]
    [SerializeField] private InteractionType interactionType;
    [SerializeField] private bool ifdestroy = true;
    [SerializeField] private ItemData itemData;

    public bool isInteracted = false;

    public event Action OnInteracted;

    public bool CanInteract(PlayerInventory inventory)
    {
        if (isInteracted) return false;

        if (interactionType == InteractionType.GiveItem)
        {
            return true;
        }
        else if (interactionType == InteractionType.ConsumeItem)
        {
            if (inventory != null && itemData != null)
            {
                return inventory.items.Contains(itemData);
            }
            return false;
        }

        return false;
    }

    public void Interact(GameObject player)
    {
        PlayerInventory inventory = player.GetComponent<PlayerInventory>();
        if (inventory == null || !CanInteract(inventory)) return;

        if (interactionType == InteractionType.GiveItem)
        {
            if (itemData != null)
            {
                inventory.AddItem(itemData);
                isInteracted = true;
                OnInteracted?.Invoke();

                if (ifdestroy)
                {
                    Destroy(gameObject);
                }
            }
        }
        else if (interactionType == InteractionType.ConsumeItem)
        {
            if (itemData != null)
            {
                if (ItemPopupManager.Instance != null)
                {
                    ItemPopupManager.Instance.ShowConfirm(
                        $"{itemData.itemName}을(를) 사용하시겠습니까?", 
                        () => {
                            if (inventory.UseItem(itemData))
                            {
                                isInteracted = true;
                                OnInteracted?.Invoke();

                                if (ifdestroy)
                                {
                                    Destroy(gameObject);
                                }
                            }
                        }
                    );
                }
            }
        }
    }
}