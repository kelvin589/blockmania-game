using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInteractive : InteractiveObjectBase
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnInteraction()
    {
        // RPC's only work in rooms
        if (PhotonNetwork.inRoom)
        {
            photonView.RPC("OnInteractionRPC", PhotonTargets.AllBuffered);
        }
        else
        {
            OnInteractionRPC();
        }
    }

    [PunRPC]
    void OnInteractionRPC()
    {
        Debug.Log("Going to collect a Key");
        KeyUIManager.Instance.CollectKey();
        ScoreUIManager.Instance.AddScoreFor(ScoreValues.Key);
        GameObject.Destroy(gameObject);
    }
}
