using UnityEngine;
using UnityEngine.UI;
using System;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager instance;

    public float musicVolume = 1f;
    public float sfxVolume = 1f;

    public event Action<float> OnMusicVolumeChanged;
    public event Action<float> OnSFXVolumeChanged;

    private Slider musicSlider;
    private Slider sfxSlider;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
            sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterSliders(Slider music, Slider sfx)
    {
        musicSlider = music;
        sfxSlider = sfx;

        if (musicSlider != null)
            musicSlider.value = musicVolume;

        if (sfxSlider != null)
            sfxSlider.value = sfxVolume;
    }

    public void SetMusicVolume(float value)
    {
        musicVolume = value;
        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();

        OnMusicVolumeChanged?.Invoke(value);
        AudioManager.Instance.UpdateMusicVolume(value);
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = value;
        PlayerPrefs.SetFloat("SFXVolume", value);
        PlayerPrefs.Save();

        OnSFXVolumeChanged?.Invoke(value);
        AudioManager.Instance.UpdateSFXVolume(value);
    }
}
