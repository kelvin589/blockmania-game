using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitManager : InteractiveObjectBase
{
    [SerializeField]
    private Text exitText;

    const string levelCompleteText = "All the keys have been found. Press E to leave.";
    const string levelNotCompleteText = "There are more keys to find!";

    // Start is called before the first frame update    
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateExitText();
    }

    void UpdateExitText()
    {
        if (KeyUIManager.Instance.HasCollectedEnoughKeys())
        {
            exitText.text = levelCompleteText;
        }
        else
        {
            exitText.text = levelNotCompleteText;
        }
    }

    public override void OnInteraction()
    {
        if (KeyUIManager.Instance.HasCollectedEnoughKeys())
        {
            Debug.Log("Level complete. Exit to level selection.");
            int currentScore = ScoreUIManager.Instance.GetCurrentScore();
            string currentLevel = SceneHelper.GetCurrentLevelName();
            CheckIfUpdateScore(currentScore, currentLevel);
        }
    }

    private void CheckIfUpdateScore(int currentScore, string currentLevel)
    {
        RemoteHighScoreManager.Instance.GetHighScore(
            currentLevel,
            (storedScore) =>
            {
                if (currentScore > storedScore)
                {
                    UpdateScore(currentScore, currentLevel);
                }
                else
                {
                    StartCoroutine(SceneHelper.LoadGameSceneAsync(ScenesEnum.LevelSelector));
                }
            }
        );
    }

    private void UpdateScore(int currentScore, string currentLevel)
    {
        RemoteHighScoreManager.Instance.SetHighScore(
            currentScore,
            currentLevel,
            () => StartCoroutine(SceneHelper.LoadGameSceneAsync(ScenesEnum.LevelSelector))
        );
    }
}
