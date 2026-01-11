using UnityEngine;
using System.Collections;

public class HidingSpot : MonoBehaviour
{
    [Header("Ayarlar")]
    public Transform exitPoint;
    public KeyCode exitKey = KeyCode.E;

    private bool isInside = false;
    private bool canExit = true; // Hemen çýkmayý önleyen kilit

    private GameObject player;
    private PlayerMovement playerMovement;
    private Collider playerCollider;

    void Update()
    {
        // Sadece içerideysek, çýkýþ izni varsa ve E'ye basýldýysa
        if (isInside && canExit && Input.GetKeyDown(exitKey))
        {
            ExitHiding();
        }
    }

    public void Interact()
    {
        if (player == null) FindPlayer();

        if (!isInside)
        {
            HidePlayer();
        }
    }

    void HidePlayer()
    {
        isInside = true;
        canExit = false; // Kilidi kapat, hemen çýkamasýn
        PlayerStatus.isHidden = true;

        if (player != null)
        {
            // Oyuncuyu dolabýn içine al
            player.transform.position = transform.position;
            player.transform.rotation = transform.rotation;

            // Hareketi ve Fiziði Kapat (PlayerMovement scriptini bulup kapatýyoruz)
            if (playerMovement != null) playerMovement.enabled = false;
            if (playerCollider != null) playerCollider.enabled = false;
        }

        // 1 Saniye sonra çýkýþa izin ver (Girdi-Çýktý bugýný önler)
        StartCoroutine(EnableExitCooldown());

        Debug.Log("Dolaba girdin.");
    }

    void ExitHiding()
    {
        isInside = false;
        PlayerStatus.isHidden = false;

        if (exitPoint != null && player != null)
            player.transform.position = exitPoint.position;

        // Hareketi Geri Aç
        if (playerMovement != null) playerMovement.enabled = true;
        if (playerCollider != null) playerCollider.enabled = true;

        Debug.Log("Dolaptan çýktýn.");
    }

    IEnumerator EnableExitCooldown()
    {
        yield return new WaitForSeconds(0.5f); // Yarým saniye bekle
        canExit = true;
    }

    void FindPlayer()
    {
        GameObject pObj = GameObject.FindGameObjectWithTag("Player");
        if (pObj != null)
        {
            player = pObj;
            playerMovement = player.GetComponent<PlayerMovement>();
            playerCollider = player.GetComponent<Collider>();
        }
    }
}