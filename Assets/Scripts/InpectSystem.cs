using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InspectSystem : MonoBehaviour
{
    [Header("Ayarlar")]
    public Transform inspectPoint;   // Objenin duracaðý yer (Kamera önü)
    public float rotateSpeed = 5f;   // Dönme hýzý
    public GameObject glitchPanel;   // Korku efekti paneli

    [Header("Player Referanslarý")]
    public MonoBehaviour playerMovement; // Hareket scriptin
    public MonoBehaviour mouseLook;      // Kamera döndürme scriptin (Genelde MouseLook veya CameraController)

    private GameObject currentObject; // Þu an incelediðimiz kopya obje
    private bool isInspecting = false;

    void Start()
    {
        if (glitchPanel != null) glitchPanel.SetActive(false);
    }

    void Update()
    {
        if (!isInspecting) return;

        // 1. Objeyi Mouse ile Döndür
        float rotX = Input.GetAxis("Mouse X") * rotateSpeed;
        float rotY = Input.GetAxis("Mouse Y") * rotateSpeed;

        if (currentObject != null)
        {
            // Hem saða sola hem yukarý aþaðý dönsün
            currentObject.transform.Rotate(Vector3.up, -rotX, Space.World);
            currentObject.transform.Rotate(Vector3.right, rotY, Space.World);
        }

        // 2. Çýkýþ Yap (E veya ESC)
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Escape))
        {
            CloseInspect();
        }
    }

    public void StartInspect(GameObject prefabModel, bool isCursed)
    {
        isInspecting = true;

        // Oyuncuyu Dondur (Kafa dönmesin, sadece obje dönsün)
        if (playerMovement != null) playerMovement.enabled = false;
        if (mouseLook != null) mouseLook.enabled = false;

        // Objeyi Yarat (InspectPoint konumunda)
        if (currentObject != null) Destroy(currentObject);
        currentObject = Instantiate(prefabModel, inspectPoint.position, inspectPoint.rotation, inspectPoint);

        // Objenin üzerindeki colliderlarý kapat ki iç içe girmesin
        Collider[] cols = currentObject.GetComponentsInChildren<Collider>();
        foreach (Collider c in cols) c.enabled = false;

        // LANETLÝ MÝ?
        if (isCursed)
        {
            StartCoroutine(GlitchEffect());
        }
    }

    public void CloseInspect()
    {
        isInspecting = false;

        // Objeyi yok et
        if (currentObject != null) Destroy(currentObject);

        // Paneli kapat
        if (glitchPanel != null) glitchPanel.SetActive(false);
        StopAllCoroutines();

        // Oyuncuyu Serbest Býrak
        if (playerMovement != null) playerMovement.enabled = true;
        if (mouseLook != null) mouseLook.enabled = true;
    }

    // Korku Efekti: Paneli rastgele açýp kapatýr
    IEnumerator GlitchEffect()
    {
        while (isInspecting)
        {
            glitchPanel.SetActive(true);
            yield return new WaitForSeconds(Random.Range(0.05f, 0.2f)); // Çok kýsa bekle
            glitchPanel.SetActive(false);
            yield return new WaitForSeconds(Random.Range(0.1f, 0.8f)); // Biraz normal dursun
        }
    }
}