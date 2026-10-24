using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    // [싱글톤 인스턴스]
    public static PlayerInventory Instance { get; set; }

    public List<ItemData> items = new List<ItemData>(); 

    public InventoryUI inventoryUI; 
    public ItemData selectItem = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddItem(ItemData newItem)
    {
        items.Add(newItem);
    
        if (inventoryUI != null)
        {
            inventoryUI.RefreshUI();
        }
    }

    public bool UseItem(ItemData targetItem)
    {
        if (items.Contains(targetItem))
        {
            items.Remove(targetItem);

            if (selectItem == targetItem)
            {
                selectItem = null;
            }

            if (inventoryUI != null)
            {
                inventoryUI.RefreshUI();
            }
            return true;
        }
        return false;
    }
}