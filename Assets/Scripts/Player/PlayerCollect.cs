using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// 触发器判断 收集物品
public class PlayerCollect : MonoBehaviour
{
    public int apple; // 收集apple数量

    public Text appleText; // 收集apple数量显示

    public GameObject panel;

    public Text panelText;

    private Animator anim; // 动画控制器

    private void Start()
    {
        anim = GetComponent<Animator>(); // 动画控制器赋值
    }

    private void OnTriggerEnter2D(Collider2D collision) // 收集物品 人物碰撞到2d标签时候的回调
    {
        if (
            collision.tag == "Collection" // 碰到可收集物品
        )
        {
            Collectable collectable =
                collision.gameObject.GetComponent<Collectable>();
            collectable.collectYes();
            apple += 1;
            appleText.text = apple.ToString() + "/5";
        }
        if (
            collision.tag == "NextLevel" // 碰到下一关
        )
        {
            if (apple >= 5)
            {
                SceneManager
                    .LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else
            {
                panelText.text = "还需收集" + (5 - apple) + "个苹果";
                panel.SetActive(true);
            }
        }
        if (
            collision.tag == "DieLine" // 碰到死亡线
        )
        {
            Invoke("Restart", 1f);
        }
        if (
            collision.tag == "Prickle" // 碰到陷阱
        )
        {
            SwitchAnim();
            Invoke("Restart", 1f);
        }
    }

    public void SwitchAnim() // 控制动画
    {
        anim.SetInteger("state", 5); // 转换成成整数类型并赋值动画变量
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "NextLevel")
        {
            panel.SetActive(false);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
