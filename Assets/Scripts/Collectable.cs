using UnityEngine;
// 可收集物品
public class Collectable : MonoBehaviour
{
    private Animator anim; // 动画控制器
    public AudioSource collectAudio;
    private void Start()
    {
        anim = GetComponent<Animator>(); // 动画控制器赋值
    }
    public void DestroyObject()
    {
        Destroy(gameObject);
    }
    public void collectYes()
    {
        collectAudio.Play();
        anim.SetTrigger("destroy");
    }
}
