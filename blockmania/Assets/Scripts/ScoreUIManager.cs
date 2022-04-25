using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public struct ScoreUIManagerState
{
    public int currentScore;

    public ScoreUIManagerState(int currentScore)
    {
        this.currentScore = currentScore;
    }
}

public enum ScoreValues : int
{
    Key = 100,
    Heart = 50,
    Collectible = 10,
    LostAHeart = -100,
}

public class ScoreUIManager : Photon.MonoBehaviour
{
    [SerializeField]
    private Text score;
    private int currentScore;
    private const int scoreUpperLimit = 9999;
    private const int scoreLowerLimit = 0;

    public static ScoreUIManager Instance { get; private set; }

    void Awake()
    {
        currentScore = 0;

        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    // Update is called once per frame
    void Update()
    {
        // Pad score with 0's if it's less than 4 digits
        score.text = $"Score: {currentScore.ToString("0000")}";
    }

    public ScoreUIManagerState ToRecord()
    {
        return new ScoreUIManagerState(currentScore);
    }

    public void SetCurrentScore(int newValue)
    {
        // RPC's only work in rooms
        if (PhotonNetwork.inRoom)
        {
            photonView.RPC("SetCurrentScoreRPC", PhotonTargets.AllBuffered, newValue);
        }
        else
        {
            SetCurrentScoreRPC(newValue);
        }
    }

    [PunRPC]
    public void SetCurrentScoreRPC(int newValue)
    {
        // Lock score between 9999 and 0
        if (newValue > scoreUpperLimit)
        {
            currentScore = scoreUpperLimit;
        }
        else if (newValue < scoreLowerLimit)
        {
            currentScore = scoreLowerLimit;
        }
        else
        {
            currentScore = newValue;
        }
    }

    public void AddScoreFor(ScoreValues item)
    {
        SetCurrentScore(currentScore + (int)item);
    }

    public int GetCurrentScore()
    {
        return currentScore;
    }
}
