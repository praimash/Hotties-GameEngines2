using UnityEngine;

public class LockedItem : MonoBehaviour
{
    [Header("Ödül Ayarı")]
    public GameObject hiddenRewardObject; // Sahnede gizli olan (tiki kapalı) anahtarı buraya sürükle!

    public void Interact()
    {
        SkillCheckSystem system = FindFirstObjectByType<SkillCheckSystem>();

        if (system != null)
        {
            system.StartMinigame(OnMinigameSuccess);
        }
        else
        {
            Debug.LogError("SkillCheckSystem bulunamadı!");
        }
    }

    private void OnMinigameSuccess()
    {
        Unlock();
    }

    public void Unlock()
    {
        Debug.Log("🎉 KİLİT AÇILDI!");

        // 1. Gizli olan anahtarı görünür yap
        if (hiddenRewardObject != null)
        {
            hiddenRewardObject.SetActive(true); // İŞTE SİHİR BURADA!
            Debug.Log("🗝️ Gizli anahtar ortaya çıktı!");
        }
        else
        {
            Debug.LogWarning("⚠️ Kankam 'Hidden Reward Object' kutusunu boş bırakmışsın, neyi açacağımı bilmiyorum!");
        }

        // 2. Kilitli kutuyu yok et
        Destroy(gameObject);
    }
}