using UnityEngine;

public class InteractionSystem : MonoBehaviour
{
    [Header("Genel Ayarlar")]
    public float interactionDistance = 3f;
    public LayerMask interactionLayer;
    public Camera cam;

    [Header("Diğer Eşyalar (Itemlar)")]
    public float hoverScaleAmount = 1.1f;
    public float smoothSpeed = 10f;

    [Header("Saklanma Yerleri (Renk Değişimi)")]
    public Color highlightColor = Color.gray; // Üzerine gelince hangi renk olsun?

    // --- HAFIZA (Büyüme İçin) ---
    private Transform currentHoverObject;
    private Transform lastHoverObject;
    private Vector3 currentOriginalScale;
    private Vector3 lastOriginalScale;

    // --- HAFIZA (Renk İçin) ---
    private Renderer currentRenderer;     // Şu an rengini değiştirdiğimiz obje
    private Color originalColor;          // Objenin gerçek rengi

    void Update()
    {
        // Eğer saklanıyorsak raycast atıp sistemi yormayalım
        if (PlayerStatus.isHidden) return;

        ShootRayAndHover();
        HandleScaling();
    }

    void ShootRayAndHover()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance, interactionLayer))
        {
            Transform hitTransform = hit.transform;

            // -----------------------------------------------------------
            // 1. ÖNCEKİ RENKLENDİRMEYİ TEMİZLE (Başka objeye geçtiysek)
            // -----------------------------------------------------------
            if (currentRenderer != null && hitTransform != currentRenderer.transform)
            {
                currentRenderer.material.color = originalColor; // Eski rengine dön
                currentRenderer = null;
            }

            // -----------------------------------------------------------
            // 2. TÜR KONTROLÜ: BU NE? (Kapı mı, Saklanma yeri mi, Eşya mı?)
            // -----------------------------------------------------------

            // A) SAKLANMA YERİ Mİ? (Renk Değişsin)
            HidingSpot hidingSpot = hit.collider.GetComponent<HidingSpot>();
            if (hidingSpot == null) hidingSpot = hit.collider.GetComponentInParent<HidingSpot>();

            if (hidingSpot != null)
            {
                // Büyümeyi İptal Et (Eğer önceden büyüyen bir şeye bakıyorsak küçülsün)
                ResetHoverScale();

                // Renk Değiştirme Mantığı
                Renderer rend = hit.collider.GetComponent<Renderer>();
                if (rend == null) rend = hit.collider.GetComponentInChildren<Renderer>(); // Kendisinde yoksa çocuğuna bak (Yatak vb.)

                if (rend != null && currentRenderer != rend)
                {
                    currentRenderer = rend;
                    originalColor = rend.material.color; // Orijinal rengi kaydet
                    currentRenderer.material.color = highlightColor; // Yeni rengi ver
                }
            }
            // B) KAPI MI? (Hiçbir görsel efekt olmasın)
            else if (hit.collider.GetComponent<SimpleDoor>() != null || hit.collider.GetComponentInParent<SimpleDoor>() != null)
            {
                ResetHoverScale();
                ResetColor();
            }
            // C) NORMAL EŞYA MI? (Büyüsün)
            else
            {
                ResetColor(); // Renk varsa temizle

                // Yeni bir objeye bakıyorsak büyüme listesine al
                if (currentHoverObject != hitTransform)
                {
                    if (currentHoverObject != null)
                    {
                        lastHoverObject = currentHoverObject;
                        lastOriginalScale = currentOriginalScale;
                    }
                    currentHoverObject = hitTransform;
                    currentOriginalScale = currentHoverObject.localScale;
                }
            }

            // -----------------------------------------------------------
            // 3. TIKLAMA (INTERACT) İŞLEMLERİ
            // -----------------------------------------------------------
            if (Input.GetKeyDown(KeyCode.E))
            {
                // Tıklayınca görsel efektleri sıfırla
                if (currentHoverObject != null) currentHoverObject.localScale = currentOriginalScale;
                ResetColor();

                // SAKLANMA YERİ
                if (hidingSpot != null) { hidingSpot.Interact(); return; }

                // KAPI
                SimpleDoor simpleDoor = hit.collider.GetComponent<SimpleDoor>();
                if (simpleDoor == null) simpleDoor = hit.collider.GetComponentInParent<SimpleDoor>();
                if (simpleDoor != null) { simpleDoor.Interact(); return; }

                // DİĞERLERİ
                InspectableItem inspectItem = hit.collider.GetComponent<InspectableItem>();
                if (inspectItem != null) { inspectItem.Interact(); return; }

                // ... Buraya diğer item kodlarını (LockedDoor vb.) ekleyebilirsin ...
            }
        }
        else
        {
            // Boşluğa bakıyorsak her şeyi resetle
            ResetHoverScale();
            ResetColor();
        }
    }

    // --- YARDIMCI FONKSİYONLAR ---

    void HandleScaling()
    {
        if (currentHoverObject != null)
        {
            Vector3 targetScale = currentOriginalScale * hoverScaleAmount;
            currentHoverObject.localScale = Vector3.Lerp(currentHoverObject.localScale, targetScale, Time.deltaTime * smoothSpeed);
        }
        if (lastHoverObject != null)
        {
            lastHoverObject.localScale = Vector3.Lerp(lastHoverObject.localScale, lastOriginalScale, Time.deltaTime * smoothSpeed);
            if (Vector3.Distance(lastHoverObject.localScale, lastOriginalScale) < 0.01f)
            {
                lastHoverObject.localScale = lastOriginalScale;
                lastHoverObject = null;
            }
        }
    }

    void ResetHoverScale()
    {
        if (currentHoverObject != null)
        {
            lastHoverObject = currentHoverObject;
            lastOriginalScale = currentOriginalScale;
            currentHoverObject = null;
        }
    }

    void ResetColor()
    {
        if (currentRenderer != null)
        {
            currentRenderer.material.color = originalColor;
            currentRenderer = null;
        }
    }
}