using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class Instantiation : MonoBehaviourPunCallbacks
{
    public void Awake()
    {
        var player = PhotonNetwork.Instantiate("Player", this.transform.position + new Vector3(Random.Range(-20, 20), 0, 0), this.transform.rotation, 0);

        // Show player mark
        player.transform.Find("PlayerMark").gameObject.SetActive(true);
        //GameObject playerName = player.transform.Find("PlayerName").gameObject;
        //playerName.GetComponent<TextMesh>().text = PhotonNetwork.NickName;

        //photonView.RPC("RPC_SetName", RpcTarget.All, PhotonNetwork.NickName);

        gameObject.GetComponent<SpawnPointScript>().FirstInitialized(player);
    }

    //[PunRPC]
    //private void RPC_SetName(object data)
    //{
    //    GameObject playerName = gameObject.transform.Find("PlayerName").gameObject;
    //    playerName.GetComponent<TextMesh>().text = data.ToString();
    //}

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        // Update UI
        GameObject playerHealthListingMenu = GameObject.Find("PlayerHealthListingMenu");
        PlayerHealthListingMenu script = playerHealthListingMenu.GetComponent<PlayerHealthListingMenu>();
        script.GetCurrentRoomPlayers();
    }
}