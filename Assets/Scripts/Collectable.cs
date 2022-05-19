using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public void DestroyObject () {
        Destroy(gameObject);
    }
}
