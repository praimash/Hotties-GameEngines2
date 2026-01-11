using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10f;
    public float mouseSensitivity = 2f; // Raw kullanýnca bunu biraz düþürmen gerekebilir (örn: 2-3 gibi)

    [Header("Sürükle Býrak")]
    public Transform playerCamera; // Player'ýn içindeki Main Camera'yý buraya sürükle
    public CharacterController controller;

    private float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        // FPS'i kodla da zorla sabitleyelim (VSync yetmezse diye)
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        // 1. MOUSE (Raw kullanýyoruz AMA Time.deltaTime ekledik ki uçmasýn)
        // Sensitivity ayarýný inspector'dan tekrar 100-200 gibi eski haline getirebilirsin.
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        // 2. YÜRÜME
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        // Burasý zaten doðruydu
        controller.Move(move.normalized * speed * Time.deltaTime);
    }
}