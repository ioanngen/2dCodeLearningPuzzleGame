using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeUI : MonoBehaviour
{
    public void PlayGame()
    {
        AudioManager.Instance?.PlaySFX(SFXType.ButtonClick);
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        AudioManager.Instance?.PlaySFX(SFXType.ButtonClick);
        Application.Quit();
    }

    public void ResetGame()
    {
        PlayerPrefs.DeleteAll();
    }
}
