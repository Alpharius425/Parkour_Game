using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    Idle, Walking, Running, Crouching, Jumping, Climbing, Wallrunning, Vaulting, Sliding, noMove
}
public class PlayerController : MonoBehaviour
{
    // script controls how the player moves and references the player input controller and the player movement scripts

    public State currentState;
    public PlayerMovementUpdated myMovement;
    public CameraControl myCamera;
    public PlayerInputDetector myInput;
    [SerializeField] CharacterController myController;

    public bool sprintHeld = false;
    public bool crouchHeld = false;

    public bool grounded; // are we on the ground or not
    [SerializeField] float timeUntilGroundCheck = 0f;
    [SerializeField] float jumpLandTime = 0.3f;
    RaycastHit hit; // saves raycast hits

    // wall run detection delays so we can't chain wall runs or get stuck when jumping
    [SerializeField] float maxTimeToWallRun;
    [SerializeField] float timeUntilWallRun;

    // climb limits so we don't infinitely climb and have to play that one song from metal gear
    [SerializeField] float maxClimbTime;
    public float curClimbTime;
    [SerializeField] LayerMask climbLayers;
    [SerializeField] LayerMask wallRunLayers;

    public float climbDetectionRange; // determines how far away we look when we try to detect obstacles
    public float vaultDetectionRange;
    public float wallRunDetectionRange;
    Vector3 vaultCheck;
    [SerializeField] LayerMask vaultLayers;

    Vector3[] detectionDirections;
    public float angleChange; // affects how much our angle shifts while wall running
    public bool angleChanged = false;
    // bools for what wall we are on
    public bool onRightWall = false;
    public bool onLeftWall = false;
    public LerpTo attachedObject = null;


    Vector3 topCheck;
    // Start is called before the first frame update
    void Start()
    {
        UpdateState(State.Idle);
        detectionDirections = new Vector3[] // sets up our parkour detection rays
        {
            Vector3.right,
            Vector3.left
        };

        vaultCheck = Vector3.forward;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState != State.Vaulting && currentState != State.Wallrunning)
        {
            grounded = myController.isGrounded;
        }

        if(grounded && currentState != State.Climbing && currentState != State.Wallrunning) // updates states when we hit the ground
        {

            if(currentState == State.Jumping && timeUntilGroundCheck >= 1)
            {
                ResetWallJumpTimer();
                myMovement.myAnimator.SetBool("Jumping", false);
                myMovement.myAnimator.SetBool("JumpLand", true);
                CheckMove();
                timeUntilGroundCheck = 0;
            }

            if(myMovement.myAnimator.GetBool("JumpLand") == true)
            {
                jumpLandTime -= Time.deltaTime;
                if(jumpLandTime <= 0)
                {
                    myMovement.myAnimator.SetBool("JumpLand", false);
                    jumpLandTime = 0.3f;
                }
            }

            if(curClimbTime < maxClimbTime && currentState != State.Climbing)
            {
                ResetClimbTime();
            }
        }
        else
        {
            timeUntilGroundCheck += Time.deltaTime;
        }

        if(currentState != State.Wallrunning && timeUntilWallRun > 0)
        {
            timeUntilWallRun -= Time.deltaTime;
        }

        if(currentState == State.Climbing)
        {
            curClimbTime -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (currentState == State.Climbing || currentState == State.Running || currentState == State.Jumping && currentState != State.Wallrunning)
        {

            // Climbing
            if(Physics.Raycast(transform.position, transform.forward, out hit, climbDetectionRange, climbLayers))
            {
                if(curClimbTime > 0)
                {
                    if (hit.collider.gameObject != gameObject)
                    {
                        if (currentState != State.Vaulting)
                        {
                            UpdateState(State.Climbing);
                        }
                        else
                        {
                            CheckMove();
                        }
                    }
                }
                else
                {
                    CheckMove();
                }
            }
            else
            {
                CheckMove();
            }


            // vaulting
            //Debug.Log("Check vaulting");
            VaultCheck();
        }

        if (currentState == State.Climbing)
        {
            Debug.DrawRay(transform.position, transform.forward * climbDetectionRange, Color.green);
        }
        else if(currentState == State.Walking || currentState == State.Running)
        {
            Debug.DrawRay(transform.position, transform.forward * climbDetectionRange, Color.red);
        }
    }

    public void CheckAttach()
    {
        if (Physics.Raycast(transform.position, Vector3.right * wallRunDetectionRange, out hit, wallRunLayers))
        {
            onRightWall = true;
            onLeftWall = false;
            myCamera.ChangeAngle(-angleChange);
        }
        else if (Physics.Raycast(transform.position, Vector3.left * wallRunDetectionRange, out hit, wallRunLayers))
        {
            onRightWall = false;
            onLeftWall = true;
            myCamera.ChangeAngle(angleChange);
        }
    }

    public void ResetWallJumpTimer()
    {
        timeUntilWallRun = maxTimeToWallRun;
    }

    public void ResetClimbTime()
    {
        curClimbTime = maxClimbTime;
    }

    public void UpdateState(State newState) // updates our state
    {
        currentState = newState;

        //Debug.Log(currentState);
    }

    public void CheckJump() // checks if we can jump
    {
        if((grounded || currentState == State.Wallrunning || currentState == State.Climbing)&& currentState != State.Jumping)
        {
            myMovement.Jump();
        }
        else
        {
            Debug.Log("Cant jump");
        }
    }

    public void CheckCrouch() // checks if we can crouch and then checks if we should slide
    {
        if(grounded)
        {
            if(crouchHeld)
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

                        myMovement.CancelSlide();
                        break;

                    case State.Crouching:
                        myMovement.UnCrouch();
                        break;
                }
            }
            else
            {
                myMovement.UnCrouch();
            }
        }
    }

    public void CheckSprint()
    {
        if(currentState == State.Running) // if we're already running and want to stop
        {
            CheckMove();
        }
        else // if we aren't running
        {
            if (myInput.canInput && (currentState == State.Walking || currentState == State.Crouching && currentState != State.Sliding))
            {
                if(currentState == State.Crouching || crouchHeld)
                {
                    myMovement.UnCrouch();
                }

                CheckMove();
            }
        }
    }

    public void CheckMove()
    {
            if (myInput.movementInput != Vector2.zero) // checks if we are still moving
            {
                if (sprintHeld)
                {
                    UpdateState(State.Running);
                }
                else if (crouchHeld)
                {
                    if (sprintHeld)
                    {
                        UpdateState(State.Running);
                    }
                    else
                    {
                        UpdateState(State.Crouching);
                    }
                }
                else
                {
                    UpdateState(State.Walking);
                }
            }
            else // if we aren't moving
            {
                if(crouchHeld == false)
                {
                    UpdateState(State.Idle);
                }
                else
                {
                    UpdateState(State.Crouching);
                }
                
            }
        
    }

    public void VaultCheck()
    {
        RaycastHit vaultHit;
        Vector3 direction = transform.TransformDirection(vaultCheck);
        //topCheck = Vector3.zero;
        //topCheck.y += 0.5f;

        //Debug.Log(direction);
        //Debug.DrawRay(transform.position, direction, Color.blue);

        if (Physics.Raycast(transform.position, direction, out vaultHit, vaultDetectionRange, vaultLayers)) // checks if theres anything infront of the player
        {
            if(vaultHit.collider.gameObject != gameObject)
            {
                direction.y += 0.2f;
                //Debug.Log("initial hit" + vaultHit.point);
                if (!Physics.Raycast(transform.position, direction, out vaultHit, vaultDetectionRange)) // we scan above the player and check if theres nothing
                {
                    topCheck = transform.position + transform.forward + Vector3.up;
                    if (Physics.Raycast(topCheck, Vector3.down, out vaultHit, vaultDetectionRange)) // if we scan forward and down from our player's head
                    {
                        //Debug.Log("Second hit" + vaultHit.point);
                        
                        myMovement.Vault(vaultHit.point);
                    }
                }
            }
            else
            {
                //Debug.Log("detect player");
            }
        }
    }
}
