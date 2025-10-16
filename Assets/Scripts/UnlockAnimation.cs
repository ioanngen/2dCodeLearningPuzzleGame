using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UnlockAnimation : MonoBehaviour
{
    public GameObject[] dots;
    public Button levelButton;
    public float dotDelay = 0.16f;

    public IEnumerator ShowDotsCoroutine()
    {
        if (levelButton != null) levelButton.interactable = false;

        foreach (var d in dots)
        {
            if (d != null) d.SetActive(true);
            yield return new WaitForSeconds(dotDelay);
        }

        if (levelButton != null) levelButton.interactable = true;
    }

    public void Play()
    {
        StartCoroutine(ShowDotsCoroutine());
    }
}
