using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnCheckPoint : MonoBehaviour
{
    // put on an empty gameobject with a trigger collider
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player") //if the player hits the collider
        {
            if(RespawnManager.respawnManager.spawnPosition != gameObject)
            {
                RespawnManager.respawnManager.spawnPosition.SetActive(false);
                RespawnManager.respawnManager.SetSpawnPosition(gameObject);
            }
        }
    }
}
