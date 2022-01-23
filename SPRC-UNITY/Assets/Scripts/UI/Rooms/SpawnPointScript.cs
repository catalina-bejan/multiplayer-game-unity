using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointScript : MonoBehaviour
{
    [SerializeField]
    private GameOverMenu gameOverMenu;
    public GameOverMenu GameOverMenu { get { return gameOverMenu; } }

    public void FirstInitialized(GameObject player)
    {
        PlayerScript playerScript = player.GetComponent<PlayerScript>();
        playerScript.FirstInitialize(this);
    }
}
