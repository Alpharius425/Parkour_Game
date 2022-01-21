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
            RespawnManager.respawnManager.SetSpawnPosition(gameObject);
            Debug.Log(RespawnManager.respawnManager.gameObject + " is the respawn manager");
        }
    }
}
