using UnityEngine;

public class InteractionSystem : MonoBehaviour
{
    [Header("Ayarlar")]
    public float interactionDistance = 3f; // Ne kadar uzaktan alabilirsin
    public LayerMask interactionLayer; // Sadece 'Interactable' layerýný görsün

    [Header("Referanslar")]
    public Camera cam; // Kamerayý buraya sürüklemeyi unutma!

    void Update()
    {
        // E tuþuna basýnca ýþýn yolla
        if (Input.GetKeyDown(KeyCode.E))
        {
            ShootRay();
        }
    }

    void ShootRay()
    {
        // 1. DEBUG ÇÝZGÝSÝ: E'ye bastýðýnda sahnede (Scene ekranýnda) kýrmýzý bir çizgi çýkarýr.
        // Bu çizgi objeye deðiyor mu diye kontrol etmeni saðlar.
        Debug.DrawRay(cam.transform.position, cam.transform.forward * interactionDistance, Color.red, 2f);

        // Ekranýn tam ortasýndan hayali bir ýþýn (Ray) oluþtur
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        // Iþýn belirlediðimiz mesafede ve layerda bir þeye çarptý mý?
        if (Physics.Raycast(ray, out hit, interactionDistance, interactionLayer))
        {
            // --- A: Bu bir normal eþya mý? (El Feneri vs.) ---
            Item item = hit.collider.GetComponent<Item>();
            if (item != null)
            {
                item.Interact();
                return; // Bulduysan iþlemi bitir
            }

            // --- B: Bu bir kilitli dolap mý? ---
            LockedItem lockedItem = hit.collider.GetComponent<LockedItem>();
            if (lockedItem != null)
            {
                lockedItem.Interact();
            }
        }
    }
}