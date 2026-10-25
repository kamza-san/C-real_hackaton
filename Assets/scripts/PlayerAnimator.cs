using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PlayerMove))]
public class PlayerAnimator : MonoBehaviour
{
    private const int FramesPerDir = 4;

    public Sprite[] frames = new Sprite[16];
    public float framesPerSecond = 8f;

    private SpriteRenderer sr;
    private PlayerMove move;
    private int dir;
    private float timer;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        move = GetComponent<PlayerMove>();
        SetFrame(0);
    }

    private void Update()
    {
        if (frames == null || frames.Length < FramesPerDir * 4) return;

        Vector2 input = move.MoveDir;
        bool moving = input.sqrMagnitude > 0.01f;

        if (moving)
        {
            if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
                dir = input.x > 0 ? 1 : 3;
            else
                dir = input.y > 0 ? 2 : 0;

            timer += Time.deltaTime;
            SetFrame((int)(timer * framesPerSecond) % FramesPerDir);
        }
        else
        {
            timer = 0f;
            SetFrame(0);
        }
    }

    private void SetFrame(int frame)
    {
        Sprite s = frames[dir * FramesPerDir + frame];
        if (s != null) sr.sprite = s;
    }
}
