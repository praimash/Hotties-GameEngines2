using UnityEngine;

public class InspectableItem : MonoBehaviour
{
    [Header("Ýnceleme Ayarlarý")]
    public GameObject modelPrefab; // Ýnceleme sýrasýnda görünecek 3D model (Kendisi de olabilir)
    public bool isCursed = false;  // Ýþaretlersen ekran kararýr/glitch girer

    private InspectSystem inspectSystem;

    void Start()
    {
        inspectSystem = FindObjectOfType<InspectSystem>();
    }

    public void Interact()
    {
        if (inspectSystem != null)
        {
            // Eðer modelPrefab boþsa, objenin kendisini kullanmaya çalýþýrýz ama
            // en temizi buraya objenin prefabýný sürüklemektir.
            GameObject targetModel = modelPrefab != null ? modelPrefab : gameObject;

            inspectSystem.StartInspect(targetModel, isCursed);
        }
    }
}