using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(ItemTable))]
public class GameClear : MonoBehaviour
{
    [SerializeField] private string endingSceneName = "Ending";   // 엔딩 씬 이름

    private ItemTable table;

    private void Awake()
    {
        table = GetComponent<ItemTable>();
    }

    private void OnEnable()
    {
        table.OnInteracted += OnClear;
    }

    private void OnDisable()
    {
        table.OnInteracted -= OnClear;
    }

    private void OnClear()
    {
        if (LoopManager.IsCleared) return;   // 중복 로드 방지

        LoopManager.SetCleared();            // 루프 타이머 정지
        SceneManager.LoadScene(endingSceneName);
    }
}