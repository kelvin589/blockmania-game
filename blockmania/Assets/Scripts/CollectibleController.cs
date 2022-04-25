using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct CollectibleState
{
    // IDs of collectibles which have not been collected
    public string[] collectibleIDs;

    public CollectibleState(string[] collectibleIDs)
    {
        this.collectibleIDs = collectibleIDs;
    }
}

public class CollectibleController : Photon.MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // RPC's only work in rooms
            if (PhotonNetwork.inRoom)
            {
                photonView.RPC("OnTriggerEnterRPC", PhotonTargets.AllBuffered);
            }
            else
            {
                OnTriggerEnterRPC();
            }
        }
    }

    [PunRPC]
    void OnTriggerEnterRPC()
    {
        ScoreUIManager.Instance.AddScoreFor(ScoreValues.Collectible);
        Destroy(gameObject);
    }
}
