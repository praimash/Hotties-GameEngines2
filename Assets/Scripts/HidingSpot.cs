using UnityEngine;

public class HidingSpot : MonoBehaviour
{
    [Header("Ayarlar")]
    public Transform exitPoint; // Dolaptan çýkýnca nerede doðalým?
    public GameObject playerMesh; // Oyuncunun vücudu (gizlemek için)
    public KeyCode exitKey = KeyCode.E; // Hangi tuþla çýkýlsýn?

    private bool isInside = false;
    private GameObject player;
    private MonoBehaviour playerMovement;
    private Collider playerCollider;

    void Update()
    {
        // Eðer içerdeysek, oyuncu nereye bakarsa baksýn E'ye basýnca çýksýn
        if (isInside && Input.GetKeyDown(exitKey))
        {
            Interact();
        }
    }

    public void Interact()
    {
        if (player == null) FindPlayer();

        if (!isInside)
        {
            HidePlayer();
        }
        else
        {
            ExitHiding();
        }
    }

    void HidePlayer()
    {
        isInside = true;

        // PlayerStatus scriptine eriþip gizlendiðimizi söylüyoruz
        // Eðer PlayerStatus scriptin yoksa bu satýrý silmen gerekebilir ama eklemiþtik.
        if (PlayerStatus.isHidden == false) PlayerStatus.isHidden = true;

        // Oyuncuyu dolabýn içine al
        if (player != null)
        {
            player.transform.position = transform.position;
            player.transform.rotation = transform.rotation;
        }

        // Fiziði ve Hareketi Kapat
        if (playerMovement != null) playerMovement.enabled = false;
        if (playerCollider != null) playerCollider.enabled = false;

        Debug.Log("Dolaba saklandýn! (Çýkmak için E'ye bas)");
    }

    void ExitHiding()
    {
        isInside = false;
        PlayerStatus.isHidden = false; // ARTIK GÝZLÝ DEÐÝLÝZ!

        // Çýkýþ noktasýna ýþýnla
        if (exitPoint != null && player != null)
        {
            player.transform.position = exitPoint.position;
        }
        else if (player != null)
        {
            // Eðer çýkýþ noktasý koymayý unuttuysan dolabýn biraz önüne atalým
            player.transform.position = transform.position + transform.forward * 1.5f;
        }

        // Fiziði ve Hareketi Aç
        if (playerMovement != null) playerMovement.enabled = true;
        if (playerCollider != null) playerCollider.enabled = true;

        Debug.Log("Dolaptan çýktýn.");
    }

    void FindPlayer()
    {
        GameObject pObj = GameObject.FindGameObjectWithTag("Player");
        if (pObj != null)
        {
            player = pObj;
            playerMovement = player.GetComponent<PlayerMovement>();
            // HATALI OLAN FirstPersonController SATIRINI SÝLDÝM.

            playerCollider = player.GetComponent<Collider>();
        }
    }
}