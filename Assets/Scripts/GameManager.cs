using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI References")]
    public GameObject resultPanel;
    public GameObject overlayPanel;
    public Text resultText;
    public Text timeText;
    public Image[] starImages; // 3 stars
    public Sprite starOn;
    public Sprite starOff;
    public Button tryAgainButton;
    public Button nextLevelButton;
    public Button exitButton;

    private float levelStartTime;
    private bool levelEnded = false;
    private bool success = false;  // remember result
    private int stars = 0;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        levelStartTime = Time.time;
        resultPanel.SetActive(false);
        overlayPanel.SetActive(false);

        // Wire buttons
        tryAgainButton.onClick.AddListener(RetryLevel);
        nextLevelButton.onClick.AddListener(NextLevel);
        exitButton.onClick.AddListener(ExitToMap);
    }

    public void EndLevel(bool successResult)
    {
        if (levelEnded) return;
        levelEnded = true;
        success = successResult;

        float timeTaken = Time.time - levelStartTime;
        resultPanel.SetActive(true);
        overlayPanel.SetActive(true);

        if (success)
        {
            resultText.text = "Level Complete!";
            timeText.text = $"Time: {timeTaken:F2} seconds";

            stars = CalculateStars(timeTaken);
            for (int i = 0; i < starImages.Length; i++)
                starImages[i].sprite = (i < stars) ? starOn : starOff;

            tryAgainButton.gameObject.SetActive(false);
            nextLevelButton.gameObject.SetActive(true);
        }
        else
        {
            resultText.text = $"Try Again\nLives Left: {LivesManager.instance.currentLives}";
            timeText.text = "";

            foreach (var star in starImages)
                star.sprite = starOff;

            tryAgainButton.gameObject.SetActive(true);
            nextLevelButton.gameObject.SetActive(false);
        }

        // Save stars for this level
        string currentLevel = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetInt("Lv" + currentLevel, stars);
        PlayerPrefs.Save();
    }

    private int CalculateStars(float time)
    {
        if (time < 10f) return 3;
        if (time < 20f) return 2;
        return 1;
    }

    // 🔹 Button Handlers
    public void RetryLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene("MainMenu"); // Loads map, animation can play there
    }

    public void ExitToMap()
    {
        // Case 1: Player hasn’t run yet (no result panel shown)
        if (!levelEnded)
        {
            LivesManager.instance.LoseLife();
        }
        else
        {
            // Case 2: Level ended
            if (!success)
            {
                // Failure → lose life
                LivesManager.instance.LoseLife();
            }
            // Success → no life lost
        }

        SceneManager.LoadScene("MainMenu");
    }
}
