using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRun : MonoBehaviour
{
    [SerializeField] Transform StartingPoint;//Place any object with a transform where you'd like and connect it to this variable to set a starting position for a wall run
    [SerializeField] Transform EndingPoint;//Place any object with a transform where you'd like and connect it to this variable to set a ending position for a wall run
    [SerializeField] PlayerController Controller;
    [SerializeField] PlayerMovementUpdated MovementControl;
    [SerializeField] bool allowWallRun;
    [SerializeField] float WallRunSpeed = 10;
    [SerializeField] float savedGravity;
    [SerializeField] bool Entered;//The player has entered into the wallrun and will soon exit if true
    Transform defaultStart;//The defaultStart has the same value at the original StartingPoint and should never change //Note: In theory, since the script automatically switches  the start and end goals as needed, the default starting positon really matter from a logic perspective and not a design one.
    Transform defaultEnd;//The defaultEnd has the same value at the original StartingPoint and should never change

    // Start is called before the first frame update
    void Start()
    {
        defaultStart = StartingPoint;
        defaultEnd = EndingPoint;
        savedGravity = MovementControl.gravity;
    }

    // Update is called once per frame
    void Update()
    {

        //If wall running is allowed then start moving the player in the DIRECTION between the start and end points. This means that the player technically doesn't need to be close to the start or end points, this just moves them in the direction between the start and end.
        if (allowWallRun)
        {
            Controller.myMovement.MoveVelocity((EndingPoint.position - StartingPoint.position).normalized * WallRunSpeed);
        }
    }


    //On entering a trigger, wall running is allowed and gravity is set to zero to prevent falling. Velocity is also reset to prevent the player from drifting off of their last input.
    void OnTriggerEnter(Collider collider)
    {
       if (collider.gameObject.tag == "Player")//we only effect the player
       {

            if (Vector3.Distance(Controller.transform.position, defaultStart.position) < Vector3.Distance(Controller.transform.position, defaultEnd.position))
            {
                //player is closer to the staring point so we make sure that the starting and ending point are set to their defaults
                StartingPoint = defaultStart;
                EndingPoint = defaultEnd;
            }
            else
            {
                //otherwise we switch them
                StartingPoint = defaultEnd;
                EndingPoint = defaultStart;
            }

            if ((Controller.sprintHeld) && (Controller.currentState == State.Jumping) )//the player must be jumping and must be sprinting
            {
                allowWallRun = true;
                MovementControl.gravity = 0;
                MovementControl.ResetVelocity();
                Entered = true;
            }
        }
    }

    //reverses the changes made by entering
    private void OnTriggerExit(Collider collider)
    {
        if ((collider.gameObject.tag) == "Player" && (Entered == true))//the player must have entered the wall run
        {
            allowWallRun = false;
            MovementControl.gravity = savedGravity;
            Controller.myMovement.MoveVelocity((Controller.transform.forward) * 5);//teleports the player forward just a little bit so they don't get sucked back in
        }
    }
}

