using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class RoomController : MonoBehaviour
{
    public static RoomController Instance { get; private set; }

    public static readonly string[] CHARACTERS = {"Amy", "Claire", "Timmy", "Ty"};

    void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
        DontDestroyOnLoad(gameObject);
    }

    public void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name.Equals("LevelSelector") && PhotonNetwork.inRoom) {
            PhotonNetwork.isMessageQueueRunning = false;
        }
        // If we are loading a level, we should create the player
        if (Regex.IsMatch(scene.name, @"Level\d+"))
        {
            Vector3 pos = new Vector3(-18.9f, 2.108f, -8.08f);
            string randomCharacter = CHARACTERS[Random.Range(0, CHARACTERS.Length)];

            if (PhotonNetwork.inRoom)
            {
                // Enable messaging again because we're in the level
                PhotonNetwork.isMessageQueueRunning = true;
                PhotonNetwork.Instantiate(randomCharacter, pos, Quaternion.identity, 0);
            }
            else
            {
                GameObject player = (GameObject)Instantiate(Resources.Load("Timmy"));
                player.transform.position = pos;
            }
        }
    }
}
