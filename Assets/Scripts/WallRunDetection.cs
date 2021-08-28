using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunDetection : MonoBehaviour
{
    //

    [SerializeField] GameObject myPlayer; // lets us affect the player object
    [SerializeField] PlayerMovement myPlayerMovement; // lets us access the player script
    [SerializeField] LayerMask detectionLayers; // tells us what layers to look for


    // Start is called before the first frame update
    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.layer != detectionLayers && col.gameObject != myPlayer) // checks if the object is something we can detect
        {
            if (myPlayerMovement.wallRunning == false) // stops us from affecting things if we are already running
            {
                Debug.Log("I am wall running on " + col.gameObject);
                myPlayer.transform.SetParent(col.gameObject.transform); // parents the player to the object we are wall running on
                myPlayerMovement.wallRunning = true; // sets wall running to true
            }
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if(col.gameObject == myPlayer.transform.parent) // if the object we detect leaving the collider is the parented object
        {
            Deactivte();
        }
    }

    public void Deactivte()
    {
        myPlayer.transform.SetParent(null); // unparents the player
        myPlayerMovement.wallRunning = false; // sets wall running to false
        myPlayerMovement.curWallRunTime = myPlayerMovement.maxWallRunTime; // resets the time we can wallrun
        gameObject.SetActive(false);
    }
}
