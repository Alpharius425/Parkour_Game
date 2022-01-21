using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn_Detection : MonoBehaviour
{
    // put on collider to respawn player
    
    private void OnTrigger(Collider col)
    {
        if(col.gameObject.tag == "Player") //if the player hits the collider
        {
            //RespawnManager.respawnManager.Respawn(col.gameObject);
            Debug.Log(RespawnManager.respawnManager.gameObject + " is the respawn manager");
        }
    }
}
