using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpTo : MonoBehaviour
{
    // script we put on objects like ziplines. once the player attaches to the object they will then lerp to the endpoint

    [SerializeField] GameObject endPoint; // an empty game object that lets us designate where the player should end up
    [SerializeField] Vector3 startPoint; // saves the position of the player when we start the lerp
    [SerializeField] float journeyDistance; // saves the distance between our start and end points
    [SerializeField] float distanceCovered; // how far we've gone in the journey

    [SerializeField] float startTime; // saves reference for when we start moving
    [SerializeField] float speed; // how fast we are going to let the player move

    [SerializeField] bool attached = false; // tells us whether the player is attached to the object
    [SerializeField] GameObject player = null;
    [SerializeField] PlayerController controller;

    [SerializeField] bool wallrunning = false; // temporary solution to wall running. will let us be able to jump from walls if the player wants to

    // Update is called once per frame
    void Update()
    {
        if(attached && controller.attachedObject == this) // looks to see if we have the player attached to the object
        {
            distanceCovered = (Time.time - startTime) * speed;
            float fractionOfJourney = distanceCovered / journeyDistance; // saves how much of the distance we've already passed
            player.transform.position = Vector3.Lerp(startPoint, endPoint.transform.position, fractionOfJourney); // begins moving the player from the starting point to the endpoint
            if (player.transform.position == endPoint.transform.position) // once the player reaches the end point
            {
                stop();
            }

            if(wallrunning)
            {
                player.GetComponent<PlayerController>().UpdateState(State.Wallrunning);
            }
            else
            {
                player.GetComponent<PlayerController>().UpdateState(State.noMove);
            }
        }
    }

    private void Start()
    {
        controller = player.GetComponent<PlayerController>();
    }

    public void stop()
    {
        attached = false; // stops the object from moving us
        
        if(wallrunning)
        {
            controller.CheckMove();
        }
        else
        {
            player.GetComponent<PlayerInputDetector>().canInput = true;
        }

        player.GetComponent<CameraControl>().RotatePlayer();
        controller.ResetWallJumpTimer();
        controller.attachedObject = null;
        Debug.Log("player is no longer attached");
    }

    public void Attach()
    {
        if(wallrunning)
        {
            //player.GetComponent<PlayerController>().UpdateState(State.Wallrunning);
        }
        else
        {
            player.GetComponent<PlayerInputDetector>().canInput = false;
        }
        player.GetComponent<PlayerController>().attachedObject = this;
        startTime = Time.time;
        player.transform.LookAt(endPoint.transform.position); // makes our player look at the endpoint
        //Quaternion rotation = player.transform.rotation;
        //rotation.z = 0f;
        //player.transform.rotation = rotation;
        startPoint = player.transform.position; // sets the starting point of the movement
        journeyDistance = Vector3.Distance(startPoint, endPoint.transform.position);
        //player.transform.SetParent(gameObject.transform); // parents the player to the object. useful if we have a moving object we want to attach to
        attached = true; // tells us we are attached to the object
        Debug.Log("player has attached");
    }
}
