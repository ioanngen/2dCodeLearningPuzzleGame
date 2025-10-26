using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UnlockAnimation : MonoBehaviour
{
    [Header("Unlock Setup")]
    public GameObject[] dots;
    public Button levelButton;
    public float dotDelay = 0.16f;

    [HideInInspector] public string levelKey;

    private bool hasPlayed = false;

    private void Awake()
    {
        if (!string.IsNullOrEmpty(levelKey))
        {
            hasPlayed = PlayerPrefs.GetInt("PlayedUnlockAnim_" + levelKey, 0) == 1;
        }

        if (hasPlayed)
        {
            ShowDotsInstantly();
        }
        else
        {
            HideDots();
        }
    }

    private void HideDots()
    {
        foreach (var d in dots)
        {
            if (d != null) d.SetActive(false);
        }

        if (levelButton != null)
            levelButton.interactable = false;
    }

    private void ShowDotsInstantly()
    {
        foreach (var d in dots)
        {
            if (d != null) d.SetActive(true);
        }

        if (levelButton != null)
            levelButton.interactable = true;
    }

    public void Play()
    {
        if (hasPlayed) return;
        StartCoroutine(ShowDotsCoroutine());
    }

    public IEnumerator ShowDotsCoroutine()
    {
        hasPlayed = true;
        PlayerPrefs.SetInt("PlayedUnlockAnim_" + levelKey, 1);
        PlayerPrefs.Save();

        if (levelButton != null)
            levelButton.interactable = false;

        foreach (var d in dots)
        {
            if (d != null)
            {
                d.SetActive(true);
                yield return new WaitForSeconds(dotDelay);
            }
        }

        if (levelButton != null)
            levelButton.interactable = true;
    }
}
