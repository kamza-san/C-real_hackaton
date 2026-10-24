using TMPro; // TMP 필수 네임스페이스
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public PlayerInventory playerInventory;
    
    public Transform slotContainer; 
    public GameObject slotPrefab;   

    public GameObject tooltipPanel;
    public TextMeshProUGUI tooltipName;
    public TextMeshProUGUI tooltipExplain;
    
    public Vector3 tooltipOffset = new Vector3(0, -60f, 0); 

    private void Start()
    {
        if (tooltipPanel != null)
        {
            CanvasGroup cg = tooltipPanel.GetComponent<CanvasGroup>();
            if (cg == null)
            {
                cg = tooltipPanel.AddComponent<CanvasGroup>();
            }
            cg.blocksRaycasts = false;
        }

        RefreshUI();
        HideTooltip(); 
    }

public void RefreshUI()
    {
        if (playerInventory == null || slotContainer == null || slotPrefab == null) return;

        for (int i = slotContainer.childCount - 1; i >= 0; i--)
        {
            Transform child = slotContainer.GetChild(i);
            child.gameObject.SetActive(false);
            DestroyImmediate(child.gameObject);
        }

        foreach (ItemData item in playerInventory.items)
        {
            GameObject newSlot = Instantiate(slotPrefab, slotContainer);
            newSlot.transform.localScale = Vector3.one;
            newSlot.transform.localPosition = Vector3.zero;

            InventorySlot slotScript = newSlot.GetComponent<InventorySlot>();
            if (slotScript != null)
            {
                slotScript.Setup(item, this);
            }
        }

        if (slotContainer is RectTransform rectTransform)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        }
    }

    public void ShowTooltip(ItemData item, Vector3 slotPosition)
    {
        if (item == null || tooltipPanel == null) return;

        tooltipPanel.SetActive(true);
        tooltipPanel.transform.SetAsLastSibling();

        if (tooltipName != null) tooltipName.text = item.itemName;
        if (tooltipExplain != null) tooltipExplain.text = item.explain;

        Vector3 targetPos = slotPosition + tooltipOffset;

        if (tooltipPanel.transform is RectTransform rt)
        {
            float halfWidth = rt.rect.width * 0.5f;
            float halfHeight = rt.rect.height * 0.5f;
            targetPos.x = Mathf.Clamp(targetPos.x, halfWidth, Screen.width - halfWidth);
            targetPos.y = Mathf.Clamp(targetPos.y, halfHeight, Screen.height - halfHeight);
        }

        tooltipPanel.transform.position = targetPos;
    }

    public void HideTooltip()
    {
        if (tooltipPanel != null)
        {
            tooltipPanel.SetActive(false);
        }
    }
}