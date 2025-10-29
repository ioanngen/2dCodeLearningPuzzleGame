using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI References")]
    public GameObject resultPanel;
    public GameObject Panel;
    public NoLivesPopup noLivesPopup;
    public Text resultText;
    public Text timeText;
    public Image[] starImages;   // 3 stars
    public Sprite starOn;
    public Sprite starOff;
    public Button tryAgainButton;
    public Button nextLevelButton;
    public Button exitButton;


    private float levelStartTime;
    private bool levelEnded = false;
    private bool levelSuccess = false;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        levelStartTime = Time.time;
        if (resultPanel != null)
            resultPanel.SetActive(false);

        // Button listeners
        if (tryAgainButton != null) tryAgainButton.onClick.AddListener(TryAgain);
        if (nextLevelButton != null) nextLevelButton.onClick.AddListener(NextLevel);
    }

    public void EndLevel(bool success)
    {
        if (levelEnded) return;
        levelEnded = true;
        levelSuccess = success;

        float timeTaken = Time.time - levelStartTime;

        PauseManager pause = FindAnyObjectByType<PauseManager>();
        if (pause != null)
            timeTaken -= pause.GetTotalPausedTime();

        resultPanel.SetActive(true);
        Panel.SetActive(true);

        if (success)
        {
            resultText.text = "Level Complete!";
            timeText.text = $"Time: {timeTaken:F1}s";

            int stars = CalculateStars(timeTaken);
            for (int i = 0; i < starImages.Length; i++)
                starImages[i].sprite = (i < stars) ? starOn : starOff;

            string currentLevel = SceneManager.GetActiveScene().name;
            int prevStars = PlayerPrefs.GetInt("Lv" + currentLevel, 0);
            if (stars > prevStars)
            {
                PlayerPrefs.SetInt("Lv" + currentLevel, stars);
                PlayerPrefs.Save();
            }

            tryAgainButton.gameObject.SetActive(false);
            nextLevelButton.gameObject.SetActive(true);
        }
        else
        {
            resultText.text = "Try Again!";
            timeText.text = "";

            foreach (var img in starImages)
                img.sprite = starOff;

            tryAgainButton.gameObject.SetActive(true);
            nextLevelButton.gameObject.SetActive(false);
        }
    }


    private int CalculateStars(float time)
    {
        if (time < 10f) return 3;
        if (time < 20f) return 2;
        return 1;
    }

    public void NextLevel()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void TryAgain()
    {
        if (!LivesManager.Instance.HasLives())
        {
            noLivesPopup.ShowPopup();
            return;
        }

        LivesManager.Instance.LoseLife();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitToMap()
    {
        ConfirmLifeUsage pp = FindFirstObjectByType<ConfirmLifeUsage>();

        if (pp != null)
        {
            pp.ShowPopup(() =>
            {
                LivesManager.Instance.LoseLife();
                UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
            });
        }
        else
        {
            Debug.LogWarning("ConfirmLifeUsePopup not assigned in GameManager!");
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }
    }
    public void Exit()
    {

        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

}
