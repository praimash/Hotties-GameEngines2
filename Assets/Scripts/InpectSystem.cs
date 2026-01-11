using UnityEngine;

public class InspectSystem : MonoBehaviour
{
    [Header("Ayarlar")]
    public Transform inspectPoint; // Kameranýn içindeki o X=0, Y=0, Z=0.5 olan nokta
    public float rotateSpeed = 200f; // Hýzý biraz düþürdüm, daha kontrollü olsun

    private GameObject currentItem;
    private Vector3 originalPos;
    private Quaternion originalRot;
    private bool isInspecting = false;

    // Fiziði yönetmek için
    private Rigidbody itemRb;
    private bool wasKinematic; // Eþyanýn fiziði baþta açýk mýydý kapalý mýydý?

    private PlayerMovement playerMove;
    private InteractionSystem interactionSys; // Raycast atmayý durdurmak için

    void Start()
    {
        playerMove = FindFirstObjectByType<PlayerMovement>();
        interactionSys = FindFirstObjectByType<InteractionSystem>();
    }

    void Update()
    {
        if (isInspecting && currentItem != null)
        {
            // Mouse ile döndürme (Kameranýn eksenlerine göre)
            float x = Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
            float y = Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime;

            // DÝKKAT: Camera.main.transform kullanýyoruz ki bakýþ açýmýza göre dönsün
            currentItem.transform.Rotate(Camera.main.transform.up, -x, Space.World);
            currentItem.transform.Rotate(Camera.main.transform.right, y, Space.World);

            // Çýkýþ
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

        // 1. Eski konumunu kaydet
        originalPos = itemObj.transform.position;
        originalRot = itemObj.transform.rotation;

        // 2. Fiziði (Yerçekimini) Kapat - ÇOK ÖNEMLÝ
        itemRb = itemObj.GetComponent<Rigidbody>();
        if (itemRb != null)
        {
            wasKinematic = itemRb.isKinematic;
            itemRb.isKinematic = true; // Obje havada donsun, düþmesin
        }

        // 3. Eþyayý kameranýn önüne taþý
        if (inspectPoint != null)
        {
            currentItem.transform.position = inspectPoint.position;

            // Ýstersen eþyayý ilk baþta kameraya düz baktýr (Opsiyonel)
            // currentItem.transform.rotation = Quaternion.LookRotation(-Camera.main.transform.forward);
        }

        // 4. Oyuncuyu ve Etkileþimi dondur
        if (playerMove != null) playerMove.enabled = false;
        if (interactionSys != null) interactionSys.enabled = false; // Eþya elindeyken baþka þeye týklama

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false; // Mouse imleci görünmesin ama dönsün
    }

    public void DropItem()
    {
        isInspecting = false;

        if (currentItem != null)
        {
            // 1. Eþyayý yerine koy
            currentItem.transform.position = originalPos;
            currentItem.transform.rotation = originalRot;

            // 2. Fiziði eski haline getir
            if (itemRb != null)
            {
                itemRb.isKinematic = wasKinematic;
                itemRb = null;
            }
            currentItem = null;
        }

        // 3. Oyuncuyu serbest býrak
        if (playerMove != null) playerMove.enabled = true;
        if (interactionSys != null) interactionSys.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
    }
}