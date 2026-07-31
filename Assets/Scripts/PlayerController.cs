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

    private Vector3 playerPos;

    
    //キーを押した方向にプレイヤが向くようにする
    private SpriteRenderer spriteRenderer;
    [HideInInspector] public bool isFacingRight = true;

    //プレイヤのアニメーション
    Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        isFacingRight = true;
        playerPos = transform.position;
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
        animator.SetBool("Move", horizontalInput != 0);

        if(horizontalInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if(horizontalInput < 0 && isFacingRight)
        {
            Flip();
        }

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

    public void Flip()
    {
        isFacingRight = !isFacingRight;
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Horizontal"))
        {
            Debug.Log("MoveCamera_horizon called with player_x: " + transform.position.x);
            Cameramove.instance.MoveCamera_horizon(transform.position.x);
        }
        else if (collision.gameObject.CompareTag("Vertical"))
        {
            Debug.Log("MoveCamera_vertical called with player_y: " + transform.position.y);
            Cameramove.instance.MoveCamera_vertical(transform.position.y);
        }
        
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Harricane"))
        {
            rb.AddForce(new Vector2(0, 50));
        }
    }
}