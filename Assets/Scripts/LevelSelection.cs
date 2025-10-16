using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{

    [SerializeField] private bool unlocked;
    public Image unlockImage;
    public GameObject[] stars;

    public Sprite starSprite;

    private void Start()
    {
        //PlayerPrefs.DeleteAll();
    }

    private void Update()
    {
        UpdateLevelImage();
        UpdateLevelStatus();
    }

    private void UpdateLevelStatus()
    {
        int previousLevelNum = int.Parse(gameObject.name) - 1;
        if (PlayerPrefs.GetInt("Lv" + previousLevelNum.ToString()) > 0 && !unlocked)
        {
            unlocked = true;

            UnlockAnimation anim = GetComponent<UnlockAnimation>();
            if (anim != null)
            {
                anim.Play();
            }
        }
    }

    private void UpdateLevelImage()
    {
        if(!unlocked)
        {
            unlockImage.gameObject.SetActive(true);
            for(int i = 0; i < stars.Length; i++)
            {
                stars[i].gameObject.SetActive(false);
            }
        }
        else
        {
            unlockImage.gameObject.SetActive(false);
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].gameObject.SetActive(true);
            }

            for(int i = 0; i < PlayerPrefs.GetInt("Lv" + gameObject.name); i++)
            {
                stars[i].gameObject.GetComponent<Image>().sprite = starSprite;
            }
        }
    }

    public void PressSelection(string levelName)
    {
        if (!unlocked)
            return;

        if (!LivesManager.Instance.HasLives())
        {
            Debug.Log("No lives left! Wait for regeneration.");
            // TODO: Show UI message panel for "No lives left"
            return;
        }

        SceneManager.LoadScene(levelName);
    }
}
