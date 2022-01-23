using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;

public class PlayerListing : MonoBehaviour
{
    [SerializeField]
    private Text text;

    public Player player { get; private set; }

    public void SetPlayerInfo(Player player)
    {
        this.player = player;
        text.text = player.NickName;
    }

}
