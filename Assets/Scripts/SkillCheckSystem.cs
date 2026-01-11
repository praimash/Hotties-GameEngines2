using UnityEngine;
using UnityEngine.UI;

public class SkillCheckSystem : MonoBehaviour
{
    [Header("UI Referansları")]
    public GameObject minigamePanel; // Panel
    public RectTransform pointerRect;
    public RectTransform zoneRect;

    [Header("Ayarlar")]
    public float rotationSpeed = 200f;
    public int targetSuccessCount = 3;

    [Tooltip("Merkezden kaç derece sağa/sola sapmaya izin var? (Örn: 15)")]
    public float hitTolerance = 15f; // <--- YENİ AYAR BURADA (15 SAĞ, 15 SOL)

    [Header("KALİBRASYON")]
    public float visualOffset = 0f;

    private int currentSuccess = 0;
    private bool isGameActive = false;
    private System.Action onUnlockSuccess;

    private MonoBehaviour playerMovement;
    private MonoBehaviour interactionSystem;

    void Start()
    {
        if (minigamePanel != null)
        {
            minigamePanel.SetActive(false);
        }
    }

    void Update()
    {
        if (!isGameActive) return;

        // İbreyi Döndür
        if (pointerRect != null)
            pointerRect.Rotate(0, 0, -rotationSpeed * Time.deltaTime);

        // Tuş Kontrolü
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space))
        {
            CheckZoneHit();
        }
    }

    void CheckZoneHit()
    {
        float hitAngle = pointerRect.localEulerAngles.z;
        float targetAngle = zoneRect.localEulerAngles.z;

        float rawDifference = Mathf.DeltaAngle(hitAngle, targetAngle);
        float differenceWithOffset = Mathf.DeltaAngle(hitAngle, targetAngle + visualOffset);

        Debug.Log("--------------------------------------------------");
        Debug.Log($"🎯 VURUŞ RAPORU:");
        Debug.Log($"Fark (Offset Hariç): {rawDifference:F1}");
        Debug.Log($"👉 ÇÖZÜM: 'Visual Offset' kutusuna {rawDifference:F1} yaz.");
        Debug.Log("--------------------------------------------------");

        // Mathf.Abs kullandığımız için hem sağa (+15) hem sola (-15) kabul eder.
        if (Mathf.Abs(differenceWithOffset) <= hitTolerance)
        {
            Success();
        }
        else
        {
            Fail();
        }
    }

    void Success()
    {
        currentSuccess++;
        Debug.Log($"✅ BAŞARILI! ({currentSuccess}/{targetSuccessCount})");
        rotationSpeed += 50f;

        if (currentSuccess >= targetSuccessCount)
        {
            WinGame();
        }
        else
        {
            RandomizeZone();
        }
    }

    void Fail()
    {
        Debug.Log("❌ BAŞARISIZ! (Kalibrasyon yap veya zamanlamayı tutturamadın)");
        CloseMinigame();
    }

    void WinGame()
    {
        Debug.Log("🎉 KİLİT AÇILDI!");
        onUnlockSuccess?.Invoke();
        CloseMinigame();
    }

    void RandomizeZone()
    {
        if (zoneRect != null)
        {
            float randomAngle = Random.Range(0f, 360f);
            zoneRect.localRotation = Quaternion.Euler(0, 0, randomAngle);
        }
    }

    public void StartMinigame(System.Action onSuccess)
    {
        if (minigamePanel != null) minigamePanel.SetActive(true);

        onUnlockSuccess = onSuccess;

        playerMovement = FindFirstObjectByType<PlayerMovement>();
        interactionSystem = FindFirstObjectByType<InteractionSystem>();

        if (playerMovement != null) playerMovement.enabled = false;
        if (interactionSystem != null) interactionSystem.enabled = false;

        currentSuccess = 0;
        rotationSpeed = 200f;
        isGameActive = true;
        RandomizeZone();
    }

    void CloseMinigame()
    {
        isGameActive = false;
        if (minigamePanel != null) minigamePanel.SetActive(false);

        if (playerMovement != null) playerMovement.enabled = true;
        if (interactionSystem != null) interactionSystem.enabled = true;
    }
}