using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public static bool isHidden = false; // Her yerden eriþilebilir: Saklanýyor mu?

    void Start()
    {
        isHidden = false; // Oyun baþlayýnca saklanmýyoruz
    }
}