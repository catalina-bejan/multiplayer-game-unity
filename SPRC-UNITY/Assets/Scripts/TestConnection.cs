using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class TestConnection : MonoBehaviourPunCallbacks
{
    void Start()
    {
        Debug.Log("Connecting to server...");
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.NickName = MasterManager.GameSettings.NickName;
        // Locks users to version
        PhotonNetwork.GameVersion = "0.0.1";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected!");
        Debug.Log(PhotonNetwork.LocalPlayer.NickName);

        if (PhotonNetwork.InLobby) return;

        PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected from server: " + cause.ToString());
    }

}
