using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("가진 아이템 목록")]
    public List<ItemData> items = new List<ItemData>(); 

    [Header("항상 떠 있는 UI 매니저")]
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
}