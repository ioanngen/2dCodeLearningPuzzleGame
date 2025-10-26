using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnlockController : MonoBehaviour
{
    public UnlockAnimation[] animations;
    public string mapSceneName = "MainMenu";

    IEnumerator Start()
    {
        string last = PlayerPrefs.GetString("LastCompletedLevel", "");
        if (string.IsNullOrEmpty(last)) yield break;

        int lastIndex = int.Parse(last);
        int nextIndex = lastIndex + 1;

        int loadNextFlag = PlayerPrefs.GetInt("LoadNextLevel", 0);

        if (nextIndex - 1 >= 0 && nextIndex - 1 < animations.Length)
        {
            var anim = animations[nextIndex - 1];
            if (anim != null)
            {
                yield return StartCoroutine(anim.ShowDotsCoroutine());
            }
        }

        if (loadNextFlag == 1)
        {
            PlayerPrefs.SetInt("LoadNextLevel", 0);
            PlayerPrefs.Save();

            SceneManager.LoadScene(nextIndex.ToString());
        }
    }
}
