using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<ItemData> items = new List<ItemData>(); 

    public InventoryUI inventoryUI; 

    public void AddItem(ItemData newItem)
    {
        items.Add(newItem);
        Debug.Log($"획득: {newItem.itemName}");
    
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
            inventoryUI.RefreshUI();
            return true;
        }
        return false;
    }
}