using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using System.Collections.Generic;

public class PlayerListingMenu : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Transform content;

    [SerializeField]
    private PlayerListing playerListing;

    private List<PlayerListing> playerListingList = new List<PlayerListing>();

    private RoomsCanvases roomsCanvases;

    private void Awake()
    {
        GetCurrentRoomPlayers();
    }

    public void FirstInitialized(RoomsCanvases canvases)
    {
        roomsCanvases = canvases;
    }

    private void GetCurrentRoomPlayers()
    {
        if (!PhotonNetwork.IsConnected) return;
        if (PhotonNetwork.CurrentRoom == null) return;
        if (PhotonNetwork.CurrentRoom.Players == null) return;

        foreach(KeyValuePair<int, Player> playerInfo in PhotonNetwork.CurrentRoom.Players)
        {
            AddPlayerListing(playerInfo.Value);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddPlayerListing(newPlayer);
    }

    private void AddPlayerListing(Player player)
    {
        PlayerListing listing = Instantiate(playerListing, content);
        if (listing != null)
        {
            listing.SetPlayerInfo(player);
            playerListingList.Add(listing);
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        int index = playerListingList.FindIndex(x => x.player == otherPlayer);
        if (index != -1)
        {
            Destroy(playerListingList[index].gameObject);
            playerListingList.RemoveAt(index);
        }
    }

    public override void OnLeftRoom()
    {
        content.DestroyChildern();
    }

    public void OnClick_StartGame()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible= false;
        PhotonNetwork.LoadLevel("Game");
    }
}
