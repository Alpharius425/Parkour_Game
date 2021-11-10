using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    Idle, Walking, Running, Crouching, Jumping, Climbing, Wallrunning, Vaulting, Sliding
}
public class PlayerController : MonoBehaviour
{
    // script controls how the player moves and references the player input controller and the player movement scripts

    public State currentState;
    public PlayerMovementUpdated myMovement;
    public CameraControl myCamera;
    public PlayerInputDetector myInput;

    public bool sprintHeld = false;

    public bool grounded; // are we on the ground or not
    [SerializeField] float downDetection; // how far down we want to check for the ground
    RaycastHit hit; // saves raycast hits

    public float detectionRange; // determines how far away we look when we try to detect obstacles
    Vector3[] detectionDirections;
    public float angleChange; // affects how much our angle shifts while wall running
    public bool angleChanged = false;
    // bools for what wall we are on
    [SerializeField] bool onRightWall = false;
    [SerializeField] bool onLeftWall = false;

    // Start is called before the first frame update
    void Start()
    {
        UpdateState(State.Idle);

        detectionDirections = new Vector3[] // sets up our parkour detection rays
        {
            Vector3.right,
            Vector3.left,
            Vector3.forward
        };
    }

    // Update is called once per frame
    void Update()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out hit, downDetection)) // checks if the player is on the ground or not
        {
            grounded = true;
            Debug.DrawRay(transform.position, Vector3.down, Color.green);
        }
        else
        {
            grounded = false;
            Debug.DrawRay(transform.position, Vector3.down, Color.red);
        }

        if(grounded && currentState == State.Jumping) // updates states when we hit the ground
        {
            if(sprintHeld)
            {
                UpdateState(State.Running);
            }
            else if(myMovement.movement == Vector3.zero)
            {
                UpdateState(State.Idle);
            }
            else
            {
                UpdateState(State.Walking);
            }
        }

        for (int i = 0; i < detectionDirections.Length; i++) // a for loop for shooting raycast
        {
            Vector3 direction = transform.TransformDirection(detectionDirections[i]); // sets the current vector we'll shoot the ray from
            if(Physics.Raycast(transform.position, direction, out hit, detectionRange)) // shoots the ray we have selected now
            {

                if(hit.collider.gameObject != gameObject)
                {
                    if (detectionDirections[i] == Vector3.forward) // if its the front facing ray we start climbing
                    {
                        UpdateState(State.Climbing);
                    }
                    else
                    {
                        UpdateState(State.Wallrunning);

                        if (detectionDirections[i] == Vector3.right && onLeftWall != true) // checks which wall we should be on and prevents us from being attached to both
                        {
                            onRightWall = true;

                            if (!angleChanged) // changes the camera angle
                            {
                                gameObject.transform.rotation = hit.collider.gameObject.transform.rotation;
                                myCamera.ChangeAngle(-angleChange);
                                angleChanged = true;
                            }

                        }
                        else if (onRightWall != true)
                        {
                            onLeftWall = true;

                            if (!angleChanged) // changes the camera angle
                            {
                                gameObject.transform.rotation = hit.collider.gameObject.transform.rotation;
                                myCamera.ChangeAngle(angleChange);
                                angleChanged = true;
                            }
                        }
                    }
                    Debug.Log("We detect" + hit.collider.name);
                    Debug.DrawRay(transform.position, direction, Color.green);
                }
            }
            else
            {
                // tells us we aren't attached to any walls
                onLeftWall = false;
                onRightWall = false;
                Debug.DrawRay(transform.position, direction, Color.red);

                if(angleChanged) // changes the camera angle back to normal
                {
                    myCamera.ResetAngle();
                    angleChanged = false;
                }

                if(currentState == State.Climbing)
                {
                    CheckMove();
                }
            }
        }


    }

    private void FixedUpdate()
    {
        if(currentState == State.Climbing || currentState == State.Running) // while we are running or climbing we're going to check if we can vault
        {
            Debug.Log("Check vaulting"); // currently causes editor to crash
            VaultCheck();
        }
    }

    public void UpdateState(State newState) // updates our state
    {
        currentState = newState;

        Debug.Log(currentState);
    }

    public void CheckJump() // checks if we can jump
    {
        if(grounded && currentState != State.Jumping)
        {
            myMovement.Jump();
        }
    }

    public void CheckCrouch() // checks if we can crouch and then checks if we should slide
    {
        if(grounded)
        {
            switch (currentState)
            {
                case State.Walking:
                    myMovement.Crouch();
                    break;

                case State.Idle:
                    myMovement.Crouch();
                    break;

                case State.Running:
                    myMovement.Slide();
                    break;

                case State.Sliding:
                    myMovement.Crouch();
                    break;

                case State.Crouching:
                    myMovement.UnCrouch();
                    break;
            }

        }
    }

    public void CheckSprint()
    {
        if(currentState == State.Running) // if we're already running and want to stop
        {
            Debug.Log("Stop running pls"); // potentially causing a crash
            CheckMove();
        }
        else // if we aren't running
        {
            if (myInput.canInput && (currentState == State.Walking || currentState == State.Crouching))
            {
                UpdateState(State.Running);
            }
        }
    }

    public void CheckMove()
    {
        if (myInput.movementInput != Vector2.zero) // checks if we are still moving
        {
            if(sprintHeld)
            {
                UpdateState(State.Running);
            }
            else
            {
                UpdateState(State.Walking);
            }
        }
        else // if we aren't moving
        {
            UpdateState(State.Idle);
        }
    }

    public void VaultCheck()
    {
        Vector3 vaultCheck = transform.position;
        RaycastHit vaultHit;
        if(Physics.Raycast(vaultCheck, transform.forward, out vaultHit, detectionRange)) // checks if theres anything infront of the player
        {
            vaultCheck.y += 1f;
            if(!Physics.Raycast(vaultCheck, transform.forward, out vaultHit, detectionRange)) // we scan above the player and check if theres nothing
            {
                vaultCheck += transform.forward;

                if(Physics.Raycast(vaultCheck, Vector3.down, out vaultHit, detectionRange)) // if we scan forward and down from our player's head
                {
                    UpdateState(State.Vaulting);
                    Debug.Log("We should vault");
                }
            }
        }
    }
}
