using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateOrJoinRoomCanvas : MonoBehaviour
{
    [SerializeField]
    private CreateRoomMenu createRoomMenu;

    [SerializeField]
    private RoomListingMenu roomListingMenu;

    private RoomsCanvases roomsCanvases;
    
    public void FirstInitialized(RoomsCanvases canvases)
    {
        roomsCanvases = canvases;
        createRoomMenu.FirstInitialized(roomsCanvases);
        roomListingMenu.FirstInitialized(roomsCanvases);
    }
}
