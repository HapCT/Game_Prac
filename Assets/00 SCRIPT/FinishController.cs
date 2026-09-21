using UnityEngine;

public class FinishController : MonoBehaviour
{

    private static FinishController _instance;
    public static FinishController instance => _instance;
    [SerializeField] private GameObject _finishUI;

    private void Awake()
    {
        if (instance == null)
        {
            _instance = this;

        }
        else if (_instance != this)
        {
            Destroy(gameObject);
            return;
        }
        if (_finishUI != null)
            _finishUI.SetActive(false);
    }
    private bool AllEnemiesDead()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        return enemies.Length == 0;
    }
    public void CheckEnemyDie()
    {
        if (AllEnemiesDead())
        {
            if (_finishUI != null)
                _finishUI.SetActive(true);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player"))
            return;

        if (!AllEnemiesDead())
        {
            // Còn enemy sống -> chạm Finish không có tác dụng
            Debug.Log("Còn enemy sống, chưa thể qua màn!");
            return;
        }

        // Hết enemy + chạm Finish -> qua màn
        GameManager.instance.FinishController();
    }

}