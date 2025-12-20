using UnityEngine;
using UnityEngine.SceneManagement;

public class UnlockController : MonoBehaviour
{
    public UnlockAnimation[] animations;

    private void Start()
    {
        string last = PlayerPrefs.GetString("LastCompletedLevel", "");
        if (string.IsNullOrEmpty(last))
            return;

        int lastCompleted = int.Parse(last);
        int nextLevelIndex = lastCompleted + 1;

        if (nextLevelIndex - 1 >= 0 && nextLevelIndex - 1 < animations.Length)
        {
            UnlockAnimation anim = animations[nextLevelIndex - 1];
            if (anim != null)
            {
                anim.Play();
            }
        }

        if (PlayerPrefs.GetInt("LoadNextLevel", 0) == 1)
        {
            PlayerPrefs.SetInt("LoadNextLevel", 0);
            PlayerPrefs.Save();

            SceneManager.LoadScene(nextLevelIndex.ToString());
        }
    }

    public void GoToMap1()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
