using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Image kontrolü için şart
using TMPro;          // TextMeshPro için şart
using System.Collections.Generic;

public class SmartEnemyAI : MonoBehaviour
{
    [Header("UI & Efektler")]
    public TextMeshProUGUI countdownText; // Geri sayım yazısı
    public Image glitchImage;             // Glitch/Karıncalı ekran resmi

    [Header("Hareket Ayarları")]
    public float walkSpeed = 2f;
    public float runSpeed = 4f;
    public List<Transform> waypoints;

    [Header("Görüş Ayarları")]
    public float viewDistance = 15f;
    [Range(0, 360)]
    public float viewAngle = 110f;
    public LayerMask obstacleMask;
    public Transform eyes;

    [Header("Ölüm Ayarları")]
    public float killTime = 5f;
    public float killDistance = 1.5f; // Temas mesafesi

    private NavMeshAgent agent;
    private Transform player;
    private float killTimer;
    private bool isChasing = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = walkSpeed;
        killTimer = killTime;

        // Oyuncuyu bul (Hata vermemesi için kontrol ekledim)
        GameObject pObj = GameObject.FindGameObjectWithTag("Player");
        if (pObj != null) player = pObj.transform;

        // Başlarken UI'ları gizle
        if (countdownText != null) countdownText.gameObject.SetActive(false);
        ResetGlitchEffect(); // Efekti sıfırla

        GoToNextPoint();
    }

    void Update()
    {
        // 1. OYUNCU DOLAPTA MI?
        if (PlayerStatus.isHidden)
        {
            StopChasing();
            return;
        }

        // 2. TEMAS İLE ÖLDÜRME (Anında)
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer <= killDistance) KillPlayer();
        }

        // 3. GÖRÜŞ KONTROLÜ
        if (CanSeePlayer())
        {
            isChasing = true;
            agent.speed = runSpeed;
            agent.isStopped = false;
            agent.SetDestination(player.position);

            // --- SAYAÇ VE GLITCH ---
            killTimer -= Time.deltaTime;

            // A) Geri Sayım Yazısı
            if (countdownText != null)
            {
                countdownText.gameObject.SetActive(true);
                countdownText.text = killTimer.ToString("F1");
            }

            // B) Glitch Efekti (Kodla Animasyon) ⚡
            if (glitchImage != null)
            {
                // Süre azaldıkça intensity (şiddet) 0'dan 1'e çıkar
                float intensity = 1 - (killTimer / killTime);
                if (intensity < 0) intensity = 0;

                // 1. Yanıp Sönme (Alpha Titremesi)
                Color c = glitchImage.color;
                // Şiddet arttıkça daha görünür olur, ama rastgele titrer
                c.a = Random.Range(intensity * 0.5f, intensity);
                glitchImage.color = c;

                // 2. Pozisyon Kayması (Shake)
                // Şiddet arttıkça ekran daha çok sallanır (Max 50 piksel)
                float shakePower = intensity * 50f;
                glitchImage.rectTransform.anchoredPosition = Random.insideUnitCircle * shakePower;
            }
            // -----------------------

            if (killTimer <= 0) KillPlayer();
        }
        else
        {
            // Görmüyorsa
            if (isChasing) StopChasing();

            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                GoToNextPoint();
            }
        }
    }

    void StopChasing()
    {
        isChasing = false;
        agent.speed = walkSpeed;
        killTimer = killTime;

        if (countdownText != null) countdownText.gameObject.SetActive(false);
        ResetGlitchEffect();

        if (agent.remainingDistance < 0.5f) GoToNextPoint();
    }

    void ResetGlitchEffect()
    {
        if (glitchImage != null)
        {
            // Rengi tamamen şeffaf yap
            Color c = glitchImage.color;
            c.a = 0f;
            glitchImage.color = c;

            // Kaymış pozisyonu merkeze al
            glitchImage.rectTransform.anchoredPosition = Vector2.zero;
        }
    }

    bool CanSeePlayer()
    {
        if (player == null) return false;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer > viewDistance) return false;
        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        if (Vector3.Angle(transform.forward, dirToPlayer) > viewAngle / 2) return false;

        RaycastHit hit;
        if (Physics.Raycast(eyes.position, dirToPlayer, out hit, distanceToPlayer))
        {
            if (hit.transform.CompareTag("Player") || hit.transform == player) return true;
            else return false;
        }
        return false;
    }

    void KillPlayer()
    {
        Debug.Log("💀 SİNYAL KOPTU - ÖLDÜN!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // --- BURAYI DÜZELTTİM (Artık sadece bir tane var) ---
    void GoToNextPoint()
    {
        if (waypoints.Count == 0) return;
        int randomIndex = Random.Range(0, waypoints.Count);
        agent.destination = waypoints[randomIndex].position;
    }

    // Gizmo Çizimi
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, killDistance);
    }
}