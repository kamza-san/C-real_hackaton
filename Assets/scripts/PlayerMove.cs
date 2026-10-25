using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    
    private Rigidbody2D rb;
    private Vector2 moveDir;
    public Vector2 MoveDir => moveDir;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f; 
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveDir.normalized * moveSpeed;
    }
    public void OnMove(InputValue value)
    {
        moveDir = value.Get<Vector2>();
    }
}