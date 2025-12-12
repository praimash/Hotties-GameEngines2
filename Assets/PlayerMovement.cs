using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // 1. Dosya ismin 'NewActions' olduðu için deðiþken türü bu olmalý:
    private NewActions inputActions;

    private void Awake()
    {
        // 2. Sýnýfý oluþtururken de dosya ismini kullanýyoruz:
        inputActions = new NewActions();
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    void Update()
    {
        // 3. ÝÞTE BURASI ÇOK ÖNEMLÝ!
        // Eriþim Yolu: [Deðiþken].[HaritaÝsmi].[AksiyonÝsmi]
        // Senin haritanýn adý da "Move", aksiyonun adý da "Move".
        // Bu yüzden "Move.Move" yazacaðýz.

        Vector2 inputVector = inputActions.Move.Move.ReadValue<Vector2>();

        // (Geri kalan kodlar ayný...)
        Vector3 moveDirection = new Vector3(inputVector.x, 0, inputVector.y);
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }
}