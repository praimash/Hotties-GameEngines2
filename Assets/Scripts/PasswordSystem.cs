using UnityEngine;
using UnityEngine.UI; // Legacy UI için bu lazým
// using TMPro; // <--- BUNU SÝLDÝK, ARTIK GEREK YOK

public class PasswordSystem : MonoBehaviour
{
    public static PasswordSystem Instance;

    [Header("UI Referanslarý")]
    public GameObject passwordPanel;

    // ARTIK TMP DEÐÝL, NORMAL INPUT FIELD ÝSTÝYORUZ
    public InputField inputField;

    private string currentCorrectPassword;
    private System.Action onUnlockSuccess;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        passwordPanel.SetActive(false);
    }

    public void OpenPasswordScreen(string password, System.Action onSuccess)
    {
        currentCorrectPassword = password;
        onUnlockSuccess = onSuccess;

        passwordPanel.SetActive(true);
        inputField.text = "";
        inputField.ActivateInputField();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }

    public void CheckPassword()
    {
        if (inputField.text == currentCorrectPassword)
        {
            Debug.Log("Þifre Doðru!");
            ClosePanel();
            onUnlockSuccess?.Invoke();
        }
        else
        {
            Debug.Log("Yanlýþ Þifre!");
            inputField.text = "";
        }
    }

    public void ClosePanel()
    {
        passwordPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }
}