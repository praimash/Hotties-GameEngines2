using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class DeathManager : MonoBehaviour
{
    public GameObject deathPanel;
    

    [Header("Ses")]
    public AudioClip deathSound;
    public float deathSoundVolume = 1f;

    [Header("Ayarlar")]
    public float fadeInDuration = 2f;
    public float displayDuration = 3f;
    public bool autoRestart = false;
    [Header("Metin fade in")]
    public GameObject DiedText;
    public GameObject SubText;
    public GameObject SpaceText;

    


    float restartTimer = 0f;
    bool canRestart = false;
    


    private CanvasGroup canvasGroup;
    private AudioSource audioSource;
    private bool isDead = false;

    void Start()
    {

        if (deathPanel != null)
        {
            deathPanel.SetActive(false);
        }

        canvasGroup = deathPanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = deathPanel.AddComponent<CanvasGroup>();
        }

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.volume = deathSoundVolume;
    }

    void Update()
    {
       

        if (canRestart && Input.GetKeyDown(KeyCode.Space))
        {
            RestartLevel();
        }
        

        if (Input.GetKeyDown(KeyCode.K))
        {  

            Die();
        }
    }

    public void Die()
    {
        restartTimer = 0f;
       
        if (isDead) return;

        isDead = true;
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (deathSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        StartCoroutine(DeathUIFlow());
        StartCoroutine(ShowDeathScreen());
    }

    IEnumerator DeathUIFlow()
    {
        yield return new WaitForSecondsRealtime(1f);
        DiedText.SetActive(true);

        yield return new WaitForSecondsRealtime(2f);
        SubText.SetActive(true);

        yield return new WaitForSecondsRealtime(3.9f);
        SpaceText.SetActive(true);

        yield return new WaitForSecondsRealtime(3.8f);
        canRestart = true;
    }

    IEnumerator ShowDeathScreen()
    {
        deathPanel.SetActive(true);
       
        canvasGroup.alpha = 0f;
        float timer = 0f;

        while (timer < fadeInDuration)
        {
            timer += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / fadeInDuration);
            yield return null;
        }

       

       

        if (autoRestart)
        {
            yield return new WaitForSecondsRealtime(displayDuration);
            RestartLevel();
        }
    }

   

    void RestartLevel()
    {
        isDead = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

 

  
}
