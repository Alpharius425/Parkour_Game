using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaultingAndClimbingDetection : MonoBehaviour
{
    // put on a empty game object parented to the player. Needs a trigger collider from about where the hips are to the top of the head as well as a rigidbody

    [SerializeField] PlayerMovement player; // allows us to tell the player we hit something
    [SerializeField] LayerMask detectionLayers; // determines what we can detect
     
    void OnTriggerStay(Collider col) // plays if we detect something
    {
        GameObject obstacle = col.gameObject;

        if(obstacle.layer != detectionLayers) // checks if the thing we hit is on the right layers
        {
            player.detectsSomething = true;
            //Debug.Log("I detected an obstacle " + obstacle.name);
            player.VaultDetection();
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if(col.gameObject.layer != detectionLayers)
        {
            player.detectsSomething = false;
        }
    }
}
