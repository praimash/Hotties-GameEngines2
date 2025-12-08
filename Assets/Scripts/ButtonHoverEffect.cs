using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //color
    public Color normalColor = Color.white;
    public Color hoverColor = Color.yellow;
    //size
    public float normalScale = 1f;
    public float hoverScale = 1.1f;
    public float animationSpeed = 10f;
    //ses
    public AudioClip hoverSound;

    private Image buttonImage;
    private Vector3 targetScale;
    private Color targetColor;
    private AudioSource audioSource;

    void Start()
    {
        buttonImage = GetComponent<Image>();
        targetScale = Vector3.one * normalScale;
        targetColor = normalColor;
        if (hoverSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = hoverSound;
            audioSource.playOnAwake = false;
        }

        if (buttonImage != null)
        {
            buttonImage.color = normalColor;
        }
        transform.localScale = Vector3.one * normalScale;
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.unscaledDeltaTime * animationSpeed);

        if (buttonImage != null)
        {

            buttonImage.color = Color.Lerp(buttonImage.color, targetColor, Time.unscaledDeltaTime * animationSpeed);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = Vector3.one * hoverScale;
        targetColor = hoverColor;

        if (audioSource != null && hoverSound != null)
        {
            audioSource.Play();
        }

    }


    public void OnPointerExit(PointerEventData eventData)
    {
         targetScale = Vector3.one * normalScale;
        targetColor = normalColor;
    }
}