using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int playerLives = 3;
    public GameObject resultPanel;
    public Text resultText;
    public Text timeText;
    public Image[] starImages; // 3 stars
    public Sprite starOn;
    public Sprite starOff;
    public Button tryAgainButton;
    public Button nextLevelButton;

    private float levelStartTime;
    private bool levelEnded = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        levelStartTime = Time.time;
        resultPanel.SetActive(false);
    }

    public void EndLevel(bool success)
    {
        if (levelEnded) return;
        levelEnded = true;

        float timeTaken = Time.time - levelStartTime;
        resultPanel.SetActive(true);

        if (success)
        {
            resultText.text = "Level Complete!";
            timeText.text = $"Time: {timeTaken:F2} seconds";

            int stars = CalculateStars(timeTaken);
            for (int i = 0; i < starImages.Length; i++)
            {
                starImages[i].sprite = (i < stars) ? starOn : starOff;
            }

            tryAgainButton.gameObject.SetActive(false);
            nextLevelButton.gameObject.SetActive(true);
        }
        else
        {
            playerLives--;
            resultText.text = $"Try Again\nLives Left: {playerLives}";
            timeText.text = "";

            foreach (var star in starImages)
                star.sprite = starOff;

            tryAgainButton.gameObject.SetActive(true);
            nextLevelButton.gameObject.SetActive(false);
        }
    }

    public void NextLevel()
    {
        int nextSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneIndex);
    }

    int CalculateStars(float time)
    {
        if (time < 10f) return 3;
        if (time < 20f) return 2;
        return 1;
    }

    public void RetryLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
