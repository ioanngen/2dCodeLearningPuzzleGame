using UnityEngine;
using UnityEngine.SceneManagement;

public class SingleLevel : MonoBehaviour
{
    private int currentStarsNum = 0;
    public int levelIndex;

    public void BackButton()
    {
        AudioManager.Instance?.PlaySFX(SFXType.ButtonClick);
        SceneManager.LoadScene("MainMenu");
    }

    public void PressStarsButton(int _starsNum)
    {
        AudioManager.Instance?.PlaySFX(SFXType.ButtonClick);
        currentStarsNum = _starsNum;

        if(currentStarsNum > PlayerPrefs.GetInt("Lv" + levelIndex))
        {
            PlayerPrefs.SetInt("Lv" + levelIndex, _starsNum);
        }

        Debug.Log(PlayerPrefs.GetInt("Lv" + levelIndex));

        BackButton();
    }

}
