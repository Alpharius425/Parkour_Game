using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    // Put on an empty game object and it will automatically update the spawn areas


    public static RespawnManager respawnManager; // allows other scripts to use this script and object

    [SerializeField] GameObject spawnPosition; // where we want to spawn
    [SerializeField] float timePenalty; // how much we want to add to the player's time if we want

    [SerializeField] float minYDistance; // how far the player is allowed to go down
    [SerializeField] GameObject player;

    private void Start()
    {
        respawnManager = this;
        Debug.Log(respawnManager.gameObject + " is the respawn manager");
    }

    private void Update()
    {
        if(player.transform.position.y < minYDistance)
        {
            Respawn();
        }
    }

    public void SetSpawnPosition(GameObject newPosition) // sets the respawn point to somewhere new
    {
        Debug.Log("Spawn point changed");
        spawnPosition = newPosition;
    }

    public void Respawn() // resets the player's position
    {
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = spawnPosition.transform.position;
        player.transform.rotation = spawnPosition.transform.rotation;
        player.GetComponent<CharacterController>().enabled = true;
        // (time += time penalty) PlaceHolder for time penalty
    }
}
