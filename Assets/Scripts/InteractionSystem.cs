using UnityEngine;
using System.Collections.Generic; // Listeler için gerekli

public class InteractionSystem : MonoBehaviour
{
    [Header("Genel Ayarlar")]
    public float interactionDistance = 3f;
    public LayerMask interactionLayer;
    public Camera cam;

    [Header("Normal Eşyalar (Sadece Büyüme)")]
    public float hoverScaleAmount = 1.1f;
    public float smoothSpeed = 10f;

    [Header("Saklanma Yerleri (Sadece Renk)")]
    public Color highlightColor = Color.gray;

    // --- HAFIZA (Büyüme İçin) ---
    private Transform currentHoverObject;
    private Transform lastHoverObject;
    private Vector3 currentOriginalScale;
    private Vector3 lastOriginalScale;

    // --- HAFIZA (Renk İçin - ARTIK LİSTE KULLANIYORUZ) ---
    private HidingSpot currentHidingSpot; // Şu an baktığımız saklanma yeri
    private List<Renderer> currentRenderers = new List<Renderer>(); // Boyadığımız tüm parçalar
    private List<Color> originalColors = new List<Color>(); // O parçaların eski renkleri

    void Update()
    {
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

            // --- TÜR KONTROLÜ ---

            SimpleDoor door = hit.collider.GetComponent<SimpleDoor>();
            if (door == null) door = hit.collider.GetComponentInParent<SimpleDoor>();

            HidingSpot hidingSpot = hit.collider.GetComponent<HidingSpot>();
            if (hidingSpot == null) hidingSpot = hit.collider.GetComponentInParent<HidingSpot>();

            // 1. KAPI MI? -> HİÇBİR ŞEY YAPMA 🚪
            if (door != null)
            {
                ResetHoverScale();
                ResetColor();
            }
            // 2. SAKLANMA YERİ Mİ? -> TÜM PARÇALARI BOYA 🎨
            else if (hidingSpot != null)
            {
                ResetHoverScale(); // Büyümeyi iptal et

                // Eğer yeni bir saklanma yerine bakıyorsak işlemleri başlat
                if (currentHidingSpot != hidingSpot)
                {
                    ResetColor(); // Önceki boyadıklarımızı temizle

                    currentHidingSpot = hidingSpot; // Yeni hedefi kaydet

                    // HidingSpot scriptinin olduğu objenin altındaki TÜM Render'ları bul
                    Renderer[] allRenderers = hidingSpot.GetComponentsInChildren<Renderer>();

                    foreach (Renderer rend in allRenderers)
                    {
                        currentRenderers.Add(rend);
                        originalColors.Add(rend.material.color); // Orijinal rengi sakla
                        rend.material.color = highlightColor;    // Yeni rengi bas
                    }
                }
            }
            // 3. NORMAL EŞYA MI? -> SADECE BÜYÜME (SCALE) 🔍
            else
            {
                ResetColor(); // Renkleri temizle

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

            // --- TIKLAMA (E TUŞU) ---
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (currentHoverObject != null) currentHoverObject.localScale = currentOriginalScale;
                ResetColor();

                if (hidingSpot != null) { hidingSpot.Interact(); return; }
                if (door != null) { door.Interact(); return; }

                InspectableItem inspectItem = hit.collider.GetComponent<InspectableItem>();
                if (inspectItem != null) { inspectItem.Interact(); return; }
            }
        }
        else
        {
            // Boşluğa bakıyorsak her şeyi resetle
            ResetHoverScale();
            ResetColor();
        }
    }

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

    // --- YENİ RENK SIFIRLAMA SİSTEMİ (LİSTE İÇİN) ---
    void ResetColor()
    {
        // Listede kayıtlı tüm parçaları eski rengine döndür
        if (currentRenderers.Count > 0)
        {
            for (int i = 0; i < currentRenderers.Count; i++)
            {
                if (currentRenderers[i] != null) // Obje yok olmadıysa
                {
                    currentRenderers[i].material.color = originalColors[i];
                }
            }

            // Listeleri temizle ki bir sonraki işlem için boşalsın
            currentRenderers.Clear();
            originalColors.Clear();
            currentHidingSpot = null;
        }
    }
}