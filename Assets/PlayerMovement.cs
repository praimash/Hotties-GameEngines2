using UnityEngine;
using UnityEngine.InputSystem; // 1. BU KÜTÜPHANEYÝ EKLEMEK ZORUNDASIN

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    // Oluþturduðumuz o otomatik class'tan bir tane tanýmlýyoruz.
    // (Senin dosyanýn adý neyse 'GameInputs' yerine onu yaz)
    private PlayerMovement inputActions;

    // OYUN BAÞLAMADAN ÖNCE (Awake)
    private void Awake()
    {
        // Tuþ ayarlarýný yükle (New'le)
        inputActions = new PlayerMovement();
    }

    // OBJE AKTÝF OLUNCA (OnEnable) - ÇOK ÖNEMLÝ
    private void OnEnable()
    {
        // Girdi sistemini "Dinlemeye Baþla"
        inputActions.Enable();
    }

    // OBJE KAPANINCA (OnDisable) - ÇOK ÖNEMLÝ
    private void OnDisable()
    {
        // Girdi sistemini "Dinlemeyi Býrak" (Hata almamak için þart)
        inputActions.Disable();
    }

    void Update()
    {
        // --- ESKÝ YÖNTEM (SÝLÝNDÝ) ---
        // float x = Input.GetAxis("Horizontal");
        // ...

        // --- YENÝ YÖNTEM ---
        // 1. Veriyi Oku:
        // "Player" haritasýndaki "Move" aksiyonunu Vector2 olarak oku.
        // Bize (x, y) deðerleri verecek (Örn: W'ye basýnca y=1 olur).
        Vector2 inputVector = inputActions.Player.Move.ReadValue<Vector2>();

        // 2. 3D Dünyaya Çevir:
        // 2D'deki Y (Yukarý), 3D dünyada Z (Ýleri) demektir.
        Vector3 moveDirection = new Vector3(inputVector.x, 0, inputVector.y);

        // 3. Hareketi Uygula:
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }
}