using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
// UI========

public class PlayerCollect : MonoBehaviour
{
    public int apple; // 收集apple数量
    public Text appleText; // 收集apple数量显示
    public GameObject panel;
    public Text panelText;

    private void OnTriggerEnter2D(Collider2D collision) // 收集物品 人物碰撞到2d标签时候的回调
    {
        if (collision.tag == "Collection")
        {
            Collectable collectable = collision.gameObject.GetComponent<Collectable>();
            collectable.collectYes();
            apple += 1;
            appleText.text = apple.ToString() + "/5";
        }
        if (collision.tag == "NextLevel")
        {
            if (apple >= 5)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else
            {
                panelText.text = "还需收集" + (5 - apple) + "个苹果";
                panel.SetActive(true);
            }

        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "NextLevel")
        {
            panel.SetActive(false);
        }
    }
}
