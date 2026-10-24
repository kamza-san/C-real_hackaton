using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ItemPopupManager : MonoBehaviour
{
    public static ItemPopupManager Instance;

    [SerializeField] private GameObject confirmPanel;
    [SerializeField] private TextMeshProUGUI confirmText;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    private Action currentOnYes;

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

    private void Start()
    {
        if (confirmPanel != null)
        {
            confirmPanel.SetActive(false);
        }
    }

    // "사용하시겠습니까?" 확인창 띄우기
    public void ShowConfirm(string message, Action onYes)
    {
        if (confirmPanel == null) return;

        confirmPanel.SetActive(true);
        if (confirmText != null)
        {
            confirmText.text = message;
        }

        currentOnYes = onYes;

        Time.timeScale = 0f;

        yesButton.onClick.RemoveAllListeners();
        yesButton.onClick.AddListener(() => {
            currentOnYes?.Invoke(); 
            CloseConfirm();
        });

        noButton.onClick.RemoveAllListeners();
        noButton.onClick.AddListener(() => {
            CloseConfirm(); 
        });
    }

    // 팝업창 닫기
    public void CloseConfirm()
    {
        if (confirmPanel != null)
        {
            confirmPanel.SetActive(false);
        }
        currentOnYes = null;

        Time.timeScale = 1f;
    }
}