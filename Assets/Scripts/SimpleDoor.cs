using UnityEngine;

public class SimpleDoor : MonoBehaviour
{
    [Header("Ayarlar")]
    public float openAngle = 90f;   // Kaç derece açýlsýn? (Genelde 90)
    public float smoothSpeed = 4f;  // Ne kadar hýzlý açýlsýn?
    public bool openAwayFromPlayer = true; // Oyuncunun tersine doðru açýlsýn (Opsiyonel)

    [Header("Sesler (Ýsteðe Baðlý)")]
    public AudioSource audioSource;
    public AudioClip openSound;
    public AudioClip closeSound;

    private bool isOpen = false;
    private Quaternion defaultRot;
    private Quaternion openRot;

    void Start()
    {
        // Kapýnýn baþlangýç (kapalý) halini kaydet
        defaultRot = transform.localRotation;

        // Açýk halini hesapla (Mevcut açý + 90 derece)
        openRot = Quaternion.Euler(0, openAngle, 0) * defaultRot;
    }

    void Update()
    {
        // Kapýyý hedef açýya doðru yumuþakça döndür
        Quaternion targetRot = isOpen ? openRot : defaultRot;
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRot, Time.deltaTime * smoothSpeed);
    }

    // InteractionSystem bu fonksiyonu çaðýracak
    public void Interact()
    {
        isOpen = !isOpen; // Durumu tam tersine çevir (Açýksa kapat, kapalýysa aç)

        // Ses çalma kýsmý
        if (audioSource != null)
        {
            if (isOpen && openSound != null) audioSource.PlayOneShot(openSound);
            else if (!isOpen && closeSound != null) audioSource.PlayOneShot(closeSound);
        }
    }
}