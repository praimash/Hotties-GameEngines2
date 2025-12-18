using UnityEngine;
using UnityEngine.UI;

public class SkillCheckSystem : MonoBehaviour
{
    [Header("Ayarlar")]
    public RectTransform pointerRect;
    public RectTransform zoneRect;
    public float rotationSpeed = 200f;
    public int targetSuccessCount = 3;

    [Header("KALİBRASYON (Burası Önemli)")]
    public float visualOffset = 0f; // Buraya konsoldaki sayıyı yazacağız

    private int currentSuccess = 0;
    private bool isGameActive = false;
    private System.Action onUnlockSuccess;

    // Referanslar (Otomatik Bulunur)
    private MonoBehaviour playerMovement;
    private MonoBehaviour interactionSystem;

    void Start()
    {
        gameObject.SetActive(false); // Başlangıçta gizle
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
        // Matematiksel Hesaplama
        float hitAngle = pointerRect.localEulerAngles.z;
        float targetAngle = zoneRect.localEulerAngles.z;

        // Asıl farkı bul (Offset dahil edilmemiş hali)
        float rawDifference = Mathf.DeltaAngle(hitAngle, targetAngle);

        // Offset dahil hesaplama (Oyunun kararı)
        float differenceWithOffset = Mathf.DeltaAngle(hitAngle, targetAngle + visualOffset);
        float tolerance = 25f; // Hata payı (Dilim genişliği)

        Debug.Log("--------------------------------------------------");
        Debug.Log($"🎯 VURUŞ RAPORU:");
        Debug.Log($"Senin Bastığın Yer (İbre): {hitAngle:F1}");
        Debug.Log($"Beyaz Alanın Yeri (Zone): {targetAngle:F1}");
        Debug.Log($"⚠️ ARADAKİ FARK: {rawDifference:F1}");
        Debug.Log($"👉 ÇÖZÜM: 'Visual Offset' kutusuna tam olarak {rawDifference:F1} (veya tam tersi) yazmalısın.");
        Debug.Log("--------------------------------------------------");

        if (Mathf.Abs(differenceWithOffset) < tolerance)
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
        Debug.Log("❌ BAŞARISIZ! (Ayar yapman lazım, konsola bak)");
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
        Debug.Log("Oyun Başladı!");
        gameObject.SetActive(true);
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
        gameObject.SetActive(false);
        if (playerMovement != null) playerMovement.enabled = true;
        if (interactionSystem != null) interactionSystem.enabled = true;
    }
}