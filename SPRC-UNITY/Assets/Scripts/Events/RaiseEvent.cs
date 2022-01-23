using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaiseEvent : MonoBehaviourPun
{
    private SpriteRenderer spriteRenderer;
    private const byte COLOR_CHANGE_EVENT = 0;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
    }

    private void NetworkingClient_EventReceived(EventData obj)
    {
        // Handle event received
        if (obj.Code.Equals(COLOR_CHANGE_EVENT))
        {
            object[] data = (object[])obj.CustomData;
            float r = (float)data[0];
            float g = (float)data[1];
            float b = (float)data[2];
            spriteRenderer.color = new Color(r, g, b, 1f);
        }
    }

    void Update()
    {
        if (base.photonView.IsMine && Input.GetKeyDown(KeyCode.Space)) ChangeColor();
    }

    private void ChangeColor()
    {
        float r = Random.Range(0f, 1f);
        float g = Random.Range(0f, 1f);
        float b = Random.Range(0f, 1f);

        spriteRenderer.color = new Color(r, g, b, 1f);

        // Raise event
        object[] data = new object[] { r, g, b };
        PhotonNetwork.RaiseEvent(COLOR_CHANGE_EVENT, 
            data, 
            RaiseEventOptions.Default,
            SendOptions.SendUnreliable);
    }
}