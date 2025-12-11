using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    public GameObject flashlightHandObject; // Elindeki fener objesi (Light componentli olan)
    public bool hasFlashlight = false; // Envanterde var mý?
    public bool isLightOn = false; // Iþýk açýk mý?

    void Update()
    {
        // Fenerimiz varsa ve F'ye basarsak aç/kapa
        if (hasFlashlight && Input.GetKeyDown(KeyCode.F))
        {
            ToggleLight();
        }
    }

    // Yerdeki feneri alýnca bu çalýþacak
    public void EnableFlashlightInHand()
    {
        hasFlashlight = true;
        // Ýstersen alýnca otomatik açýlýr, istersen kapalý gelir.
        // Biz sadece 'var' olduðunu kaydedelim.
        Debug.Log("El feneri envantere eklendi! F ile açabilirsin.");
    }

    void ToggleLight()
    {
        isLightOn = !isLightOn;
        flashlightHandObject.SetActive(isLightOn); // Objeyi (ve ýþýðýný) aç/kapa

        // Buraya "klik" sesi de ekleyebilirsin:
        // GetComponent<AudioSource>().Play();
    }
}