using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager respawnManager; // allows other scripts to use this script and object

    [SerializeField] GameObject spawnPosition; // where we want to spawn
    [SerializeField] int timePenalty; // how much we want to add to the player's time if we want

    private void Start()
    {
        respawnManager = this;
        Debug.Log(respawnManager.gameObject + " is the respawn manager");
    }

    public void SetSpawnPosition(GameObject newPosition) // sets the respawn point to somewhere new
    {
        Debug.Log("Spawn point changed");
        spawnPosition = newPosition;
    }

    public void Respawn(GameObject player) // resets the player's position
    {
        player.transform.position = spawnPosition.transform.position;
        Debug.Log("Player respawned");
        // (time += time penalty) PlaceHolder for time penalty
    }
}
