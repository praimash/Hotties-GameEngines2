using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName; // Eþyanýn adý (Anahtar, Fener vs.)
    public Sprite icon;     // Envanterde gözükecek resim

    public void Interact()
    {
        Debug.Log(itemName + " alýndý!");

        // Envanter Yöneticisine kendini gönder
        InventoryManager.Instance.AddItem(this);

        // Sahneden yok et (Ama script verisi Manager'da kopyalandý)
        // Not: Burada sadece görseli kapatýyoruz ki veri kaybolmasýn.
        gameObject.SetActive(false);
        transform.SetParent(InventoryManager.Instance.transform); // Çöp olmasýn diye Inventory'nin altýna taþý
    }
}