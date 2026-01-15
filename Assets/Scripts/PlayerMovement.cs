using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Hareket Ayarlarý")]
    public float speed = 12f;
    public float gravity = -19.62f; // Yerçekimi gücü
    public float jumpHeight = 3f;

    [Header("Zemin Kontrolü")]
    public Transform groundCheck; // Ayaklarýnýn altýndaki boþ obje
    public float groundDistance = 0.4f; // Yere ne kadar yakýnsa "yerde" sayýlsýn?
    public LayerMask groundMask; // Hangi layer zemin?

    [Header("Referanslar")]
    public CharacterController controller;
    public Transform playerCamera;
    public float mouseSensitivity = 100f;

    private float xRotation = 0f;
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        // Mouse'u kilitle ve gizle
        Cursor.lockState = CursorLockMode.Locked;

        // Eðer controller atanmamýþsa otomatik bul
        if (controller == null) controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // 1. ZEMÝN KONTROLÜ (Yerçekimini sýfýrlamak için)
        if (groundCheck != null)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        }

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Yerdeyken aþaðý çekmeye zorlama, hafif bastýr
        }

        // 2. MOUSE HAREKETÝ (Kamera Dönüþü)
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        if (playerCamera != null)
            playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);

        // 3. YÜRÜME
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        // 4. ZIPLAMA
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // 5. YERÇEKÝMÝ UYGULA
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}