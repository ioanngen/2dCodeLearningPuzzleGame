using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NoLivesPopup : MonoBehaviour
{
    [Header("UI References")]
    public GameObject popupPanel;
    public CanvasGroup popupCanvasGroup;
    public RectTransform popupTransform;

    [Header("Background Overlay")]
    public Image backgroundOverlay;
    public float overlayFadeDuration = 0.3f;
    [Range(0f, 1f)] public float overlayAlpha = 0.5f;

    public void ShowPopup()
    {
        if (popupPanel == null) return;

        popupPanel.SetActive(true);

        if (backgroundOverlay != null)
        {
            backgroundOverlay.gameObject.SetActive(true);
            StartCoroutine(FadeOverlayIn());
        }

        StartCoroutine(AnimatePopupIn());
    }

    private IEnumerator AnimatePopupIn()
    {
        if (popupCanvasGroup == null || popupTransform == null)
            yield break;

        popupCanvasGroup.alpha = 0f;
        popupTransform.localScale = Vector3.one * 0.8f;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 3f;
            popupCanvasGroup.alpha = Mathf.Lerp(0, 1, t);
            popupTransform.localScale = Vector3.Lerp(Vector3.one * 0.8f, Vector3.one, t);
            yield return null;
        }
    }

    private IEnumerator FadeOverlayIn()
    {
        Color c = backgroundOverlay.color;
        c.a = 0f;
        backgroundOverlay.color = c;

        float t = 0f;
        while (t < overlayFadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0, overlayAlpha, t / overlayFadeDuration);
            backgroundOverlay.color = c;
            yield return null;
        }

        c.a = overlayAlpha;
        backgroundOverlay.color = c;
    }
}
