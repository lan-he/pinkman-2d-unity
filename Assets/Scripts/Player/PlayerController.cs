using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
// 角色控制
public class PlayerController : MonoBehaviour
{
    //[SerializeField] private Rigidbody2D rb; // 缸体 private私有变量
    private Rigidbody2D rb; // 缸体
    private Animator anim; // 动画控制器
    private Collider2D coll; // 碰撞体
    public ParticleSystem playerPs; // 粒子系统
    // 移动=========
    public int speed; // 声明速度
    private float moveX;
    private bool facingRight = true;
    // 跳跃========
    [Range(1, 10)]
    public int jumpForce; // 跳跃系数
    private bool moveJump; // 跳跃输
    private bool jumpHold; // 长按跳跃
    public bool isGround; // 是否在地面上
    public int jumpCount = 2; // 跳跃次数
    private bool isJump; // 传递作用
    public Transform groundCheck; //地面监测点
    public LayerMask ground; // 声明碰撞体图层
    [SerializeField]
    private Vector2 boxSize;
    private float fallAddition = 2.5f; // 下落重力加成
    private float jumpAddition = 1.5f; // 跳跃重力加成
    public AudioSource jumpAudio;
    // 受伤=======
    private bool isHurt; // 是否受伤
    // 动画=======
    private enum playerState
    {
        idle,
        run,
        jump,
        fall,
        hurt
    } // 枚举 {静止,跑动,起跳,降落}
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
            ParticleSystem();
        }
        if (!isHurt)
        {
            Movement();
            Jump();
        }
        SwitchAnim();
    }
    private void FixedUpdate() // FixedUpdate固定时间执行
    {
        CheckGround();
    }
    void Movement() // 角色移动
    {
        rb.velocity = new Vector2(moveX * speed, rb.velocity.y); // 横向施加一个力

        // anim.SetFloat("running", Mathf.Abs(moveX)); 改变动画参数的状态
        if (
            (facingRight == false && moveX > 0) ||
            (facingRight == true && moveX < 0)
        )
        {
            Filp();
        }
    }
    private void Filp() // 角色反转
    {
        facingRight = !facingRight;
        Vector3 playerScale = transform.localScale;
        playerScale.x *= -1;
        transform.localScale = playerScale;
        ParticleSystem();
    }
    private void CheckGround()
    {
        // isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, ground); // 判断是否接触地面 圆形第二个参数是圆的半径
        isGround =
            Physics2D.OverlapBox(groundCheck.position, boxSize, 0, ground); // 判断是否接触地面 方形第二个参数是v2,第三个参数是角度
    }
    private void OnDrawGizmos()
    {
        // Gizmos.DrawWireSphere(groundCheck.position, 0.1f); // 可视化圆形物体
        Gizmos.DrawWireCube(groundCheck.position, boxSize); // 可视化盒子
        Gizmos.color = Color.red;
    }
    private void Jump() // 角色跳跃
    {
        if (isGround)
        {
            jumpCount = 1;
        }
        if (isJump)
        {
            jumpAudio.Play();
            rb.velocity = Vector2.up * jumpForce;
            // rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); // 施加一个瞬间的力
            jumpCount--;
            isJump = false;
        }

        if (rb.velocity.y < 0)
        {
            rb.gravityScale = fallAddition; // 修改缸体的重力
        }
        else if (rb.velocity.y > 0 && !jumpHold)
        {
            rb.gravityScale = jumpAddition; // 修改缸体的重力
        }
        else
        {
            rb.gravityScale = 1f; // 重置缸体重力
        }
    }
    void SwitchAnim() // 控制动画
    {
        playerState states; // 由枚举对象赋值
        if (Mathf.Abs(moveX) > 0)
        {
            states = playerState.run;
        }
        else
        {
            states = playerState.idle;
        }
        if (rb.velocity.y > 0.1f)
        {
            states = playerState.jump;
        }
        else if (rb.velocity.y < -0.1f)
        {
            states = playerState.fall;
        }
        if (isHurt)
        {
            states = playerState.hurt;
        }
        anim.SetInteger("state", (int)states); // 转换成成整数类型并赋值动画变量
    }
    private void ParticleSystem() // 粒子系统
    {
        playerPs.Play();
    }
    private void OnCollisionEnter2D(Collision2D collision) // 消灭敌人 缸体之间触碰的回调
    {
        if (
            collision.gameObject.tag == "Enemy" // 碰撞的是敌人
        )
        {
            EnemyMushroom mushroom =
                collision.gameObject.GetComponent<EnemyMushroom>();
            if (!isGround && rb.velocity.y < -0.1f)
            {
                mushroom.HitAnimator();
                // Destroy(collision.gameObject);
                rb.velocity = new Vector2(rb.velocity.x, 6);
            }
            else if (
                transform.position.x > collision.gameObject.transform.position.x
            )
            {
                rb.velocity = new Vector2(2, rb.velocity.y);
                isHurt = true;
            }
            else if (
                transform.position.x < collision.gameObject.transform.position.x
            )
            {
                rb.velocity = new Vector2(-2, rb.velocity.y);
                isHurt = true;
            }
        }
    }
    public void HurtDown() // 受伤结束 由动画事件触发
    {
        rb.velocity = new Vector2(0, 0);
        isHurt = false;
    }
    void KillPlayer()
    {
        // Invoke("KillPlayer",dieTime); // 延迟事件
    }
}
