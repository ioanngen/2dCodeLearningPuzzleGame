using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeUI : MonoBehaviour
{
    [Header("Panels")]
    public GameObject settingsPanel;

    [Header("Buttons")]
    public GameObject continueButton;

    [Header("Audio Sliders")]
    public Slider musicSlider;
    public Slider sfxSlider;

    private void Start()
    {
        settingsPanel.SetActive(false);

        if (SettingsManager.instance != null)
        {
            musicSlider.value = SettingsManager.instance.musicVolume;
            sfxSlider.value = SettingsManager.instance.sfxVolume;
        }

        string lastLevel = PlayerPrefs.GetString("LastPlayedLevel", "");
        if (string.IsNullOrEmpty(lastLevel))
            continueButton.SetActive(false);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ContinueGame()
    {
        string lastLevel = PlayerPrefs.GetString("LastPlayedLevel", "");
        if (!string.IsNullOrEmpty(lastLevel))
        {
            SceneManager.LoadScene(lastLevel);
        }
        else
        {
            PlayGame();
        }
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    public void OnMusicVolumeChanged(float value)
    {
        if (SettingsManager.instance != null)
            SettingsManager.instance.SetMusicVolume(value);
    }

    public void OnSFXVolumeChanged(float value)
    {
        if (SettingsManager.instance != null)
            SettingsManager.instance.SetSfxVolume(value);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}
