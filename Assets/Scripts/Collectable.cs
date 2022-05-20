using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 可收集物品
public class Collectable : MonoBehaviour
{
    public void DestroyObject () {
        Destroy(gameObject);
    }
}
