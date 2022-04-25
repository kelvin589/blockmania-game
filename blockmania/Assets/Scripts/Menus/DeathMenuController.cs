using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenuController : Photon.MonoBehaviour
{
    [SerializeField]
    GameObject deathPanel;
    [SerializeField]
    GameObject player;
    bool isPaused;
    GameObject crosshair;

    void Awake()
    {
        // Disable Load and restart button in multiplayer
        if (PhotonNetwork.inRoom)
        {
            deathPanel.transform.GetChild(3).gameObject.SetActive(false);
            deathPanel.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    void Start()
    {
        deathPanel.SetActive(false);
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

    public void ShowDeathMenu()
    {
        crosshair.SetActive(false);
        player.SetActive(false);
        deathPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void HideDeathMenu()
    {
        crosshair.SetActive(true);
        player.SetActive(true);
        deathPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void RestartButton()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(SceneHelper.LoadGameSceneAsync((ScenesEnum)currentLevel));
    }

    public void LoadButton()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentLevel);
        SaveGameController.Instance.shouldLoadSave = true;
    }

    public void LevelSelectButton()
    {
        StartCoroutine(SceneHelper.LoadGameSceneAsync(ScenesEnum.LevelSelector));
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
