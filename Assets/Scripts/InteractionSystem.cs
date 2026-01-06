using UnityEngine;

public class InteractionSystem : MonoBehaviour
{
    [Header("Ayarlar")]
    public float interactionDistance = 3f; // Mesafe
    public LayerMask interactionLayer; // Sadece bu layerdakileri görsün

    [Header("Referanslar")]
    public Camera cam; // Kamerayı sürükle

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ShootRay();
        }
    }

    void ShootRay()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        // --- HATA AYIKLAMA (RÖNTGEN) ---
        // Işın neye çarpıyor görelim. 
        if (Physics.Raycast(ray, out RaycastHit debugHit, interactionDistance))
        {
            // Eğer kendi Player layerına çarpıyorsa konsola uyarı basar
            if (debugHit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                Debug.LogWarning("⚠️ DİKKAT: Kendi vücuduna (Player) çarpıyorsun! Interaction System ayarlarından Player layerını kapat.");
            }
            else
            {
                Debug.Log("🔍 BAKTIĞIN ŞEY: " + debugHit.collider.name);
            }
        }
        // --------------------------------

        Debug.DrawRay(cam.transform.position, cam.transform.forward * interactionDistance, Color.red, 2f);

        // --- GERÇEK ETKİLEŞİM ---
        // LayerMask kullanarak ışın atıyoruz. Böylece "Default" veya "Player" layerlarını görmezden gelebiliriz.
        if (Physics.Raycast(ray, out hit, interactionDistance, interactionLayer))
        {
            // 1. İncelemelik Eşya mı? (Inspectable)
            InspectableItem inspectItem = hit.collider.GetComponent<InspectableItem>();
            if (inspectItem != null)
            {
                inspectItem.Interact();
                return;
            }

            // 2. Normal Eşya mı? (Item)
            Item item = hit.collider.GetComponent<Item>();
            if (item != null)
            {
                item.Interact();
                return;
            }

            // 3. Kilitli Dolap mı? (LockedItem)
            LockedItem lockedItem = hit.collider.GetComponent<LockedItem>();
            if (lockedItem != null)
            {
                lockedItem.Interact();
                return;
            }

            // 4. Kilitli Kapı mı? (LockedDoor)
            LockedDoor door = hit.collider.GetComponent<LockedDoor>();
            if (door != null)
            {
                door.Interact();
                return;
            }

            // 5. Şifreli Obje mi? (PasswordObject) --- [YENİ EKLENEN KISIM] ---
            PasswordObject passObj = hit.collider.GetComponent<PasswordObject>();
            if (passObj != null)
            {
                Debug.Log("Sistem: Şifreli objeye tıkladın!"); // Kontrol için log
                passObj.Interact();
                return;
            }
            // 6. Saklanma Noktası mı? (YENİ)
            HidingSpot spot = hit.collider.GetComponent<HidingSpot>();
            if (spot != null)
            {
                spot.Interact();
                return;
            }
        }
    }
}