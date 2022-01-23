using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomListingMenu : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Transform content;

    [SerializeField]
    private RoomListing roomListing;

    private List<RoomListing> roomListingList = new List<RoomListing>();

    private RoomsCanvases roomsCanvases;

    public void FirstInitialized(RoomsCanvases canvases)
    {
        roomsCanvases = canvases;
        roomListing.FirstInitialized(roomsCanvases);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            // Removed from room list
            if (info.RemovedFromList)
            {
                // Remove from my room listing list
                int index = roomListingList.FindIndex(x => x.RoomInfo.Name == info.Name);
                if (index != -1)
                {
                    Destroy(roomListingList[index].gameObject);
                    roomListingList.RemoveAt(index);
                }
            }
            else
            {
                // Added to rooms list
                int index = roomListingList.FindIndex(x => x.RoomInfo.Name == info.Name);
                if (index == -1)
                {
                    RoomListing newListing = Instantiate(roomListing, content);
                    if (roomListingList != null)
                    {
                        newListing.SetRoomInfo(info);
                        roomListingList.Add(newListing);
                    }
                }
            }

            Debug.Log("info: " + info);
        }
    }

    public override void OnJoinedRoom()
    {
        roomsCanvases.CurrentRoomCanvas.Show();
        content.DestroyChildern();
        roomListingList.Clear();
    }

}
