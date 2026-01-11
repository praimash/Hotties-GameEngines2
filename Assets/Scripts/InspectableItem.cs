using UnityEngine;

public class InspectableItem : MonoBehaviour
{
    public void Interact()
    {
        // Yöneticiyi bul ve incelemeyi baþlat
        InspectSystem system = FindFirstObjectByType<InspectSystem>();
        if (system != null)
        {
            system.Inspect(this.gameObject);
        }
    }
}