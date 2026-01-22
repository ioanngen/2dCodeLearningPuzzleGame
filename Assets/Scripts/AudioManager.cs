using UnityEngine;
using UnityEngine.SceneManagement;

public enum MusicType
{
    Home,
    Gameplay
}

public enum SFXType
{
    ButtonClick,
    AttachBlock,
    LevelWin,
    LevelFail,
    WrongBlock
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Music")]
    public AudioSource musicSource;
    public AudioClip homeMusic;
    public AudioClip gameplayMusic;

    [Header("SFX")]
    public AudioSource sfxSource;
    public AudioClip buttonClick;
    public AudioClip attachBlock;
    public AudioClip levelWin;
    public AudioClip levelFail;
    public AudioClip wrongBlock;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Contains("Home"))
            PlayMusic(MusicType.Home);
        else
            PlayMusic(MusicType.Gameplay);
    }

    public void PlayMusic(MusicType type)
    {
        AudioClip clip = type == MusicType.Home ? homeMusic : gameplayMusic;

        if (musicSource.clip == clip) return;

        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlaySFX(SFXType type)
    {
        AudioClip clip = type switch
        {
            SFXType.ButtonClick => buttonClick,
            SFXType.AttachBlock => attachBlock,
            SFXType.LevelWin => levelWin,
            SFXType.LevelFail => levelFail,
            SFXType.WrongBlock => wrongBlock,
            _ => null
        };

        if (clip != null)
            sfxSource.PlayOneShot(clip);
    }

    public void UpdateMusicVolume(float value)
    {
        musicSource.volume = value;
    }

    public void UpdateSFXVolume(float value)
    {
        musicSource.volume = value;
    }
}
