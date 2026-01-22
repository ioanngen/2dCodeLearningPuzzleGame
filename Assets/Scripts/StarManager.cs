using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class StarManager : MonoBehaviour
{
    public static StarManager Instance;

    [Header("UI References")]
    public TMP_Text totalStarsText;
    public Button addLifeButton;
    public Button nextWorldButton;
    public bool BackButton;

    [Header("Config")]
    public int starsPerLife = 10;
    public int requiredStarsForNextWorld = 30;
    public int totalLevelsInWorld = 13;
    public float starCountAnimSpeed = 3f;

    private int totalStars;
    private Coroutine countAnimRoutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        totalStars = PlayerPrefs.GetInt("TotalStars", 0);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RefreshUIReferences();
        UpdateUI(forceInstant: true);
    }

    private void RefreshUIReferences()
    {
        if (totalStarsText == null)
            totalStarsText = GameObject.FindWithTag("TotalStarsText")?.GetComponent<TMP_Text>();

        if (addLifeButton == null)
            addLifeButton = GameObject.FindWithTag("AddLifeButton")?.GetComponent<Button>();

        if (nextWorldButton == null)
            nextWorldButton = GameObject.FindWithTag("NextWorldButton")?.GetComponent<Button>();

        if (addLifeButton != null)
        {
            addLifeButton.onClick.RemoveAllListeners();
            addLifeButton.onClick.AddListener(TryExchangeStarsForLife);
        }

        if (nextWorldButton != null)
        {
            nextWorldButton.onClick.RemoveAllListeners();
            nextWorldButton.onClick.AddListener(TryUnlockNextWorld);
        }

        UpdateUI(forceInstant: true);
    }

    public void AddStars(int count)
    {
        ToastManager.Instance.ShowMessage("Congrats! You have earned " + count + " stars!");
        int oldStars = totalStars;
        totalStars += count;
        PlayerPrefs.SetInt("TotalStars", totalStars);
        PlayerPrefs.Save();
        AnimateStarCount(oldStars, totalStars);
    }

    private void AnimateStarCount(int from, int to)
    {
        if (countAnimRoutine != null)
            StopCoroutine(countAnimRoutine);

        countAnimRoutine = StartCoroutine(AnimateStarCountRoutine(from, to));
    }

    private IEnumerator AnimateStarCountRoutine(int from, int to)
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * starCountAnimSpeed;
            int displayed = Mathf.RoundToInt(Mathf.Lerp(from, to, t));
            if (totalStarsText != null)
                totalStarsText.text = displayed.ToString();
            yield return null;
        }

        if (totalStarsText != null)
            totalStarsText.text = to.ToString();

        UpdateUI(forceInstant: true);
    }

    public void UpdateUI(bool forceInstant = false)
    {
        if (totalStarsText != null && forceInstant)
            totalStarsText.text = totalStars.ToString();

        if (!BackButton)
        {
            if (nextWorldButton != null)
                nextWorldButton.interactable = CanUnlockNextWorld();
        }
        else
        {
            nextWorldButton.interactable = true;
        }


        if (addLifeButton != null && LivesManager.Instance != null)
        {
            bool canAddLife = LivesManager.Instance.currentLives < LivesManager.Instance.maxLives;

            var img = addLifeButton.GetComponent<Image>();
            if (img != null)
                img.color = canAddLife ? Color.white : new Color(1, 1, 1, 0.4f);
        }
    }

    private void TryExchangeStarsForLife()
    {
        if (LivesManager.Instance.currentLives >= LivesManager.Instance.maxLives)
        {
            ToastManager.Instance.ShowMessage("Already at max lives.");
            UpdateUI();
            return;
        }

        if (totalStars >= starsPerLife)
        {
            totalStars -= starsPerLife;
            PlayerPrefs.SetInt("TotalStars", totalStars);
            PlayerPrefs.Save();

            LivesManager.Instance.AddLife();
            AnimateStarCount(totalStars + starsPerLife, totalStars);
        }
        else
        {
            ToastManager.Instance.ShowMessage("Not enough stars to buy a life!");
        }
    }

    public void TryUnlockNextWorld()
    {
        AudioManager.Instance?.PlaySFX(SFXType.ButtonClick);
        if (CanUnlockNextWorld())
        {
            GoToMap2();
        }
        else
        {
            ToastManager.Instance.ShowMessage("No Enough Stars to Unlock Next World!");
        }
    }

    public void GoToMap2()
    {
        AudioManager.Instance?.PlaySFX(SFXType.ButtonClick);
        SceneManager.LoadScene("MainMenu 2");
    }

    private bool CanUnlockNextWorld()
    {
        int completedCount = 0;
        for (int i = 1; i <= totalLevelsInWorld; i++)
        {
            if (PlayerPrefs.GetInt("Lv" + i) > 0)
                completedCount++;
        }
        return completedCount >= totalLevelsInWorld && totalStars >= requiredStarsForNextWorld;
    }
}