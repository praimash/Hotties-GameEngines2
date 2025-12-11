using UnityEngine;

public class InteractionSystem : MonoBehaviour
{
    public float interactionDistance = 3f; // Ne kadar uzaktan alabilirsin
    public Camera cam;
    public LayerMask interactionLayer; // Sadece eþyalarý algýlasýn diye

    void Update()
    {
        // Eþyayý almak için 'E' tuþuna bas
        if (Input.GetKeyDown(KeyCode.E))
        {
            ShootRay();
        }
    }

    void ShootRay()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // Ekranýn tam ortasý
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance, interactionLayer))
        {
            // Eðer vurduðumuz þeyde "Item" scripti varsa tetikle
            Item item = hit.collider.GetComponent<Item>();
            if (item != null)
            {
                item.Interact();
            }
        }
    }
}