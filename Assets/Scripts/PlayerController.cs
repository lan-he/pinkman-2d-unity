using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //[SerializeField] private Rigidbody2D rb; // 缸体 private私有变量
    private Rigidbody2D rb; // 缸体
    private Animator anim; // 动画控制器
    private Collider2D coll; // 碰撞体
    // 移动
    public int speed; // 声明速度
    private float moveX;
    private bool facingRight = true;
    // 跳跃
    [Range(1, 10)]
    public int jumpForce; // 跳跃系数
    private bool moveJump; // 跳跃输入
    private bool jumpHold; // 长按跳跃
    public bool isGround; // 是否在地面上
    public int jumpCount = 2; // 跳跃次数
    private bool isJump; // 传递作用
    public Transform groundCheck; //地面监测点
    public LayerMask ground; // 声明碰撞体图层
    private float fallAddition = 2.5f; // 下落重力加成
    private float jumpAddition = 1.5f; // 跳跃重力加成


    public int apple; // 收集apple数量
    public TextMeshProUGUI appleText; // 收集apple数量显示
    private bool isHurt; // 是否受伤
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); //缸体赋值
        anim = GetComponent<Animator>(); // 动画控制器赋值
        coll = GetComponent<Collider2D>();
    }

    // Update 每一帧都回执行
    private void Update()
    {
        moveX = Input.GetAxis("Horizontal"); //获取-1~1
        moveJump = Input.GetButtonDown("Jump"); // 按下就是一次true
        jumpHold = Input.GetButton("Jump"); // 按住就一直true

        if (moveJump && jumpCount > 0)
        {
            isJump = true;
        }
        SwitchAnim(); // 动画
    }

    // FixedUpdate固定时间执行
    private void FixedUpdate()
    {
        if (!isHurt)
        {
            Movement(); // 角色移动
        }
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, ground); // 判断是否接触地面
        Jump(); // 角色跳跃
    }

    void Movement() // 角色移动
    {
        rb.velocity = new Vector2(moveX * speed, rb.velocity.y);
        anim.SetFloat("running", Mathf.Abs(moveX));
        if ((facingRight == false && moveX > 0) || (facingRight == true && moveX < 0))
        {
            Filp();
        }
        else
        {
            anim.SetBool("idle", true);
        }
    }
    private void Filp()
    {
        facingRight = !facingRight;
        Vector3 playerScale = transform.localScale;
        playerScale.x *= -1;
        transform.localScale = playerScale;
    }

    private void Jump()
    {
        if (isGround)
        {
            jumpCount = 2;
        }
        if (isJump)
        {
            rb.velocity = Vector2.up * jumpForce;
            // rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpCount--;
            isJump = false;
        }

        if (rb.velocity.y < 0)
        {
            rb.gravityScale = fallAddition;
        }
        else if (rb.velocity.y > 0 && !jumpHold)
        {
            rb.gravityScale = jumpAddition;
        }
        else
        {
            rb.gravityScale = 1f;
        }
        // //角色跳跃
        // if (isGround)
        // {
        //     jumpCount = 2;
        //     rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        //     jumpCount--;
        // }
        // else if (jumpCount > 0 && !isGround)
        // {
        //     rb.velocity = new Vector2(rb.velocity.x, 10);
        //     jumpCount--;
        // }
    }

    // 控制动画
    void SwitchAnim()
    {
        // 如果在空中
        if (!isGround)
        {
            // anim.SetBool("idle", false);
            // 同时缸体的位移小于0说明在下降
            if (rb.velocity.y < 0)
            {
                anim.SetBool("jumping", false);
                anim.SetBool("falling", true);
            }
            else
            {
                anim.SetBool("jumping", true);
                anim.SetBool("falling", false);
            }
        }
        else
        {
            // 碰到地面了
            anim.SetBool("jumping", false);
            anim.SetBool("falling", false);
            anim.SetBool("idle", true);
        }

        if (isHurt)
        {
            anim.SetBool("hurt", true);
            if (Mathf.Abs(rb.velocity.x) < 0.1f)
            {
                isHurt = false;
                anim.SetBool("hurt", false);
                anim.SetBool("idle", true);
            }
        }
    }

    // 收集物品
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collection")
        {
            Destroy(collision.gameObject);
            apple += 1;
            appleText.text = apple.ToString();
        }
    }

    // 消灭敌人
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (anim.GetBool("falling"))
            {
                Destroy(collision.gameObject);
                rb.velocity = new Vector2(rb.velocity.x, 8);
            }
            else if (
                transform.position.x > collision.gameObject.transform.position.x
            )
            {
                rb.velocity = new Vector2(6, rb.velocity.y);
                isHurt = true;
            }
            else if (
                transform.position.x < collision.gameObject.transform.position.x
            )
            {
                rb.velocity = new Vector2(-6, rb.velocity.y);
                isHurt = true;
            }
        }
    }
}
