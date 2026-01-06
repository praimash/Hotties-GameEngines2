using UnityEngine;

public class PasswordObject : MonoBehaviour
{
    [Header("Þifre Ayarý")]
    public string myPassword = "1234"; // Bu objenin þifresi ne?
    public bool isLocked = true;

    public void Interact()
    {
        if (!isLocked) return;

        // Sistemden paneli açmasýný iste
        PasswordSystem.Instance.OpenPasswordScreen(myPassword, UnlockItem);
    }

    // Þifre doðru girilince burasý çalýþýr
    void UnlockItem()
    {
        isLocked = false;
        Debug.Log("Eriþim Ýzni Verildi!");

        // Rengini Yeþil Yap
        GetComponent<Renderer>().material.color = Color.green;

        // Ýstersen burada kapý açma animasyonu vs. tetikleyebilirsin
    }
}