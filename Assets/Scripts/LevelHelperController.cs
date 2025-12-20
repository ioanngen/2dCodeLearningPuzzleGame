using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelHelperController : MonoBehaviour
{
    [Header("Helper Settings")]
    public bool hasHelper = true;
    public GameObject helperCanvas;

    private int levelNumber;

    private void Awake()
    {
        // Get level number from scene name
        string sceneName = SceneManager.GetActiveScene().name;
        int.TryParse(sceneName, out levelNumber);
    }

    private void Start()
    {
        if (!hasHelper || levelNumber <= 0)
            return;

        string key = $"HelperSeen_Level_{levelNumber}";
        bool seen = PlayerPrefs.GetInt(key, 0) == 1;

        if (!seen)
        {
            ShowHelper();
            PlayerPrefs.SetInt(key, 1);
            PlayerPrefs.Save();
        }
    }

    private void ShowHelper()
    {
        helperCanvas.SetActive(true);
        Time.timeScale = 0f;
    }

    public void CloseHelper()
    {
        helperCanvas.SetActive(false);
        Time.timeScale = 1f;
    }
}
