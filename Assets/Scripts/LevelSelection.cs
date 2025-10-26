using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    [SerializeField] private bool unlocked;
    public Image unlockImage;
    public GameObject[] stars;
    public Sprite starSprite;

    private UnlockAnimation unlockAnim;

    [Header("Popup Reference")]
    public NoLivesPopup noLivesPopup;

    private void Start()
    {
        unlockAnim = GetComponent<UnlockAnimation>();

        if (unlockAnim != null && string.IsNullOrEmpty(unlockAnim.levelKey))
        {
            unlockAnim.levelKey = "Level" + gameObject.name;
        }

        UpdateLevelImage();
        UpdateLevelStatus();
    }

    private void Update()
    {
        UpdateLevelImage();
    }

    private void UpdateLevelStatus()
    {
        int currentLevelNum = int.Parse(gameObject.name);
        int previousLevelNum = currentLevelNum - 1;

        if (PlayerPrefs.GetInt("Lv" + previousLevelNum.ToString()) > 0 && !unlocked)
        {
            unlocked = true;

            if (unlockAnim != null)
            {
                bool hasPlayed = PlayerPrefs.GetInt("PlayedUnlockAnim_Level" + gameObject.name, 0) == 1;
                if (!hasPlayed)
                {
                    unlockAnim.Play();
                }
                else
                {
                    foreach (var d in unlockAnim.dots)
                    {
                        if (d != null) d.SetActive(true);
                    }
                    if (unlockAnim.levelButton != null)
                        unlockAnim.levelButton.interactable = true;
                }
            }
        }
    }

    private void UpdateLevelImage()
    {
        if (!unlocked)
        {
            unlockImage.gameObject.SetActive(true);
            foreach (var star in stars)
                star.SetActive(false);
        }
        else
        {
            unlockImage.gameObject.SetActive(false);
            foreach (var star in stars)
                star.SetActive(true);

            int starCount = PlayerPrefs.GetInt("Lv" + gameObject.name);
            for (int i = 0; i < stars.Length; i++)
            {
                Image img = stars[i].GetComponent<Image>();
                img.sprite = (i < starCount) ? starSprite : img.sprite;
            }
        }
    }

    public void PressSelection(string levelName)
    {
        if (!unlocked)
            return;

        if (!LivesManager.Instance.HasLives())
        {
            noLivesPopup.ShowPopup();
            return;
        }

        SceneManager.LoadScene(levelName);
    }
}
