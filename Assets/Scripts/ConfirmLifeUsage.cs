using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ConfirmLifeUsage : MonoBehaviour
{
    [Header("UI References")]
    public GameObject popupPanel;
    public CanvasGroup canvasGroup;
    public Image backgroundOverlay;

    private System.Action onConfirmAction;

    private void Awake()
    {
        if (popupPanel != null)
            popupPanel.SetActive(false);

        if (canvasGroup != null)
            canvasGroup.alpha = 0f;
    }

    public void ShowPopup(System.Action onConfirm)
    {
        onConfirmAction = onConfirm;

        if (popupPanel != null && !popupPanel.activeSelf)
            popupPanel.SetActive(true);

        StopAllCoroutines();
        StartCoroutine(AnimatePopupIn());
    }

    private IEnumerator AnimatePopupIn()
    {
        if (canvasGroup == null) yield break;

        canvasGroup.alpha = 0f;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime * 3f;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t);

            if (backgroundOverlay != null)
                backgroundOverlay.color = new Color(0, 0, 0, Mathf.Lerp(0f, 0.8f, t));

            yield return null;
        }

        canvasGroup.alpha = 1f;
        if (backgroundOverlay != null)
            backgroundOverlay.color = new Color(0, 0, 0, 0.8f);
    }

    public void OnYesClicked()
    {
        onConfirmAction?.Invoke();
        HidePopup();
    }

    public void OnNoClicked()
    {
        HidePopup();
    }

    private void HidePopup()
    {
        StopAllCoroutines();
        StartCoroutine(AnimatePopupOut());
    }

    private IEnumerator AnimatePopupOut()
    {
        if (canvasGroup == null) yield break;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime * 3f;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t);

            if (backgroundOverlay != null)
                backgroundOverlay.color = new Color(0, 0, 0, Mathf.Lerp(0.5f, 0f, t));

            yield return null;
        }

        canvasGroup.alpha = 0f;
        if (backgroundOverlay != null)
            backgroundOverlay.color = new Color(0, 0, 0, 0f);

        if (popupPanel != null)
            popupPanel.SetActive(false);
    }
}
