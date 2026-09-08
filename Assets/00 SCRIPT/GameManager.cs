using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager instance => _instance;
    [SerializeField] Transform _spawnPlayer;
    [SerializeField] PlayerController player;
    int maxLevels = 1;

    private void Awake()
    {
        if(instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        maxLevels = PlayerPrefs.GetInt("MaxLevel", 1);
    }
    public void LoadLevel(int level)
    {
        if(level > maxLevels)
        {
            return;
        }
        SceneManager.LoadScene("Level" + level);
    }
    public void FinishController()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        CompleteLevel(currentLevel);
    }


    public void CompleteLevel(int level)
    {
        if (level >= maxLevels)
        {
            maxLevels = level + 1;
            PlayerPrefs.SetInt("MaxLevel", maxLevels);
            PlayerPrefs.Save();
        }
        SceneManager.LoadScene("Level" + level + 1);
    }
    public bool IsLevelUnlocked(int level)
    {
        return level <= maxLevels;
    }
    public void Respawn()
    {
        player.gameObject.SetActive(true);
        player.transform.position = _spawnPlayer.position;
        player.ResetPlayer();

    }   

}
