using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

public class RoomListing : MonoBehaviour
{
    [SerializeField]
    private Text text;

    private RoomsCanvases roomsCanvases;

    public void FirstInitialized(RoomsCanvases canvases)
    {
        roomsCanvases = canvases;
    }

    public RoomInfo RoomInfo { get; private set; }

    public void SetRoomInfo(RoomInfo roomInfo)
    {
        RoomInfo = roomInfo;
        text.text = roomInfo.Name + " (" + roomInfo.PlayerCount + "/" + roomInfo.MaxPlayers + ")";
    }

    public void OnClick_Button()
    {
        // Set username
        //GameObject userNameGameObject = roomsCanvases
        //    .CreateOrJoinRoomCanvas
        //    .transform.Find("CreateRoomMenu")
        //    .transform.Find("UserNameInput")
        //    .transform.Find("Text").gameObject;

        //Text userName = null;
        //if (userNameGameObject != null) userName = userNameGameObject.GetComponent<Text>();
        //if (userName != null && userName.text.Trim().Length != 0) PhotonNetwork.NickName = userName.text;
        PhotonNetwork.JoinRoom(RoomInfo.Name);
    }

}