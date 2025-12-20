using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource musicSource;
    public AudioSource sfxSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateMusicVolume(float value)
    {
        if (musicSource != null)
            musicSource.volume = value;
    }

    public void UpdateSFXVolume(float value)
    {
        if (sfxSource != null)
            sfxSource.volume = value;
    }
}
