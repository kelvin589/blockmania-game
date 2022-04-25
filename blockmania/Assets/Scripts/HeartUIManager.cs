using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public struct HeartUIManagerState
{
    public int numberOfLives;

    public HeartUIManagerState(int numberOfLives)
    {
        this.numberOfLives = numberOfLives;
    }
}

public class HeartUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject livesObject;
    [SerializeField]
    private Sprite filledHeart;
    [SerializeField]
    private Sprite emptyHeart;
    [SerializeField]
    private DeathMenuController deathMenuController;

    private Image[] lives;
    private int numberOfLives;

    public static HeartUIManager Instance { get; private set; }

    void Awake()
    {
        lives = livesObject.GetComponentsInChildren<Image>();
        numberOfLives = lives.Length;

        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }
    
    public HeartUIManagerState ToRecord()
    {
        return new HeartUIManagerState(numberOfLives);
    }

    public void RemoveLife()
    {
        numberOfLives--;
        if (IsOutOfLives())
        {
            deathMenuController.ShowDeathMenu();
        }
        ScoreUIManager.Instance.AddScoreFor(ScoreValues.LostAHeart);
        UpdateLifeUI();
    }

    public void SetNumberOfLives(int newValue) {
        if (newValue < 0 || newValue > lives.Length) return;
        this.numberOfLives = newValue;
        UpdateLifeUI();
    }

    void UpdateLifeUI()
    {
        int temp = 0;
        foreach (Image life in lives)
        {
            if (temp < numberOfLives)
            {
                temp++;
                life.sprite = filledHeart;
            }
            else
            {
                life.sprite = emptyHeart;
            }
        }
    }

    bool IsOutOfLives()
    {
        return numberOfLives < 1;
    }
}
