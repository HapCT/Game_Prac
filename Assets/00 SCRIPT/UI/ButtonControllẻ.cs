using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ButtonController : MonoBehaviour
{
    public void ExitGame()
    {
        Application.Quit();
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("Menu-Level");
    }
    public void RestartGame()
    {
        GameManager.instance.Respawn();
        SceneManager.UnloadSceneAsync("Restart");
    }
    public void LoadLevel(int level)
    {
        SceneManager.LoadScene("Level" + level);
    }
}
