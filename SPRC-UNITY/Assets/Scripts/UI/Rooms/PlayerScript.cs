using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private SpawnPointScript spawnPointScript;
    public SpawnPointScript SpawnPointScript { get { return spawnPointScript; } }

    public bool isReferenced = false;

    public void FirstInitialize(SpawnPointScript spawnPointScript)
    {
        isReferenced = true;
        Debug.LogError("player script init");
        this.spawnPointScript = spawnPointScript;
    }

    public SpawnPointScript GetSpawnPointScript()
    {
        return this.spawnPointScript;
    }
}
