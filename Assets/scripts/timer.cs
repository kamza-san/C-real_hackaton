using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;   // 시간표시용
    public float timeLimit =61f;      // 제한시간(인데 61로 하는이유는 60으로하면 인게임에서 59로 시작함 이유를 모르겠음;;;;;;;)
    private float currentTime; // 지금 시간 얼마야
    private bool isRunning =true; // 타이머 돌아가는거 표시용

    void Start()
    {
        currentTime = timeLimit;
    }

    void Update()
    {
        if (!isRunning) return;

        currentTime -= Time.deltaTime; // 1초씩 빼는용도


        // 여긴 게임오버 처리하는곳
        if (currentTime <= 0f)
        {
            currentTime = 0f;
            isRunning = false;
            OnTimeOver();
        }

        timerText.text =((int)currentTime).ToString();
    }

    void OnTimeOver()
    {
        Debug.Log("님 죽음"); //일단 임시
    }
}