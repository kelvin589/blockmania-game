using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : Photon.MonoBehaviour
{
    [SerializeField]
    GameObject pausePanel;
    [SerializeField]
    GameObject player;
    bool isPaused;
    GameObject crosshair;

    void Awake()
    {
        // Disable Load, restart and save button in multiplayer
        if (PhotonNetwork.inRoom)
        {
            pausePanel.transform.GetChild(3).gameObject.SetActive(false);
            pausePanel.transform.GetChild(5).gameObject.SetActive(false);
            pausePanel.transform.GetChild(4).gameObject.SetActive(false);
        }
    }

    void Start()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        if (PhotonNetwork.inRoom)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in players)
            {
                if (player.GetComponent<PhotonView>().isMine)
                {
                    this.player = player;
                }
            }
        }
        else
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        crosshair = FindObjectOfType<PlayerInteractionController>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        // Escape does not work in the editor
    #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Q))
    #else
        if (Input.GetKeyDown(KeyCode.Escape))
    #endif
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    void Pause()
    {
        crosshair.SetActive(false);
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pausePanel.SetActive(true);
        // Need to disable player to unlock mouse
        player.SetActive(false);

        // Don't deactivate enemies in multiplayer
        if (PhotonNetwork.inRoom) { return; }
        SetEnemiesActive(false);
    }

    void Resume()
    {
        crosshair.SetActive(true);
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pausePanel.SetActive(false);
        // Need to disable player to unlock mouse
        player.SetActive(true);

        // Don't deactivate enemies in multiplayer
        if (PhotonNetwork.inRoom) { return; }
        SetEnemiesActive(true);
    }

    void SetEnemiesActive(bool isActive)
    {
        AgentController[] enemies = Resources.FindObjectsOfTypeAll<AgentController>();
        foreach (AgentController enemy in enemies)
        {
            enemy.gameObject.SetActive(isActive);
        }
    }

    public void ResumeButton()
    {
        Resume();
    }

    public void LevelSelectButton()
    {
        StartCoroutine(SceneHelper.LoadGameSceneAsync(ScenesEnum.LevelSelector));
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void RestartButton()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(SceneHelper.LoadGameSceneAsync((ScenesEnum)currentLevel));
    }

    public void SaveButton()
    {
        SaveGameController.Instance.Save(SceneHelper.GetCurrentLevelName());
    }

    public void LoadButton()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentLevel);
        SaveGameController.Instance.shouldLoadSave = true;
        Resume();
    }
}
