using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    //[SerializeField] private Rigidbody2D rb; // 缸体 private私有变量
    public Rigidbody2D rb;
    public Animator anim; // 动画控制器
    public Collider2D coll; // 碰撞体

    public float speed, jumpForce = 9; // 声明速度&跳跃系数
    public bool isGround,isJump; // 是否在地面上&是否在跳跃 
    //public bool jumpPressed; // 按键是否被按下
    public int jumpCount; // 跳跃次数

    public Transform groundCheck; //地面监测点
    public LayerMask ground; // 声明碰撞体
    public int apple; // 收集apple数量
    public TextMeshProUGUI appleText; // 收集apple数量显示


    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); //缸体赋值
        anim = GetComponent<Animator>(); // 动画控制器赋值
        coll = GetComponent<Collider2D>();
    }

    // Update 每一帧都回执行
    void Update()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, ground);
        if (isGround)
        {
            jumpCount = 2;
            isJump = false;
        }
        if (Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            Jump(); // 角色跳跃
        }
        SwitchAnim(); // 动画
    }
    // FixedUpdate固定时间执行
    private void FixedUpdate()
    {
        Movement(); // 角色移动
        //Jump(); // 角色跳跃
        //SwitchAnim(); // 动画
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
        else {
            anim.SetBool("idle", true);
        }
    }
    void Jump()
    {
        isJump = true;
        //在地面上
        //角色跳跃
        if (isGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            anim.SetBool("jumping", true);
            //jumpPressed = false;
        }
        else if (jumpCount > 0 && isJump) {
            rb.velocity = new Vector2(rb.velocity.x, 3);
            jumpCount--;
            anim.SetBool("jumping", true);
            //jumpPressed = false;
        }
    }

    // 控制动画
    void SwitchAnim()
    {
        // 每次进入都先默认非idle状态
        //anim.SetBool("idle", true);
        // 如果在跳
        if (!isGround)
        {
            // 同时缸体的位移小于0说明在下降
            if (rb.velocity.y < 0)
            {
                anim.SetBool("jumping", false);
                anim.SetBool("falling", true);
            }
            else {
                anim.SetBool("jumping", true);
                anim.SetBool("falling", false);
            }

        }
        else
        {
            // 碰到地面了
            anim.SetBool("falling", false);
            anim.SetBool("idle", true);
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
}
