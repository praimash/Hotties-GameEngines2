using UnityEngine;

public class InteractionSystem : MonoBehaviour
{
    [Header("Ayarlar")]
    public float interactionDistance = 3f;
    public LayerMask interactionLayer;
    public Camera cam;

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

        if (Physics.Raycast(ray, out hit, interactionDistance, interactionLayer))
        {
            // 1. İncelemelik Eşya
            InspectableItem inspectItem = hit.collider.GetComponent<InspectableItem>();
            if (inspectItem != null) { inspectItem.Interact(); return; }

            // 2. Kilitli Kutu/Minigame (Skill Check olan)
            LockedItem lockedItem = hit.collider.GetComponent<LockedItem>();
            if (lockedItem != null) { lockedItem.Interact(); return; }

            // --- YENİ EKLENEN: Kilitli Kapı (Anahtar İsteyen) ---
            LockedDoor door = hit.collider.GetComponent<LockedDoor>();
            if (door != null) { door.Interact(); return; }
            // ----------------------------------------------------

            // 4. Saklanma Dolabı
            HidingSpot spot = hit.collider.GetComponent<HidingSpot>();
            if (spot != null) { spot.Interact(); return; }

            // 5. Şifreli Panel
            PasswordObject passObj = hit.collider.GetComponent<PasswordObject>();
            if (passObj != null) { passObj.Interact(); return; }

            // 6. Normal Eşya / Fener
            Item item = hit.collider.GetComponent<Item>();
            if (item != null) { item.Interact(); return; }
        }
    }
}
