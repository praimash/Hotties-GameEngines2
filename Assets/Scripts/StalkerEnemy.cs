using UnityEngine;
using UnityEngine.SceneManagement; // Sahneyi resetlemek için

public class StalkerEnemy : MonoBehaviour
{
    [Header("Görüş Ayarları")]
    public float viewDistance = 15f; // Ne kadar uzağı görebilir?
    public float viewAngle = 60f;    // Görüş açısı (Gözleri ensesinde olmasın)
    public LayerMask obstacleMask;   // Duvarların layerı (Default olsun genelde)

    [Header("Ölüm Ayarları")]
    public float killTime = 5f; // Bizi görünce kaç saniye sonra öldürsün?

    private Transform player;
    private float killTimer;
    private bool isHunting = false; // Şu an bizi kovalıyor mu?

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        killTimer = killTime;
    }

    void Update()
    {
        // 1. Oyuncu Saklanıyor mu?
        if (PlayerStatus.isHidden)
        {
            // Saklanıyorsa bizi göremez, sayacı sıfırla
            isHunting = false;
            killTimer = killTime;
            return;
        }

        // 2. Oyuncuyu Görüyor muyuz?
        if (CanSeePlayer())
        {
            isHunting = true;
            killTimer -= Time.deltaTime; // Süreyi azalt

            Debug.LogWarning("DÜŞMAN SENİ GÖRDÜ! Kalan Süre: " + (int)killTimer);

            if (killTimer <= 0)
            {
                KillPlayer();
            }
        }
        else
        {
            // Gözden kaybolursak hemen pes etmesin, yavaşça dolsun (Opsiyonel)
            isHunting = false;
            killTimer = killTime; // Şimdilik direkt resetliyoruz
        }
    }

    bool CanSeePlayer()
    {
        if (player == null) return false;

        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        float dstToPlayer = Vector3.Distance(transform.position, player.position);

        // 1. Mesafe kontrolü
        if (dstToPlayer > viewDistance) return false;

        // 2. Açı kontrolü (Önünde mi?)
        if (Vector3.Angle(transform.forward, dirToPlayer) > viewAngle / 2) return false;

        // 3. Duvar kontrolü (Arada duvar var mı?)
        // Raycast atıyoruz: Bize çarpıyorsa sorun yok, duvara çarpıyorsa göremez.
        if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
        {
            return true; // Engel yok, görüyorum!
        }

        return false;
    }

    void KillPlayer()
    {
        Debug.Log("💀 ÖLDÜN!");
        // Sahneyi yeniden başlat (Ölüm ekranı vs. sonra eklersin)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Editörde görüş alanını çizelim (Görmek için)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        Vector3 viewAngleA = DirFromAngle(-viewAngle / 2, false);
        Vector3 viewAngleB = DirFromAngle(viewAngle / 2, false);

        Gizmos.DrawLine(transform.position, transform.position + viewAngleA * viewDistance);
        Gizmos.DrawLine(transform.position, transform.position + viewAngleB * viewDistance);
    }

    Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal) angleInDegrees += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}