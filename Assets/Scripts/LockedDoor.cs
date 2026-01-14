using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    [Header("Ayarlar")]
    public string requiredKeyName; // Hangi anahtar lazým? (Ýsmi tam tutmalý)
    public float openAngle = 90f;  // Kapý kaç derece dönecek?
    public float smoothSpeed = 2f; // Açýlma hýzý

    private bool isOpen = false;
    private Quaternion targetRotation;

    void Start()
    {
        // Kapýnýn hedef açýsýný baþta kendi açýsý olarak ayarla
        targetRotation = transform.rotation;
    }

    void Update()
    {
        // Eðer kapý açýldýysa yavaþça hedef açýya dön
        if (isOpen)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothSpeed * Time.deltaTime);
        }
    }

    public void Interact()
    {
        if (isOpen) return; // Zaten açýksa uðraþma

        // Envantere sor: Anahtar var mý?
        if (InventoryManager.Instance.HasItem(requiredKeyName))
        {
            OpenDoor();
        }
        else
        {
            Debug.Log("Kapý Kilitli! '" + requiredKeyName + "' lazým.");
            // Ýstersen buraya kilit sesi veya ekrana yazý ekleyebilirsin
        }
    }

    void OpenDoor()
    {
        isOpen = true;
        Debug.Log("Kapý açýlýyor...");

        // Þu anki açýsýnýn üzerine 90 derece ekle
        targetRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + openAngle, transform.eulerAngles.z);
    }
}