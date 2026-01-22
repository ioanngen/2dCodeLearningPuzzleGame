using UnityEngine;
using UnityEngine.UI;

public class SceneSettingsUI : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;

    private void Start()
    {
        if (SettingsManager.instance != null)
            SettingsManager.instance.RegisterSliders(musicSlider, sfxSlider);

        musicSlider.onValueChanged.AddListener(value => SettingsManager.instance.SetMusicVolume(value));
        sfxSlider.onValueChanged.AddListener(value => SettingsManager.instance.SetSFXVolume(value));
    }
}
