using UnityEngine;

public class InspectSystem : MonoBehaviour
{
    [Header("Ayarlar")]
    public float rotateSpeed = 200f;

    [Header("Görünüm Ayarý")]
    [Range(0.01f, 1f)]
    public float viewScale = 0.2f; // <-- Varsayýlaný iyice küçülttüm

    [Header("Referanslar (Sürükle!)")]
    public PlayerMovement playerMove;
    public CharacterController playerController;
    public InteractionSystem interactionSys;

    public Camera mainCam;

    private GameObject currentItem;
    private Vector3 originalPos;
    private Quaternion originalRot;
    private Vector3 originalScale;
    private bool isInspecting = false;
    private Collider[] itemColliders;
    private Rigidbody itemRb;
    private bool wasKinematic;

    void Start()
    {
        if (mainCam == null) mainCam = Camera.main;

        // Otomatik bulmayalým, sen elle sürükle (En garantisi)
        if (playerMove == null) playerMove = FindFirstObjectByType<PlayerMovement>();
        if (interactionSys == null) interactionSys = FindFirstObjectByType<InteractionSystem>();
        if (playerController == null) playerController = FindFirstObjectByType<CharacterController>();
    }

    void Update()
    {
        if (isInspecting && currentItem != null)
        {
            // Eþyayý sürekli kameranýn önünde tut (Hareket etsek bile)
            // BURASI KRÝTÝK: Kameranýn pozisyonu + (Kameranýn baktýðý yön * 1.5 metre)
            Vector3 targetPos = mainCam.transform.position + (mainCam.transform.forward * 1.5f);
            currentItem.transform.position = targetPos;

            // Döndürme
            float x = Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
            float y = Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime;

            // Kameraya göre döndür
            currentItem.transform.Rotate(mainCam.transform.up, -x, Space.World);
            currentItem.transform.Rotate(mainCam.transform.right, y, Space.World);

            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E))
            {
                DropItem();
            }
        }
    }

    public void Inspect(GameObject itemObj)
    {
        if (isInspecting) return;
        isInspecting = true;
        currentItem = itemObj;

        // Eski bilgileri kaydet
        originalPos = itemObj.transform.position;
        originalRot = itemObj.transform.rotation;
        originalScale = itemObj.transform.localScale;

        // 1. Collider ve Fizik Kapat
        itemColliders = itemObj.GetComponentsInChildren<Collider>();
        foreach (Collider col in itemColliders) col.enabled = false;

        itemRb = itemObj.GetComponent<Rigidbody>();
        if (itemRb != null) { wasKinematic = itemRb.isKinematic; itemRb.isKinematic = true; }

        // 2. POZÝSYONLAMA (Matematiksel)
        // Kameranýn 1.5 metre önüne ýþýnla
        currentItem.transform.position = mainCam.transform.position + (mainCam.transform.forward * 1.5f);

        // Yüzünü kameraya dön
        currentItem.transform.LookAt(mainCam.transform);

        // Boyutunu ayarla
        currentItem.transform.localScale = originalScale * viewScale;

        // 3. OYUNCUYU KAPAT
        if (playerMove != null) playerMove.enabled = false;
        if (interactionSys != null) interactionSys.enabled = false;
        if (playerController != null) playerController.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
    }

    public void DropItem()
    {
        isInspecting = false;

        if (currentItem != null)
        {
            // Eski yerine koy
            currentItem.transform.position = originalPos;
            currentItem.transform.rotation = originalRot;
            currentItem.transform.localScale = originalScale;

            if (itemRb != null) { itemRb.isKinematic = wasKinematic; itemRb = null; }

            if (itemColliders != null)
            {
                foreach (Collider col in itemColliders) { if (col != null) col.enabled = true; }
                itemColliders = null;
            }
            currentItem = null;
        }

        // Oyuncuyu aç
        if (playerController != null) playerController.enabled = true;
        if (playerMove != null) playerMove.enabled = true;
        if (interactionSys != null) interactionSys.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
    }
}