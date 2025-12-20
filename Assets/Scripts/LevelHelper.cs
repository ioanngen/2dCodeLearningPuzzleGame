using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class LevelHelper : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI balloonText;
    public Button nextButton;
    public Button prevButton;

    [Header("Content")]
    [TextArea(2, 4)]
    public string[] balloonTexts;

    [Header("Typing Effect")]
    public float wordDelay = 0.12f;

    private int index;
    private Coroutine typingRoutine;
    private LevelHelperController controller;

    private void Awake()
    {
        controller = Object.FindFirstObjectByType<LevelHelperController>();
    }

    private void OnEnable()
    {
        index = 0;
        RefreshUI();
    }

    public void Next()
    {
        if (index == balloonTexts.Length - 1)
        {
            controller.CloseHelper();
            return;
        }

        index++;
        RefreshUI();
    }

    public void Previous()
    {
        if (index <= 0) return;

        index--;
        RefreshUI();
    }

    private void RefreshUI()
    {
        // Stop previous typing animation
        if (typingRoutine != null)
            StopCoroutine(typingRoutine);

        balloonText.text = "";

        typingRoutine = StartCoroutine(TypeText(balloonTexts[index]));

        // Previous button only when possible
        prevButton.gameObject.SetActive(index > 0);

    }

    private IEnumerator TypeText(string fullText)
    {
        string[] words = fullText.Split(' ');

        balloonText.text = "";

        foreach (string word in words)
        {
            balloonText.text += word + " ";
            yield return new WaitForSecondsRealtime(wordDelay);
        }
    }
}
