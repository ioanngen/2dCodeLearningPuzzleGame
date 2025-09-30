using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnlockController : MonoBehaviour
{
    [Tooltip("Assign UnlockAnimation components in level order (index 0 = unlocks level 1->2, index 1 = 2->3, ...).")]
    public UnlockAnimation[] animations;

    [Tooltip("Name of map scene as appears in Build Settings")]
    public string mapSceneName = "MainMenu";

    IEnumerator Start()
    {
        string last = PlayerPrefs.GetString("LastCompletedLevel", "");
        if (string.IsNullOrEmpty(last)) yield break;

        int lastIndex = int.Parse(last); // assumes level scenes are named "1","2",...
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

        // reset flag if present
        if (loadNextFlag == 1)
        {
            PlayerPrefs.SetInt("LoadNextLevel", 0);
            PlayerPrefs.Save();

            // load next level scene by name (assumes numeric names "1","2"...)
            SceneManager.LoadScene(nextIndex.ToString());
        }
    }
}
