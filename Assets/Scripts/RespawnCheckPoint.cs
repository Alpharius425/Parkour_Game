using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnCheckPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player")) //if the player hits the collider
        {
            RespawnManager.respawnManager.SetSpawnPosition(gameObject);
            Debug.Log(RespawnManager.respawnManager.gameObject + " is the respawn manager");
        }
    }
}
