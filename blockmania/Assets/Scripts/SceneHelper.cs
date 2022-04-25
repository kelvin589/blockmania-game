using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// This scenes enum matches those in the build index
public enum ScenesEnum : int
{
    MainMenu = 0,
    LevelSelector = 1,
    Level1 = 2,
    HelpMenu = 3,
    Level2 = 4,
    SettingsMenu = 5
}

public class SceneHelper : MonoBehaviour
{
    public static IEnumerator LoadGameSceneAsync(ScenesEnum scene)
    {
        // Use a coroutine to load the Scene in the background
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync((int)scene);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public static IEnumerator LoadLevelAsync(int scene)
    {
        // Use a coroutine to load the Scene in the background
        AsyncOperation asyncLoad = PhotonNetwork.LoadLevelAsync(scene);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public static string GetCurrentLevelName() {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        return ((ScenesEnum)currentLevel).ToString();
    }
}
