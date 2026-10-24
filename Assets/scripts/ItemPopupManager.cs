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
    public void ShowConfirm(string message, Action onYes)
    {
        if (confirmPanel == null) return;

        confirmPanel.SetActive(true);
        noButton.gameObject.SetActive(true); // 밑에 함수에서 no 버튼 보이는걸 false로 해놔서 혹시 몰라서 해둠
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

    public void CantInterect(string message)
    {
        if (confirmPanel == null) return; 

        confirmPanel.SetActive(true);

        confirmText.text = message;

        Time.timeScale = 0f;

        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();

        yesButton.onClick.AddListener(() =>
        {
            CloseConfirm();
        });

        noButton.gameObject.SetActive(false); // no버튼 숨기는거
    }

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