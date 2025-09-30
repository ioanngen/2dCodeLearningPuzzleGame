using System;
using UnityEngine;
using TMPro;

public class LivesManager : MonoBehaviour
{
    public static LivesManager instance;

    [Header("Lives Settings")]
    public int maxLives = 5;
    public int currentLives = 5;
    public float lifeRegenSeconds = 20f * 60f; // 20 minutes

    [Header("UI (assign in Map scene)")]
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI timerText;
    public GameObject clockIcon; // toggled when lives < max

    private DateTime nextLifeTime; // when the next life will be granted

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        LoadLives();
        UpdateUI();
    }

    void Update()
    {
        if (currentLives < maxLives)
        {
            if (DateTime.Now >= nextLifeTime)
            {
                // add one life (could add more if lot of time passed)
                AddLife();
            }
            else
            {
                UpdateTimerUI();
            }
        }
    }

    public bool HasLives()
    {
        return currentLives > 0;
    }

    // call when the player should lose a life (on level fail or exit-per-request)
    public void LoseLife()
    {
        if (currentLives <= 0) return;
        currentLives--;
        if (currentLives < maxLives)
        {
            // start countdown for next life if not already
            nextLifeTime = DateTime.Now.AddSeconds(lifeRegenSeconds);
            PlayerPrefs.SetString("NextLifeTime", nextLifeTime.ToString());
        }
        SaveLives();
        UpdateUI();
    }

    private void AddLife()
    {
        if (currentLives >= maxLives) return;

        // calculate how many full life intervals passed
        string saved = PlayerPrefs.GetString("NextLifeTime", string.Empty);
        DateTime savedNext = string.IsNullOrEmpty(saved) ? DateTime.Now : DateTime.Parse(saved);
        TimeSpan passed = DateTime.Now - (savedNext - TimeSpan.FromSeconds(lifeRegenSeconds));
        double totalSecondsPassed = (DateTime.Now - savedNext + TimeSpan.FromSeconds(lifeRegenSeconds)).TotalSeconds;
        // simpler: add 1 then set nextLifeTime = now + interval if still not full
        currentLives++;
        if (currentLives < maxLives)
        {
            nextLifeTime = DateTime.Now.AddSeconds(lifeRegenSeconds);
            PlayerPrefs.SetString("NextLifeTime", nextLifeTime.ToString());
        }
        else
        {
            PlayerPrefs.DeleteKey("NextLifeTime");
        }
        SaveLives();
        UpdateUI();
    }

    private void UpdateTimerUI()
    {
        if (timerText == null || clockIcon == null) return;
        TimeSpan remaining = nextLifeTime - DateTime.Now;
        if (remaining.TotalSeconds < 0) remaining = TimeSpan.Zero;
        timerText.text = string.Format("{0:D2}:{1:D2}", remaining.Minutes, remaining.Seconds);
        clockIcon.SetActive(true);
    }

    private void UpdateUI()
    {
        if (livesText != null) livesText.text = $"{currentLives}/{maxLives}";

        if (currentLives >= maxLives)
        {
            if (timerText != null) timerText.text = "Full";
            if (clockIcon != null) clockIcon.SetActive(false);
        }
        else
        {
            UpdateTimerUI();
        }
    }

    private void SaveLives()
    {
        PlayerPrefs.SetInt("PlayerLives", currentLives);
        PlayerPrefs.Save();
    }

    private void LoadLives()
    {
        currentLives = PlayerPrefs.GetInt("PlayerLives", maxLives);
        string next = PlayerPrefs.GetString("NextLifeTime", string.Empty);
        if (!string.IsNullOrEmpty(next))
        {
            nextLifeTime = DateTime.Parse(next);
        }
        else
        {
            nextLifeTime = DateTime.Now;
        }
    }

    // optional helper to force set values (not used automatically)
    public void SetLives(int v)
    {
        currentLives = Mathf.Clamp(v, 0, maxLives);
        if (currentLives < maxLives) nextLifeTime = DateTime.Now.AddSeconds(lifeRegenSeconds);
        SaveLives();
        UpdateUI();
    }
}
