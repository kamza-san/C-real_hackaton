using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "Gamescene";

    private void Awake()
    {
        Time.timeScale = 1f;
    }

    public void StartGame()
    {
        LoopManager.ResetAll();
        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}