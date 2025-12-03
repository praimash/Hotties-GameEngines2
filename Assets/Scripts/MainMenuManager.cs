using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("GamePlay");
    }
    public void OpenSettings()
    {
        Debug.Log("Settings opened");
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Settings opened");
    }
}