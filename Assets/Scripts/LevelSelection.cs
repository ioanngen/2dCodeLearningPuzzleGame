using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    [SerializeField] private bool unlocked;

    [Header("UI")]
    public Image unlockImage;
    public GameObject[] stars;
    public Sprite starSprite;

    private UnlockAnimation unlockAnim;

    private void Start()
    {
        unlockAnim = GetComponent<UnlockAnimation>();

        UpdateLevelStatus();
        UpdateLevelImage();
    }

    private void UpdateLevelStatus()
    {
        if (!int.TryParse(gameObject.name, out int currentLevel))
            return;

        int previousLevel = currentLevel - 1;

        if (PlayerPrefs.GetInt("Lv" + previousLevel, 0) > 0 && !unlocked)
        {
            unlocked = true;

            if (unlockAnim != null)
            {
                unlockAnim.Play();
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

            int starCount = PlayerPrefs.GetInt("Lv" + gameObject.name, 0);
            for (int i = 0; i < stars.Length; i++)
            {
                Image img = stars[i].GetComponent<Image>();
                img.sprite = (i < starCount) ? starSprite : img.sprite;
            }
        }
    }

    public void PressSelection(string levelName)
    {
        AudioManager.Instance?.PlaySFX(SFXType.ButtonClick);

        if (!unlocked)
            return;

        if (!LivesManager.Instance.HasLives())
        {
            ToastManager.Instance.ShowMessage("No lives left!");
            return;
        }

        SceneManager.LoadScene(levelName);
    }
}
