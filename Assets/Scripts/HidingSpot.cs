using UnityEngine;
using System.Collections;

public class HidingSpot : MonoBehaviour
{
    [Header("Nokta Ayarları")]
    public Transform exitPoint; // Çıkınca nerede doğsun?
    public Transform hidePoint; // İÇERİDEYKEN nerede durup nereye baksın? (BOŞ BIRAKIRSAN YATAĞI BAZ ALIR)

    [Header("Diğer Ayarlar")]
    public KeyCode exitKey = KeyCode.E;
    public float exitOffset = 1.5f;

    private bool isInside = false;
    private bool canExit = true;
    private float lastInteractionTime;

    private GameObject player;
    private PlayerMovement playerMovement;
    private Collider playerCollider;

    void Update()
    {
        if (isInside && canExit && Input.GetKeyDown(exitKey))
        {
            ExitHiding();
        }
    }

    public void Interact()
    {
        if (Time.time < lastInteractionTime + 0.5f) return;

        if (player == null) FindPlayer();

        if (!isInside)
        {
            HidePlayer();
        }
    }

    void HidePlayer()
    {
        isInside = true;
        canExit = false;
        PlayerStatus.isHidden = true;
        lastInteractionTime = Time.time;

        if (player != null)
        {
            // --- YENİ EKLENEN KISIM: HIDE POINT ---
            if (hidePoint != null)
            {
                // Eğer özel nokta belirlediysen oraya git ve ORANIN BAKTIĞI YERE bak
                player.transform.position = hidePoint.position;
                player.transform.rotation = hidePoint.rotation;
            }
            else
            {
                // Belirlemediysen eskisi gibi objenin merkezine git
                player.transform.position = transform.position;

                // Sadece Y yönünü al (Dik dur)
                Vector3 targetRot = transform.eulerAngles;
                player.transform.rotation = Quaternion.Euler(0, targetRot.y, 0);
            }
            // --------------------------------------

            if (playerMovement != null) playerMovement.enabled = false;
            if (playerCollider != null) playerCollider.enabled = false;
        }

        StartCoroutine(EnableExitCooldown());
        Debug.Log("Saklandın.");
    }

    void ExitHiding()
    {
        isInside = false;
        PlayerStatus.isHidden = false;
        lastInteractionTime = Time.time;

        if (player != null)
        {
            if (exitPoint != null)
            {
                player.transform.position = exitPoint.position;
            }
            else
            {
                Vector3 autoExitPos = transform.position + (transform.forward * exitOffset);
                player.transform.position = autoExitPos;
            }

            // Çıkarken dik dur ve karşıya bak
            Vector3 currentRot = player.transform.eulerAngles;
            player.transform.rotation = Quaternion.Euler(0, currentRot.y, 0);

            Camera cam = player.GetComponentInChildren<Camera>();
            if (cam != null)
            {
                // Kafayı düzelt (isteğe bağlı)
                // cam.transform.localRotation = Quaternion.identity; 
            }
        }

        if (playerMovement != null) playerMovement.enabled = true;
        if (playerCollider != null) playerCollider.enabled = true;

        Debug.Log("Çıktın.");
    }

    IEnumerator EnableExitCooldown()
    {
        yield return new WaitForSeconds(0.5f);
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