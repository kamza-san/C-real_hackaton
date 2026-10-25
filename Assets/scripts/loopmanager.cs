using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-100)]
public class LoopManager : MonoBehaviour
{
    [SerializeField] private float loopTime = 60f;
    [SerializeField] private float blackHoldTime = 1.5f;
    [SerializeField] private Camera cam;
    [SerializeField] private Canvas[] canvasesToHide;

    [Header("총소리")]
    [SerializeField] private AudioSource gunshotSource;
    [SerializeField] private AudioClip gunshotClip;
    [SerializeField] private float shotToBlackDelay = 0.3f;

    [Header("캐릭터")]
    [SerializeField] private GameObject[] characters;
    [SerializeField] private bool avoidSameAsLast = true;

    public static bool IsEnding { get; private set; }
    public static bool IsCleared { get; private set; }

    public static void SetCleared() => IsCleared = true;

    private static int lastCharacterIndex = -1;
    private static readonly HashSet<string> doneIds = new HashSet<string>();

    public static void MarkDone(string id) => doneIds.Add(id);
    public static bool IsDone(string id) => doneIds.Contains(id);

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatics()
    {
        ResetAll();
    }

    private float currentTime;

    private void Awake()
    {

        Time.timeScale = 1f;
        IsEnding = false;
        IsCleared = false;
        // 어떤 경로로든 0이 남아 있어도 새 루프는 항상 정상 속도로 시작
        Time.timeScale = 1f;
        IsEnding = false;

        if (characters == null || characters.Length == 0) return;

        int index = PickCharacterIndex();
        lastCharacterIndex = index;

        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].SetActive(i == index);
        }
    }

    private int PickCharacterIndex()
    {
        if (!avoidSameAsLast || lastCharacterIndex < 0 || characters.Length == 1)
        {
            return Random.Range(0, characters.Length);
        }

        int r = Random.Range(0, characters.Length - 1);
        return r >= lastCharacterIndex ? r + 1 : r;
    }

    private void Start()
    {
        currentTime = loopTime;
        if (cam == null) cam = Camera.main;
    }

    private void Update()
    {

        if (IsEnding || IsCleared) return;
        if (IsEnding) return;

        currentTime -= Time.deltaTime;
        if (currentTime <= 0f)
        {
            StartCoroutine(EndLoop());
        }
    }

    private IEnumerator EndLoop()
    {
        IsEnding = true;

        if (ItemPopupManager.Instance != null)
        {
            ItemPopupManager.Instance.CloseConfirm();
        }
        Time.timeScale = 1f;

        if (gunshotSource != null && gunshotClip != null)
        {
            gunshotSource.PlayOneShot(gunshotClip);
        }
        yield return new WaitForSecondsRealtime(shotToBlackDelay);

        // 한 번에 암전
        if (cam != null)
        {
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = Color.black;
            cam.cullingMask = 0;
        }

        foreach (Canvas c in canvasesToHide)
        {
            if (c != null) c.enabled = false;
        }

        yield return new WaitForSecondsRealtime(blackHoldTime);

        // 재시작 직전에 한 번 더 정리
        if (ItemPopupManager.Instance != null)
        {
            ItemPopupManager.Instance.CloseConfirm();
        }
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static void ResetAll()
    {
        lastCharacterIndex = -1;
        doneIds.Clear();
        IsEnding = false;
        IsCleared = false;
    }
}