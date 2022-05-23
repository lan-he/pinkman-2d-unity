using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    Material material;
    Vector2 movement;

    public Vector2 speed;
    void Start()
    {
        material = GetComponent<Renderer>().material;
    }

    private void FixedUpdate()
    {
        movement += speed * Time.deltaTime;
        material.mainTextureOffset = movement;
    }
}
