using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemysFrog : MonoBehaviour
{
    private Rigidbody2D rb;
    public Transform leftPoint;
    public Transform rightPoint;
    public float speed;
    private float leftx;
    private float rightx;
    private bool faceLeft = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        leftx = leftPoint.position.x;
        rightx = rightPoint.position.x;
        Destroy(leftPoint.gameObject);
        Destroy(rightPoint.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }
    private void Movement()
    {
        if (faceLeft)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            if (transform.position.x < leftx)
            {
                transform.localScale = new Vector3(1, 1, 1);
                faceLeft = false;
            }
        }
        else
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            if (transform.position.x > rightx)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                faceLeft = true;
            }
        }
    }
}
