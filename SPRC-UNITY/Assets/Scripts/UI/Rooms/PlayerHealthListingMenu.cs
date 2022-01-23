using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using ExitGames.Client.Photon;
using System;
using System.Collections;

public class PlayerHealthListingMenu : MonoBehaviourPun
{
    public Transform content;

    public GameObject playerHealth;

    private void Awake()
    {
        GetCurrentRoomPlayers();
    }

    public void GetCurrentRoomPlayers()
    {
        if (!PhotonNetwork.IsConnected) return;
        if (PhotonNetwork.CurrentRoom == null) return;
        if (PhotonNetwork.CurrentRoom.Players == null) return;

        // Destroy all player listings
        foreach (Transform child in content.transform) Destroy(child.gameObject);

        // Generate player listings
        foreach (KeyValuePair<int, Player> playerInfo in PhotonNetwork.CurrentRoom.Players)
        {
            GameObject playerName = playerHealth.transform.Find("Text").gameObject;
            if (playerName == null) continue;

            Text playerNameText = playerName.GetComponent<Text>();
            if (playerNameText == null) continue;

            // Read custom props
            ExitGames.Client.Photon.Hashtable customProperties = playerInfo.Value.CustomProperties;
            int health = 100;
            if (customProperties.ContainsKey("health")) health = (int)customProperties["health"];
            else Debug.Log("not set");

            playerNameText.text = playerInfo.Value.NickName + ": " + health.ToString();

            Instantiate(playerHealth, content);
        }
    }
}
