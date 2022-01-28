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

    public bool sprintHeld = false;
    public bool crouchHeld = false;

    public bool grounded; // are we on the ground or not
    [SerializeField] float downDetection; // how far down we want to check for the ground
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

    public float detectionRange; // determines how far away we look when we try to detect obstacles
    public float vaultDetectionRange;
    Vector3 vaultCheck;
    [SerializeField] LayerMask vaultLayers;

    Vector3[] detectionDirections;
    public float angleChange; // affects how much our angle shifts while wall running
    public bool angleChanged = false;
    // bools for what wall we are on
    [SerializeField] bool onRightWall = false;
    [SerializeField] bool onLeftWall = false;
    public LerpTo attachedObject = null;


    Vector3 topCheck;
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
        vaultCheck = Vector3.forward;
    }

    // Update is called once per frame
    void Update()
    {

        if(currentState != State.Vaulting && currentState != State.Wallrunning)
        {
            Debug.DrawRay(topCheck, Vector3.down);
            if (Physics.Raycast(transform.position, Vector3.down, out hit, downDetection)) // checks if the player is on the ground or not
            {
                grounded = true;
                Debug.DrawRay(transform.position, Vector3.down, Color.green);
            }
            else
            {
                grounded = false;
                Debug.DrawRay(transform.position, Vector3.down, Color.red);
            }
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

        /*
        if(currentState == State.Running || currentState == State.Climbing || currentState == State.Jumping)
        {
            for (int i = 0; i < detectionDirections.Length; i++) // a for loop for shooting raycast
            {
                Vector3 direction = transform.TransformDirection(detectionDirections[i]); // sets the current vector we'll shoot the ray from
                if (Physics.Raycast(transform.position, direction, out hit, detectionRange, climbLayers)) // shoots the ray we have selected now
                {
                    if (hit.collider.gameObject != gameObject)
                    {
                        if (detectionDirections[i] == Vector3.forward && curClimbTime > 0 && currentState != State.Vaulting) // if its the front facing ray we start climbing
                        {
                            UpdateState(State.Climbing);
                        }
                        else
                        {
                            if (hit.collider.gameObject.GetComponent<LerpTo>() && attachedObject == null && currentState != State.Wallrunning && currentState != State.noMove && timeUntilWallRun < 0) // checks if the player hits something with the lerp to script and isn't already parented to another
                            {

                                hit.collider.gameObject.GetComponent<LerpTo>().Attach();
                            }
                        }
                        //else commented out until we can get a working wall run system
                        //{
                        //    UpdateState(State.Wallrunning);

                        //    if (detectionDirections[i] == Vector3.right && onLeftWall != true) // checks which wall we should be on and prevents us from being attached to both
                        //    {
                        //        onRightWall = true;

                        //        if (!angleChanged) // changes the camera angle
                        //        {
                        //            gameObject.transform.rotation = hit.collider.gameObject.transform.rotation;
                        //            myCamera.ChangeAngle(-angleChange);
                        //            angleChanged = true;
                        //        }

                        //    }
                        //    else if (onRightWall != true)
                        //    {
                        //        onLeftWall = true;

                        //        if (!angleChanged) // changes the camera angle
                        //        {
                        //            gameObject.transform.rotation = hit.collider.gameObject.transform.rotation;
                        //            myCamera.ChangeAngle(angleChange);
                        //            angleChanged = true;
                        //        }
                        //    }
                        //}
                        Debug.DrawRay(transform.position, direction, Color.green);
                    }
                }
                else
                {
                    // tells us we aren't attached to any walls
                    onLeftWall = false;
                    onRightWall = false;
                    Debug.DrawRay(transform.position, direction, Color.red);

                    //if (angleChanged) // changes the camera angle back to normal
                    //{
                    //    myCamera.ResetAngle();
                    //    angleChanged = false;
                    //}

                    if (currentState == State.Climbing || currentState == State.Wallrunning)
                    {
                        //Debug.Log("here");
                        CheckMove();
                    }
                }
            }
        }
        */
    }

    private void FixedUpdate()
    {
        if(currentState == State.Climbing || currentState == State.Running || currentState == State.Jumping) // while we are running or climbing we're going to check if we can vault
        {
            Debug.Log("Check vaulting");
            VaultCheck();
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
                CheckMove();
            }
        }
    }

    public void CheckMove()
    {
            if (myInput.movementInput != Vector2.zero && currentState != State.Wallrunning) // checks if we are still moving
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
            else if (myInput.movementInput == Vector2.zero && crouchHeld == false) // if we aren't moving
            {
                UpdateState(State.Idle);
            }
        
    }

    public void VaultCheck()
    {
        RaycastHit vaultHit;
        Vector3 direction = transform.TransformDirection(vaultCheck);
        //topCheck = Vector3.zero;
        //topCheck.y += 0.5f;

        //Debug.Log(direction);
        Debug.DrawRay(transform.position, direction, Color.blue);

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
                        Debug.Log("Second hit" + vaultHit.point);
                        
                        //myMovement.Vault(vaultHit.point);
                    }
                }
            }
            else
            {
                Debug.Log("detect player");
            }
        }
    }
}
