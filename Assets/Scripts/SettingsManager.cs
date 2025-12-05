using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    //audio settings
    public Slider MasterVolumeSlider;
    public Slider MusicVolumeSlider;
    public Slider SFXVolumeSlider;
    // panel
    public GameObject settingsPanel;

   

    void Start()
    {
        settingsPanel.SetActive(false);

        LoadAudioSettings();



    }

    void LoadAudioSettings()
    {
        MasterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
        MusicVolumeSlider.value = PlayerPrefs.GetFloat("MUsicVolume", 1f);
        SFXVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
    }
    //audio
    public void SetMasterVolume(float volume)
    {
        Debug.Log("master volume" + volume);
    }
    public void SetMusicVolume(float volume)
    {
        Debug.Log("music volume" + volume);
    }
    public void SetSFXVolume(float volume)
    {
        Debug.Log("SFX volume" + volume);

    }
    //settings panel
    public void OpenSettings()
    {
        settingsPanel.SetActive(true);

    }
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }
    public void ApplySettings()
    {
        PlayerPrefs.SetFloat("MasterVolume", MasterVolumeSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", MusicVolumeSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", SFXVolumeSlider.value);
        Debug.Log("Settingscomplated");
    }
    
}
