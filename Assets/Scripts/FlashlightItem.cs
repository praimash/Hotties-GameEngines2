using UnityEngine;

// Item scriptinden miras alýyor (Hem eþya hem özellik)
public class FlashlightItem : Item
{
    public override void Interact()
    {
        // 1. Önce babasýnýn (Item.cs) görevini yap (Envantere ikon ekle)
        base.Interact();

        // 2. Sonra Player'daki Fener kodunu bul ve kilidi aç
        FlashlightController playerLight = FindFirstObjectByType<FlashlightController>();
        if (playerLight != null)
        {
            playerLight.EnableFlashlightInHand();
        }
    }
}