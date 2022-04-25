using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public struct KeyUIManagerState
{
    public int collectedKeys;
    // IDs of keys which have not been collected
    public string[] keyIDs;

    public KeyUIManagerState(int collectedKeys, string[] keyIDs)
    {
        this.collectedKeys = collectedKeys;
        this.keyIDs = keyIDs;
    }
}

public class KeyUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject keysObject;
    [SerializeField]
    private Sprite filledKey;
    [SerializeField]
    private Sprite emptyKey;

    private Image[] keys;
    // Number of keys player has collected
    private int collectedKeys;
    private int numberOfKeys;

    public static KeyUIManager Instance { get; private set; }

    void Awake()
    {
        keys = keysObject.GetComponentsInChildren<Image>();
        numberOfKeys = keys.Length;
        // Start with 0 keys
        collectedKeys = 0;

        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public KeyUIManagerState ToRecord()
    {
        // Retrieves only keys that are active in the scene i.e. have not been collected
        KeyInteractive[] keys = FindObjectsOfType<KeyInteractive>();
        string[] keyIDs = new string[keys.Length];
        for (int i = 0; i < keys.Length; i++)
        {
            keyIDs[i] = keys[i].gameObject.name;
        }

        return new KeyUIManagerState(collectedKeys, keyIDs);
    }

    public void SetCollectedKeys(int newValue)
    {
        if (newValue < 0 || newValue > keys.Length) return;
        this.collectedKeys = newValue;
        UpdateKeyUI();
    }

    public void CollectKey()
    {
        if (!HasCollectedEnoughKeys())
        {
            Debug.Log("Collected a key");
            collectedKeys++;
            UpdateKeyUI();
        }
    }

    void UpdateKeyUI()
    {
        int temp = 0;
        foreach (Image key in keys)
        {
            if (temp < collectedKeys)
            {
                temp++;
                key.sprite = filledKey;
            }
            else
            {
                key.sprite = emptyKey;
            }
        }
    }

    public bool HasCollectedEnoughKeys()
    {
        return collectedKeys == keys.Length;
    }
}
