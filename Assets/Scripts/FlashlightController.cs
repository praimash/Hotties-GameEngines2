using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    [Header("Ayarlar")]
    public GameObject flashlightHandObject; // Kameranýn içindeki Iþýk objesi
    public bool hasFlashlight = false;      // Envanterde var mý?
    public bool isLightOn = false;          // Þu an açýk mý?

    void Start()
    {
        // Oyun baþýnda eldeki ýþýðý kapat
        if (flashlightHandObject != null) flashlightHandObject.SetActive(false);
    }

    void Update()
    {
        // Fener varsa ve F'ye basýlýrsa
        if (hasFlashlight && Input.GetKeyDown(KeyCode.F))
        {
            ToggleLight();
        }
    }

    // Yerdeki fener alýnýnca bu çalýþýr
    public void EnableFlashlightInHand()
    {
        hasFlashlight = true;
        Debug.Log("El feneri özelliði açýldý!");
    }

    void ToggleLight()
    {
        isLightOn = !isLightOn;
        if (flashlightHandObject != null)
            flashlightHandObject.SetActive(isLightOn);
    }
}