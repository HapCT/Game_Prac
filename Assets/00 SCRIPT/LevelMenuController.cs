using UnityEngine;

public class LevelMenuController : MonoBehaviour
{
    [SerializeField] GameObject[] levelButtons;

    private void Start()
    {
        int maxLevel = PlayerPrefs.GetInt("MaxLevel", 1);

        for (int i = 0; i < levelButtons.Length; i++)
        {
            levelButtons[i].SetActive(i + 1 <= maxLevel);
        }
    }
}