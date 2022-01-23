using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class MessageListingMenu : MonoBehaviourPun
{

    [SerializeField]
    private Transform content;

    [SerializeField]
    private MessageListing messageListing;

    [SerializeField]
    private InputField inputMessage;

    private const byte MESSAGE_EVENT = 1;

    private void Awake()
    {
        //GetCurrentRoomPlayers();
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
        if (obj.Code.Equals(MESSAGE_EVENT))
        {
            object[] data = (object[])obj.CustomData;
            string sender = (string)data[0];
            string message = (string)data[1];

            // Instantiate messageListing
            Debug.Log(message);
            Debug.Log(sender);
            MessageListing listing = Instantiate(messageListing, content);
            if (listing != null)
            {
                listing.SetMessageInfo(sender, message);
            }

        }
    }


    public void OnClick_SendMessage()
    {
        // Raise event
        string sender = PhotonNetwork.NickName;
        string message = inputMessage.text;
        object[] data = new object[] { sender, message };
        PhotonNetwork.RaiseEvent(MESSAGE_EVENT, data, RaiseEventOptions.Default, SendOptions.SendUnreliable);

        // Clear input text
        inputMessage.text = string.Empty;

        // Instantiate self message
        MessageListing listing = Instantiate(messageListing, content);
        if (listing != null)
        {
            listing.SetMessageInfo("You", message);
        }
    }
}