using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn_Detection : MonoBehaviour
{
    [SerializeField] GameObject player;
    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject == player) //if the player hits the collider
        {
            RespawnManager.respawnManager.Respawn(col.gameObject);
            Debug.Log(RespawnManager.respawnManager.gameObject + " is the respawn manager");
        }
    }
}
