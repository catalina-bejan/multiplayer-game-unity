using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChangeEvent : MonoBehaviour
{
    private PhotonView photonView;
    public string nickname;

    void Awake()
    {
        photonView = GetComponent<PhotonView>();
        if (photonView.IsMine)
        {
            float r = Random.Range(0f, 1f);
            float g = Random.Range(0f, 1f);
            float b = Random.Range(0f, 1f);

            // Raise event
            object[] data = new object[] { r, g, b };
            photonView.RPC("RPC_SendColor", RpcTarget.All, data);
        }
    }

    [PunRPC]
    private void RPC_SendColor(object[] data)
    {
        float r = (float)data[0];
        float g = (float)data[1];
        float b = (float)data[2];
        Color color = new Color(r, g, b);
        gameObject.GetComponent<SpriteRenderer>().color = color;
    }
}
