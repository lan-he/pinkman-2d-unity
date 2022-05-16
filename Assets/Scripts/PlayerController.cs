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

    public int speed; // 声明速度

    public int jumpForce; // 跳跃系数

    public bool isGround; // 是否在地面上&是否在跳跃

    public int jumpCount; // 跳跃次数

    public Transform groundCheck1; //地面监测点

    public Transform groundCheck2; //地面监测点

    public LayerMask ground; // 声明碰撞体

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
        isGround =
            Physics2D.OverlapCircle(groundCheck1.position, 0.1f, ground) ||
            Physics2D.OverlapCircle(groundCheck2.position, 0.1f, ground); // 判断是否接触地面
        if (Input.GetButtonDown("Jump"))
        {
            Jump(); // 角色跳跃
        }

        if (!isHurt)
        {
            Movement(); // 角色移动
        }

        SwitchAnim(); // 动画
    }

    // FixedUpdate固定时间执行
    private void FixedUpdate()
    {
    }

    void Movement()
    {
        //float horizontalMove = Input.GetAxis("Horizontal"); // 获取1到-1之间的数字
        float horizontalMove = Input.GetAxisRaw("Horizontal"); //获取-1,0,1这三个数
        rb.velocity = new Vector2(horizontalMove * speed, rb.velocity.y);
        anim.SetFloat("running", Mathf.Abs(horizontalMove));

        // 调整角色面部朝向
        if (horizontalMove != 0)
        {
            transform.localScale = new Vector3(horizontalMove, 1, 1);
        }
        else
        {
            anim.SetBool("idle", true);
        }
    }

    void Jump()
    {
        //角色跳跃
        if (isGround)
        {
            jumpCount = 2;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
        }
        else if (jumpCount > 0 && !isGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, 10);
            jumpCount--;
        }
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
                rb.velocity = new Vector2(10, rb.velocity.y);
                isHurt = true;
            }
            else if (
                transform.position.x < collision.gameObject.transform.position.x
            )
            {
                rb.velocity = new Vector2(-10, rb.velocity.y);
                isHurt = true;
            }
        }
    }
}
