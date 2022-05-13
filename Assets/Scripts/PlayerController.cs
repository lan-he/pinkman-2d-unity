using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]private Rigidbody2D rb; // 声明缸体
    private Animator anim;

    public Collider2D coll;
    public float speed; // 声明速度
    public float jumpforce; // 跳跃系数
    public LayerMask ground; // 声明碰撞体
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); //缸体赋值
        anim = GetComponent<Animator>();
    }

    // Update 每一帧都回执行
    void Update()
    {
        //Movement();
    }
    // FixedUpdate固定时间执行
    void FixedUpdate()
    {
        Movement();
        SwitchAnim();
    }

    void Movement()
    {
        float horizontalmove = Input.GetAxis("Horizontal"); // 获取1到-1之间的数字
        float facedircetion = Input.GetAxisRaw("Horizontal"); //获取-1,0,1这三个数

        // 角色移动
        if (horizontalmove != 0)
        {
            rb.velocity = new Vector2(horizontalmove * speed, rb.velocity.y);
            anim.SetFloat("running", Mathf.Abs(facedircetion));
        }
        // 调整角色面部朝向
        if (facedircetion != 0)
        {
            transform.localScale = new Vector3(facedircetion, 1, 1);
        }
        //角色跳跃
        if (Input.GetButton("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpforce);
            anim.SetBool("jumping", true);
        }
    }

    void SwitchAnim()
    {
        anim.SetBool("idle", false);
        if (anim.GetBool("jumping"))
        {
            if (rb.velocity.y < 0)
            {
                anim.SetBool("jumping", false);
                anim.SetBool("falling", true);
            }

        }
        else if (coll.IsTouchingLayers(ground))
        {
            anim.SetBool("falling", false);
            anim.SetBool("idle", true);
        }
    }
}
