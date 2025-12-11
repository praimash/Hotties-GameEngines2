using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName; // Buraya Inspector'dan isim yazacaðýz

    public void Interact()
    {
        Debug.Log(itemName + " alýndý!");

        // --- DEÐÝÞEN KISIM BURASI ---
        // Eðer bu eþyanýn adý "El Feneri" ise Player'daki feneri aktifleþtir
        if (itemName == "El Feneri")
        {
            // Player'ý bul, içindeki FlashlightController'ý bul ve feneri aç
            FlashlightController controller = FindFirstObjectByType<FlashlightController>();
            if (controller != null)
            {
                controller.EnableFlashlightInHand();
            }
        }
        // ---------------------------

        Destroy(gameObject); // Yerdekini yok et
    }
}