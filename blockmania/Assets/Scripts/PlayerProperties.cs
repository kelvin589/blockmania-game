using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct PlayerState
{
    public Vector3 position;
    public Quaternion rotation;
    public string id;

    public PlayerState(Vector3 position, Quaternion rotation, string id)
    {
        this.position = position;
        this.rotation = rotation;
        this.id = id;
    }
}

public class PlayerProperties : MonoBehaviour
{
    public string id;

    public void Awake() 
    {
        id = gameObject.name;
    }

    public PlayerState ToRecord()
    {
        return new PlayerState(transform.position, transform.rotation, gameObject.name);
    }
}
