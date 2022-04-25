using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using System.Xml;
using System.IO;
using System;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

[Serializable]
public struct GameState
{
    public HeartUIManagerState heartUIManagerState;
    public ScoreUIManagerState scoreUIManagerState;
    public KeyUIManagerState keyUIManagerState;
    public CollectibleState collectibleState;
    public PlayerState[] players;

    public GameState(HeartUIManagerState heartUIManagerState, ScoreUIManagerState scoreUIManagerState, KeyUIManagerState keyUIManagerState, CollectibleState collectibleState, PlayerState[] players)
    {
        this.heartUIManagerState = heartUIManagerState;
        this.scoreUIManagerState = scoreUIManagerState;
        this.keyUIManagerState = keyUIManagerState;
        this.collectibleState = collectibleState;
        this.players = players;
    }
}

public class SaveGameController : MonoBehaviour
{
    public static SaveGameController Instance { get; private set; }
    public bool shouldLoadSave = false;

    void Awake()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;

        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
        DontDestroyOnLoad(gameObject);
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // If we are loading a level and we should load the save
        if (Regex.IsMatch(scene.name, @"Level\d+") && shouldLoadSave)
        {
            shouldLoadSave = false;
            Load(SceneHelper.GetCurrentLevelName());
        }
    }

    private string Filename(string level)
    {
        return "Assets/Saves/save-" + level + ".xml";
    }

    public void Save(string level)
    {
        HeartUIManagerState heartUIManagerState = HeartUIManager.Instance.ToRecord();
        ScoreUIManagerState scoreUIManagerState = ScoreUIManager.Instance.ToRecord();
        KeyUIManagerState keyUIManagerState = KeyUIManager.Instance.ToRecord();

        PlayerProperties[] players = Resources.FindObjectsOfTypeAll<PlayerProperties>();
        PlayerState[] playerStates = new PlayerState[players.Length];
        for (int i = 0; i < players.Length; i++)
        {
            playerStates[i] = players[i].ToRecord();
        }

        CollectibleController[] collectibles = FindObjectsOfType<CollectibleController>();
        string[] collectibleIDs = new string[collectibles.Length];
        for (int i = 0; i < collectibles.Length; i++)
        {
            collectibleIDs[i] = collectibles[i].gameObject.name;
        }
        CollectibleState collectibleState = new CollectibleState(collectibleIDs);

        GameState state = new GameState(heartUIManagerState, scoreUIManagerState, keyUIManagerState, collectibleState, playerStates);

        XmlDocument xmlDocument = new XmlDocument();
        XmlSerializer serializer = new XmlSerializer(typeof(GameState));
        using (MemoryStream stream = new MemoryStream())
        {
            serializer.Serialize(stream, state);
            stream.Position = 0;
            xmlDocument.Load(stream);
            xmlDocument.Save(Filename(level));
        }
    }

    public void Load(string level)
    {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(Filename(level));
        string xmlString = xmlDocument.OuterXml;

        GameState state;
        using (StringReader read = new StringReader(xmlString))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(GameState));
            using (XmlReader reader = new XmlTextReader(read))
            {
                state = (GameState)serializer.Deserialize(reader);
            }
        }

        HeartUIManager.Instance.SetNumberOfLives(state.heartUIManagerState.numberOfLives);
        ScoreUIManager.Instance.SetCurrentScore(state.scoreUIManagerState.currentScore);

        // Destroy keys which have been collected
        KeyUIManager.Instance.SetCollectedKeys(state.keyUIManagerState.collectedKeys);
        KeyInteractive[] keys = FindObjectsOfType<KeyInteractive>();
        for (int i = 0; i < keys.Length; i++)
        {
            // If it's not found in the stored keys array,
            // it means that it's been collected
            string id = keys[i].gameObject.name;
            int pos = Array.IndexOf(state.keyUIManagerState.keyIDs, id);
            if (pos == -1) GameObject.Destroy(keys[i].gameObject);
        }

        // Destroy collectbles which have been collected
        CollectibleController[] collectibles = FindObjectsOfType<CollectibleController>();
        for (int i = 0; i < collectibles.Length; i++)
        {
            string id = collectibles[i].gameObject.name;
            int pos = Array.IndexOf(state.collectibleState.collectibleIDs, id);
            if (pos == -1) GameObject.Destroy(collectibles[i].gameObject);
        }

        // Restore player's (including enemies) position and rotation
        // Get all the PlayerProperties (users + enemies)
        PlayerProperties[] players = FindObjectsOfType<PlayerProperties>();
        // Then set the positions based on id
        for (int i = 0; i < players.Length; i++)
        {
            // Loop through all the stored players to find the right one
            for (int j = 0; j < state.players.Length; j++)
            {
                string id = players[i].id;
                string storedID = state.players[j].id;
                if (id.Equals(storedID))
                {
                    GameObject playerObject = players[i].gameObject;
                    // https://answers.unity.com/questions/1614287/teleporting-character-issue-with-transformposition.html
                    // In-built FPSController has CharacterController which has its own internal sense of position and velocity
                    CharacterController characterController = playerObject.GetComponent<CharacterController>();
                    if (characterController != null) characterController.enabled = false;
                    playerObject.transform.position = state.players[j].position;
                    playerObject.transform.rotation = state.players[j].rotation;
                    if (characterController != null) characterController.enabled = true;
                }
            }
        }

    }
}
