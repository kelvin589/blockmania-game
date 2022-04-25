using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ConnectionManager : Photon.MonoBehaviour
{
    public Text connectionStatusText;
    public Text playerNameText;
    public Text currentRoomText;
    public Text isMasterClientText;

    public InputField roomToJoinInput;

    void Awake()
    {   
        // Connect to the main photon server
        if (!PhotonNetwork.connectedAndReady) PhotonNetwork.ConnectUsingSettings(Globals.VERSION);

        // Create and set a random  player name
        PhotonNetwork.playerName = "Player" + Random.Range(1000, 9999);
        playerNameText.text = "Player Name: " + PhotonNetwork.playerName;
        currentRoomText.text = "Room: (no room)";
        isMasterClientText.text = "Master Client: " + PhotonNetwork.isMasterClient;
    }

    void UpdateRoomInfo()
    {
        if (PhotonNetwork.room == null) // Not in a room
        {
            currentRoomText.text = "Room: (no room)";
        }
        else // In a room
        {
            currentRoomText.text = "Room: " + PhotonNetwork.room.Name + " (" + PhotonNetwork.room.PlayerCount + ")";
        }
        isMasterClientText.text = "Master Client: " + PhotonNetwork.isMasterClient;
    }

    // BUTTON HANDLERS

    public void ButtonHandlerCreateRoom()
    {
        if (PhotonNetwork.connectedAndReady && roomToJoinInput.text.Length > 0) // check there is a name entered before creating
        {
            PhotonNetwork.CreateRoom(roomToJoinInput.text);
        }
    }

    public void ButtonHandlerJoinRoom()
    {
        if (PhotonNetwork.connectedAndReady && roomToJoinInput.text.Length > 0) // check there is a name entered before joining
        {
            PhotonNetwork.JoinRoom(roomToJoinInput.text);
        }
    }

    public void ButtonHandlerLeaveRoom()
    {
        PhotonNetwork.isMessageQueueRunning = true;
        if (PhotonNetwork.connectedAndReady)
        {
            PhotonNetwork.LeaveRoom();
        }
    }

    // EVENT CALLBACKS

    void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster!");
        connectionStatusText.text = "Status: Connected";
        UpdateRoomInfo();
    }

    void OnFailedToConnectToPhoton()
    {
        Debug.Log("OnFailedToConnectToPhoton");
        connectionStatusText.text = "Status: Connection Failed";
        UpdateRoomInfo();
    }

    void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom: " + PhotonNetwork.room.Name);
        UpdateRoomInfo();
    }

    void OnPhotonCreateRoomFailed()
    {
        Debug.Log("OnPhotonCreateRoomFailed");
        UpdateRoomInfo();
    }

    void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom: " + PhotonNetwork.room.Name);
        UpdateRoomInfo();
        // Pause message queue. Another player may already be in a level.
        // Receive messages only when loaded a level.
        PhotonNetwork.isMessageQueueRunning = false;
    }

    void OnPhotonPlayerConnected()
    {
        Debug.Log("OnPhotonPlayerConnected");
        UpdateRoomInfo();
    }

    void OnPhotonPlayerDisconnected()
    {
        Debug.Log("OnPhotonPlayerDisconnected");
        UpdateRoomInfo();
    }

    void OnPhotonJoinRoomFailed()
    {
        Debug.Log("OnPhotonJoinRoomFailed");
        connectionStatusText.text = "Status: Join Room Failed!";
        UpdateRoomInfo();
    }

    void OnLeftRoom()
    {
        Debug.Log("OnLeftRoom");
        connectionStatusText.text = "Status: Left Room!";
        UpdateRoomInfo();
    }

}