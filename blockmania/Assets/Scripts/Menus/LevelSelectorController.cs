using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectorController : Photon.MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void Awake()
    {
        // Load current level scores
        GameObject[] levelUIBoxes = GameObject.FindGameObjectsWithTag("LevelUIBox");
        foreach (GameObject levelUIBox in levelUIBoxes)
        {
            RemoteHighScoreManager.Instance.GetHighScore(
                levelUIBox.name,
                (score) =>
                {
                    Text scoreText = levelUIBox.GetComponentsInChildren<Text>()[1];
                    scoreText.text = $"High Score: {score.ToString("0000")}";
                }
            );
        }

        // Reappear cursor due to FPSController Behaviour
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ButtonHandlerBack()
    {
        StartCoroutine(SceneHelper.LoadGameSceneAsync(ScenesEnum.MainMenu));
    }

    public void ButtonHandlerLevel1()
    {
        LoadLevel(ScenesEnum.Level1);
    }

    public void ButtonHandlerLevel2()
    {
        LoadLevel(ScenesEnum.Level2);
    }

    void LoadLevel(ScenesEnum level)
    {
        StartCoroutine(SceneHelper.LoadLevelAsync((int)level));
    }
}
