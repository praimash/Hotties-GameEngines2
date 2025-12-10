using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public SettingsManager settingsManager;
    public void StartGame()
    {
        SceneManager.LoadScene("GamePlay");
    }
    public void OpenSettings()
    {
        settingsManager.OpenSettings();
    }
    public void QuitGame()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
        Debug.Log("Settings opened");
    }
}