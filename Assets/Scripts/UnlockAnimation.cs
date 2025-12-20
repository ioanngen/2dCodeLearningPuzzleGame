using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UnlockAnimation : MonoBehaviour
{
    [Header("Dots Path")]
    public GameObject[] dots;

    [Header("Level Button")]
    public Button levelButton;

    [Header("Animation")]
    public float dotDelay = 0.15f;

    private string levelKey;
    private bool hasPlayed;

    private void Awake()
    {
        levelKey = "Level" + gameObject.name;

        hasPlayed = PlayerPrefs.GetInt("UnlockPlayed_" + levelKey, 0) == 1;

        if (hasPlayed)
        {
            ShowDotsInstant();
        }
        else
        {
            HideDots();
        }
    }

    private void HideDots()
    {
        foreach (var d in dots)
            if (d != null) d.SetActive(false);

        if (levelButton != null)
            levelButton.interactable = false;
    }

    private void ShowDotsInstant()
    {
        foreach (var d in dots)
            if (d != null) d.SetActive(true);

        if (levelButton != null)
            levelButton.interactable = true;
    }

    public void Play()
    {
        if (hasPlayed)
            return;

        StartCoroutine(PlayAnimation());
    }

    private IEnumerator PlayAnimation()
    {
        hasPlayed = true;

        PlayerPrefs.SetInt("UnlockPlayed_" + levelKey, 1);
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
