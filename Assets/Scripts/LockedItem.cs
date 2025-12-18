using UnityEngine;

public class LockedItem : MonoBehaviour
{
    public bool isLocked = true;

    [Header("Ödül Ayarý")]
    public GameObject rewardObject; // Dolap açýlýnca çýkacak eþya (Anahtar)

    // Kod sistemi otomatik bulacak
    private SkillCheckSystem skillCheckSystem;

    void Start()
    {
        // Ödül objesi baþta varsa, garanti olsun diye gizleyelim
        if (rewardObject != null)
        {
            rewardObject.SetActive(false);
        }

        // OYUN BAÞLAYINCA SAHNEDEKÝ SÝSTEMÝ ZORLA BUL
        skillCheckSystem = FindObjectOfType<SkillCheckSystem>(true);

        if (skillCheckSystem == null)
        {
            Debug.LogError("HATA: Sahnede 'SkillCheckSystem' scripti olan obje yok!");
        }
    }

    public void Interact()
    {
        if (isLocked)
        {
            Debug.Log("Kilit zorlanýyor...");
            if (skillCheckSystem != null)
            {
                skillCheckSystem.StartMinigame(OpenCabinet);
            }
        }
        else
        {
            // Kilit açýksa sadece kapak animasyonu veya sesi çalar
            Debug.Log("Dolap zaten açýk.");
        }
    }

    void OpenCabinet()
    {
        isLocked = false;
        Debug.Log("Dolap açýldý! Anahtar ortaya çýktý.");

        // Dolabýn rengini deðiþtir (Görsel geri bildirim)
        GetComponent<Renderer>().material.color = Color.green;

        // --- ANAHTARI ORTAYA ÇIKAR ---
        if (rewardObject != null)
        {
            rewardObject.SetActive(true); // Gizli anahtarý görünür yap
        }
        else
        {
            Debug.LogWarning("UYARI: Dolabýn içine 'Reward Object' (Anahtar) koymayý unuttun!");
        }
    }
}