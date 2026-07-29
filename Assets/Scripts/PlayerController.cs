using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5.0f;

    [SerializeField]
    private float jumpPower = 8.0f;

    [SerializeField]
    private Transform groundCheck;

    [SerializeField]
    private float groundCheckRadius = 0.15f;

    [SerializeField]
    private LayerMask groundLayer;

    private Rigidbody2D rb;
    private float horizontalInput;
    private bool jumpRequested;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // A・Dまたは左右矢印キー
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // Spaceを押し、地面にいる場合だけジャンプ
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            jumpRequested = true;
        }
    }

    private void FixedUpdate()
    {
        Vector2 velocity = rb.velocity;

        // 横方向の速度を変更
        velocity.x = horizontalInput * moveSpeed;

        // 縦方向にジャンプ速度を設定
        if (jumpRequested)
        {
            velocity.y = jumpPower;
            jumpRequested = false;
        }

        rb.velocity = velocity;
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        ) != null;
    }
}