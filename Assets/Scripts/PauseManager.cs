using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject pausePopup;
    public CanvasGroup popupCanvasGroup;
    public Button resumeButton;
    public Button restartButton;
    public Button settingsButton;
    public NoLivesPopup noLivesPopup;

    private bool isPaused = false;
    private float pauseStartTime;
    private float totalPausedTime;

    private void Start()
    {
        if (pausePopup != null)
            pausePopup.SetActive(false);
    }

    private void Update()
    {
        // Pause with Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    public void PauseGame()
    {
        if (pausePopup == null) return;

        isPaused = true;
        pauseStartTime = Time.time;
        Time.timeScale = 0f; // Stop all in-game time

        pausePopup.SetActive(true);
        StartCoroutine(AnimatePopupIn());
    }

    public void ResumeGame()
    {
        if (!isPaused) return;

        isPaused = false;
        totalPausedTime += Time.time - pauseStartTime;

        Time.timeScale = 1f;
        if (pausePopup != null)
            pausePopup.SetActive(false);
    }

    private IEnumerator AnimatePopupIn()
    {
        if (popupCanvasGroup == null) yield break;

        popupCanvasGroup.alpha = 0f;
        popupCanvasGroup.transform.localScale = Vector3.one * 0.8f;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime * 3f; // unscaled time because Time.timeScale = 0
            popupCanvasGroup.alpha = Mathf.Lerp(0f, 1f, t);
            popupCanvasGroup.transform.localScale = Vector3.Lerp(Vector3.one * 0.8f, Vector3.one, t);
            yield return null;
        }
    }

    public void RestartLevel()
    {
        ConfirmLifeUsage pp = FindFirstObjectByType<ConfirmLifeUsage>();

        Time.timeScale = 1f;

        if (!LivesManager.Instance.HasLives())
        {
            noLivesPopup.ShowPopup();
            return;
        }
        else
        {
            pp.ShowPopup(() =>
            {
                LivesManager.Instance.LoseLife();
                UnityEngine.SceneManagement.SceneManager.LoadScene(
                    UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            });
        }
    }

    public void OpenSettings()
    {
        GameObject settingsPanel = GameObject.FindWithTag("SettingsPanel");
        if (settingsPanel != null)
            settingsPanel.SetActive(true);
    }

    public float GetTotalPausedTime()
    {
        return totalPausedTime;
    }
}
