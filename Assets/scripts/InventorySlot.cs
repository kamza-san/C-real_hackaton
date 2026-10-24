using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image iconImage;

    private ItemData currentItem;
    private InventoryUI ui;

    private void Awake()
    {
        Image img = GetComponent<Image>();
        if (img == null)
        {
            img = gameObject.AddComponent<Image>();
            img.color = Color.clear;
            img.raycastTarget = true;
        }
    }

    public void Setup(ItemData item, InventoryUI inventoryUI)
    {
        currentItem = item;
        ui = inventoryUI;

        if (item != null && iconImage != null)
        {
            iconImage.gameObject.SetActive(true);
            iconImage.sprite = item.image;
        }
        else if (iconImage != null)
        {
            iconImage.gameObject.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentItem != null && ui != null)
        {
            ui.ShowTooltip(currentItem, transform.position);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (ui != null)
        {
            ui.HideTooltip();
        }
    }

    private void OnDisable()
    {
        if (ui != null)
        {
            ui.HideTooltip();
        }
    }
}