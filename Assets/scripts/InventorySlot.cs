using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Image iconImage;

    private ItemData currentItem;
    private InventoryUI ui;
    public Image slotBackground;

    private void Awake()
    {
        slotBackground = GetComponent<Image>();
        if (slotBackground == null)
        {
            slotBackground = gameObject.AddComponent<Image>();
            slotBackground.color = Color.clear;
            slotBackground.raycastTarget = true;
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

        // UI가 갱신될 때 현재 선택된 아이템과 일치하면 선택 상태 표시 유지
        UpdateSelectionVisual();
    }

    // [핵심] 슬롯을 클릭했을 때 호출되는 함수
    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentItem == null || PlayerInventory.Instance == null) return;

        // 1. 이미 선택된 아이템을 또 누른 경우 -> 선택 해제 (null)
        if (PlayerInventory.Instance.selectItem == currentItem)
        {
            PlayerInventory.Instance.selectItem = null;
            Debug.Log("아이템 선택 해제");
        }
        // 2. 다른 아이템이거나 아무것도 안 골랐던 경우 -> 새로운 아이템으로 선택
        else
        {
            PlayerInventory.Instance.selectItem = currentItem;
            Debug.Log($"아이템 선택됨: {currentItem.itemName}");
        }

        // 인벤토리 전체 슬롯들의 시각적 효과(테두리 등) 갱신을 위해 UI 전체 새로고침
        if (ui != null)
        {
            ui.RefreshUI();
        }
    }

    // 선택 여부에 따라 슬롯의 시각적 요소를 바꿔주는 함수 (선택되면 색상 변경 등)
    private void UpdateSelectionVisual()
    {
        if (PlayerInventory.Instance == null) return;

        bool isSelected = (currentItem != null && PlayerInventory.Instance.selectItem == currentItem);

        if (slotBackground != null)
        {
            // 예시: 선택되면 슬롯 배경을 반투명 노란색으로, 아니면 원래 투명하게 변경
            // (만약 별도의 테두리 이미지 오브젝트가 있다면 그 오브젝트를 켜고 끄도록 수정하셔도 됩니다)
            if (currentItem != null)
            {
                slotBackground.color = isSelected ? new Color32(255, 215, 0, 200) : new Color32(72, 72, 72, 200);
            }
            else
            {
                slotBackground.color = Color.clear;
            }
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