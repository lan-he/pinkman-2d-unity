using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 敌人脚本
public class EnemyMushroom : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim; // 动画控制器
    public Transform thisTransforms;
    public Transform leftPoint;
    public Transform rightPoint;
    public AudioSource stepon;
    public Collider2D disColl;
    public float speed;
    private float leftx;
    private float rightx;
    private bool faceLeft = false;
    private bool dieing = false;
    private float rotationX;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        leftx = leftPoint.position.x;
        rightx = rightPoint.position.x;
        Destroy(leftPoint.gameObject);
        Destroy(rightPoint.gameObject);
    }
    void Update()
    {
        Movement();
       
        if (dieing)
        {
             rotationX += Time.deltaTime * 10 * 3;
            thisTransforms.rotation = Quaternion.Euler(0, 0, rotationX);
            // rb.velocity = new Vector2(rb.velocity.x, rotationX);
        }

    }
    private void Movement()
    {
        if (faceLeft)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            if (transform.position.x < leftx)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                faceLeft = false;
            }
        }
        else
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            if (transform.position.x > rightx)
            {
                transform.localScale = new Vector3(1, 1, 1);
                faceLeft = true;
            }
        }
    }
    public void DestroyGameObject()
    {
        Destroy(gameObject);
    }
    public void HitAnimator()
    {
        disColl.enabled = false;
        rb.velocity = new Vector2(rb.velocity.x, 5);
        dieing = true;
        stepon.Play();
        anim.SetTrigger("die");
        Invoke("DestroyGameObject", 2f);
    }
}
