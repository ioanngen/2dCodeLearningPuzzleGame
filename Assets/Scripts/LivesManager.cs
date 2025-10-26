using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class LivesManager : MonoBehaviour
{
    public static LivesManager Instance;

    public int maxLives = 5;
    public int currentLives;
    public float regenTime = 1200f; // 20 minutes
    private DateTime nextLifeTime;

    [Header("UI References (Map Scene Only)")]
    public TMP_Text livesText;
    public TMP_Text timerText;
    public GameObject timerPanel;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadLivesData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            FindUIReferences();
            UpdateUI();
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            FindUIReferences();
            UpdateUI();
        }
    }

    private void Update()
    {
        HandleRegeneration();
        UpdateUI();
    }

    public bool HasLives()
    {
        return currentLives > 0;
    }

    public void LoseLife()
    {
        if (currentLives > 0)
        {
            currentLives--;

            SaveLivesData();
            UpdateUI();
        }
    }

    private void HandleRegeneration()
    {
        if (currentLives < maxLives)
        {
            if (DateTime.Now >= nextLifeTime)
            {
                currentLives++;
                SaveLivesData();

                if (currentLives < maxLives)
                    nextLifeTime = DateTime.Now.AddSeconds(regenTime);

                UpdateUI();
            }

            TimeSpan remaining = nextLifeTime - DateTime.Now;
            if (timerText != null)
                timerText.text = $"{remaining.Minutes:D2}:{remaining.Seconds:D2}";
        }
        else
        {
            if (timerPanel != null)
                timerPanel.SetActive(false);
        }
    }

    public void AddLife()
    {
        if (currentLives < maxLives)
        {
            currentLives++;
            SaveLivesData();
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        if (livesText != null)
            livesText.text = currentLives.ToString();

        if (timerPanel != null)
            timerPanel.SetActive(currentLives < maxLives);
    }

    private void LoadLivesData()
    {
        currentLives = PlayerPrefs.GetInt("CurrentLives", maxLives);
        string nextLifeTimeStr = PlayerPrefs.GetString("NextLifeTime", "");

        if (!string.IsNullOrEmpty(nextLifeTimeStr) &&
            DateTime.TryParse(nextLifeTimeStr, out DateTime savedTime))
        {
            nextLifeTime = savedTime;
        }
        else
        {
            nextLifeTime = DateTime.Now.AddSeconds(regenTime);
        }
    }

    private void SaveLivesData()
    {
        PlayerPrefs.SetInt("CurrentLives", currentLives);
        PlayerPrefs.SetString("NextLifeTime", nextLifeTime.ToString());
        PlayerPrefs.Save();
    }

    private void FindUIReferences()
    {
        // Find UI only in the Map scene
        var livesObj = GameObject.FindWithTag("LivesText");
        var timerObj = GameObject.FindWithTag("TimerText");
        var panelObj = GameObject.FindWithTag("TimerPanel");

        if (livesObj != null)
            livesText = livesObj.GetComponent<TMP_Text>();

        if (timerObj != null)
            timerText = timerObj.GetComponent<TMP_Text>();

        if (panelObj != null)
            timerPanel = panelObj;
    }
}
