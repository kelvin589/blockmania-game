using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Adapted the script from https://assetstore.unity.com/packages/3d/environments/block-world-68107
// Using LeverManager for the Level instead of AniActivate (of original asset)
public class LeverManager : InteractiveObjectBase
{
    private bool isActive;
    private Animator animator;
    private int isActiveHashId;
    [SerializeField]
    private Material activeLeverMaterial;
    [SerializeField]
    private Material leverMaterial;
    [SerializeField]
    private Renderer leverPipeRenderer;

    public bool IsActive() => isActive;

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        isActiveHashId = Animator.StringToHash("isActive");
        isActive = false;
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
    public void OnInteractionRPC()
    {
        Debug.Log("Trigger lever");
        isActive = !isActive;
        animator.SetBool(isActiveHashId, isActive);
        // Lever texture was updated to make part of it green to indicate selected
        if (isActive)
        {
            leverPipeRenderer.materials = new Material[] { activeLeverMaterial };
        }
        else
        {
            leverPipeRenderer.materials = new Material[] { leverMaterial };
        }
    }
}
