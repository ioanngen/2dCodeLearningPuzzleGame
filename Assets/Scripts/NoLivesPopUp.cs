using UnityEngine;

public class NoLivesPopup : MonoBehaviour
{
    public static NoLivesPopup instance;

    void Awake()
    {
        instance = this;
        gameObject.SetActive(false); // hidden at start
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
