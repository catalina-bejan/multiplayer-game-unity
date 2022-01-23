using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentRoomCanvas : MonoBehaviour
{
    [SerializeField]
    private PlayerListingMenu playerListingMenu;

    [SerializeField]
    private LeaveRoomMenu leaveRoomMenu;

    private RoomsCanvases roomsCanvases;

    public void FirstInitialized(RoomsCanvases canvases)
    {
        roomsCanvases = canvases;
        playerListingMenu.FirstInitialized(canvases);
        leaveRoomMenu.FirstInitialized(canvases);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

}
