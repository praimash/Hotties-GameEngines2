using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;
    public Sprite icon;

    // 'virtual' yazdýk ki Fener bunu deðiþtirip kendine özellik ekleyebilsin
    public virtual void Interact()
    {
        Debug.Log(itemName + " alýndý!");

        // Envanter sistemin varsa oraya gönderir
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.AddItem(this);

            // Görseli kapatýp Inventory'nin içine taþýyoruz
            gameObject.SetActive(false);
            transform.SetParent(InventoryManager.Instance.transform);
        }
        else
        {
            Debug.LogWarning("InventoryManager bulunamadý! Eþya sadece yok edildi.");
            Destroy(gameObject);
        }
    }
}