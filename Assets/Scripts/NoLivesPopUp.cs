using UnityEngine;

public class NoLivesPopup : MonoBehaviour
{
    public static NoLivesPopup instance;

    void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
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
