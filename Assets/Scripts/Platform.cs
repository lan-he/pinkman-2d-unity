using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    PlatformEffector2D pe2d;
    void Start()
    {
        pe2d = GetComponent<PlatformEffector2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            pe2d.rotationalOffset = 180f;
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            pe2d.rotationalOffset = 0f;
        }

    }
}
