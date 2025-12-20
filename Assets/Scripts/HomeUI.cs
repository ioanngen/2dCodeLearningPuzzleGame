using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeUI : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("MainMenu");
        PlayerPrefs.DeleteAll();
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}
