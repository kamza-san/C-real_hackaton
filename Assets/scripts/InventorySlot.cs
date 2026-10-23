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
        // 슬롯 루트에 Graphic 컴포넌트가 없으면 마우스 Hover 감지가 누락될 수 있으므로 투명 Image 자동 보장
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
            Debug.Log($"[InventorySlot] 마우스 오버: {currentItem.itemName}");
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