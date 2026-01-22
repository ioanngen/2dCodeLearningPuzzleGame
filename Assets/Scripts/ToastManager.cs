using UnityEngine;
using TMPro;
using System.Collections;

public class ToastManager : MonoBehaviour
{
    public static ToastManager Instance;

    public CanvasGroup canvasGroup;
    public TextMeshProUGUI messageText;

    private Coroutine routine;

    private void Awake()
    {
        Instance = this;
        HideInstant();
    }

    public void ShowMessage(string message, float duration = 2f)
    {
        if (routine != null)
            StopCoroutine(routine);

        messageText.text = message;
        routine = StartCoroutine(ShowRoutine(duration));
    }

    private IEnumerator ShowRoutine(float duration)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = false;
        yield return new WaitForSecondsRealtime(duration);
        HideInstant();
    }

    private void HideInstant()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
    }
}
