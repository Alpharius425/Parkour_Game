using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementUpdated : MonoBehaviour
{
    // script controls the player's movement

    public CharacterController controller;
    public PlayerController myController;
    public CameraControl myCamera;
    public PlayerInputDetector myInput;
    public Animator myAnimator;

    public Vector3 movement = Vector3.zero; // the character's actual movement
    [SerializeField] float gravity;
    [SerializeField] bool airControlsOn = true;

    // speed info
    [SerializeField] float walkSpeed = 4f;
    [SerializeField] float runSpeed = 8f;
    [SerializeField] float crouchSpeed = 2f;
    [SerializeField] float climbSpeed = 4f;
    //[SerializeField] float wallRunSpeed = 4f;
    [SerializeField] float slideSpeed = 8f;
    [SerializeField] float airSpeed;
    [SerializeField] private float actualSpeed; // the actual speed of the character

    // crouch/slide info
    [SerializeField] float crouchHeight;
    [SerializeField] float crouchCamHeight;
    [SerializeField] float crouchCenterHeight;
    [SerializeField] float defaultHeight;
    [SerializeField] float defaultCamHeight;
    [SerializeField] float defaultCenterHeight;

    [SerializeField] float slideTime;
    [SerializeField] float slideTimeMax;
    public bool sliding = false;
    Vector3 slideMove = Vector3.zero;

    // jump info
    [SerializeField] float jumpForce;
    //public bool grounded = false;
    public Vector3 velocity = Vector3.zero;
    Vector3 jumpDir;
    [SerializeField] float jumpGravityDelay; // how long until gravity takes hold of us again

    // jump multipliers
    [SerializeField] float idleJumpMultiplier;
    [SerializeField] float walkJumpMultiplier;
    [SerializeField] float RunJumpMultiplier;
    [SerializeField] float ClimbJumpMultiplier;
    [SerializeField] float wallRunJumpMultiplier;
    [SerializeField] float slideJumpMultiplier;
    [SerializeField] float crouchJumpMultiplier;

    // Vaulting info
    [SerializeField] float vaultSpeed;
    Vector3 newPosition;
    Vector3 oldLocation;
    [SerializeField] float journeyDistance; // saves the distance between our start and end points
    [SerializeField] float distanceCovered; // how far we've gone in the journey
    Vector3 riseCurve;
    [SerializeField] float startTime; // saves reference for when we start moving


    Vector3 center;
    // Start is called before the first frame update
    void Start()
    {
        ChangeSpeed(walkSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        if (myController.currentState != State.Climbing && myController.currentState != State.Wallrunning && myController.currentState != State.noMove && myController.currentState != State.Vaulting) // simulate gravity
        {
            if(velocity.y < 0 && myController.grounded && myController.currentState != State.Sliding) // if we're on the ground and our velocity is high
            {
                myController.CheckMove();
                //Debug.Log("Here");
                velocity.y = -2;
                
            }
            
            SetVelocity();
        }

        if (sliding) // controls how long the player slides
        {
            SlideMove();
            slideTime -= Time.deltaTime;

            if (slideTime < 0)
            {
                CancelSlide();
            }
        }
    }

    private void FixedUpdate()
    {
        if(myController.currentState == State.Vaulting)
        {
            Debug.Log("Vaulting");
            //distanceCovered = (Time.time - startTime) * vaultSpeed;
            //float fractionOfJourney = distanceCovered / journeyDistance; // saves how much of the distance we've already passed
            //gameObject.transform.position = Vector3.Slerp(riseCurve, newPosition, fractionOfJourney);
            ////transform.position += (center * 0.5f);
            myAnimator.SetBool("Vaulting", false);

            if (gameObject.transform.position == newPosition)
            {
                //transform.position += center;
                myController.CheckMove();
                //myCamera.RotatePlayer();
                myAnimator.SetBool("Vaulting", false);
                //Debug.Log("Finished vaulting");
            }
        }
        Debug.DrawLine(oldLocation, newPosition);
    }

    public void Move(Vector3 movementInput) // called from the player input script and gets the inputs from the player
    {
        movement = transform.right * movementInput.x + transform.forward * movementInput.y;

        if (!sliding) // prevents us from changing direction if we are sliding
        {
            switch (myController.currentState) // checks our state and affects our movement depending on the state
            {
                case State.Walking:
                    if (!airControlsOn && myController.grounded == false)
                    {
                        return;
                    }
                    ChangeSpeed(walkSpeed);
                    //movement.y = gravity;
                    MoveInput();
                    myAnimator.SetBool("Idle", true);
                    //myAnimator.SetBool("Running", true);
                    break;

                case State.Running:
                    if (!airControlsOn && myController.grounded == false)
                    {
                        return;
                    }
                    ChangeSpeed(runSpeed);
                    //movement.y = gravity;
                    MoveInput();
                    myAnimator.SetBool("Running", true);
                    break;

                case State.Crouching:
                    if (!airControlsOn && myController.grounded == false)
                    {
                        return;
                    }
                    ChangeSpeed(crouchSpeed);
                    //movement.y = gravity;
                    MoveInput();
                    myAnimator.SetBool("Idle", true);
                    //myAnimator.SetBool("Running", true);
                    break;

                case State.Climbing: // changes our forward and back input to up and down input, locks left and right movement
                    if(myController.curClimbTime < 0)
                    {
                        return;
                    }
                    ChangeSpeed(climbSpeed);
                    Vector3 climbMovement = Vector3.zero;
                    //climbMovement.y = movement.z;
                    climbMovement.y = 1; // todo has problem with moving up and down depending on the movement direction this is only a temperary work around
                    movement = Vector3.zero + climbMovement;
                    MoveInput();
                    break;

                case State.Wallrunning: // locks all movement except forward and back movement
                    //movement.x = 0;
                    //movement.y = 0;
                    //MoveInput();
                    return;

                case State.Sliding: // locks our movement to the direction we were running in
                    if (!airControlsOn && myController.grounded == false)
                    {
                        return;
                    }
                    myController.sprintHeld = false;
                    if (myInput.crouchToggled)
                    {
                        myController.crouchHeld = false;
                    }
                    //velocity = movement;
                    //Debug.Log(velocity);
                    //SetVelocity();
                    //ChangeHeight(crouchHeight);
                    break;

                case State.Jumping:
                    MoveInput();
                    //movement.y = gravity;
                    break;

                case State.Idle:
                    if(myController.grounded)
                    {
                        myAnimator.SetBool("Idle", true);
                        myAnimator.SetBool("Running", false);
                    }
                    break;

                default:
                    return;
            }
        }

        if(myController.grounded == false && myController.currentState != State.Climbing && myController.currentState != State.Wallrunning)
        {
            myController.UpdateState(State.Jumping);
        }

        if(myController.currentState == State.Jumping)
        {
            myAnimator.SetBool("Jumping", true);
            myAnimator.SetBool("Idle", false);
            myAnimator.SetBool("Running", false);
        }

        if(myController.currentState != State.Idle)
        {
            myAnimator.SetBool("Idle", false);
        }
    }

    public void MoveInput() // called if we are movng via player input
    {
        if(airControlsOn && !myController.grounded)
        {
            controller.Move(movement * actualSpeed * airSpeed * Time.deltaTime);
        }
        else
        {
            controller.Move(movement * actualSpeed * Time.deltaTime);
        }
    }

    public void MoveVelocity(Vector3 movement) // called if we are moving via velocity like when we slide, jump or fall
    {
        controller.Move(movement * Time.deltaTime);
    }

    public void Jump() // allows us to jump with a multiplier if we want
    {
        float jumpPower = 0;
        myAnimator.SetBool("Jumping", true);
        myAnimator.SetBool("Running", false);
        myAnimator.SetBool("Idle", false);
        Debug.Log("about to jump");
        switch(myController.currentState) // checks what state we are in and changes our jump accordingly
        {
            case State.Idle:
                jumpPower = idleJumpMultiplier;
                break;

            case State.Walking:
                jumpPower = walkJumpMultiplier;
                break;

            case State.Running:
                jumpPower = RunJumpMultiplier;
                break;

            case State.Crouching:
                jumpPower = crouchJumpMultiplier;
                UnCrouch();
                break;

            case State.Climbing:
                jumpPower = ClimbJumpMultiplier;
                myCamera.ResetZRotation();
                myCamera.RotatePlayer();
                break;

            case State.Wallrunning:
                myController.attachedObject.stop();
                jumpPower = wallRunJumpMultiplier;
                myCamera.ResetZRotation();
                myCamera.RotatePlayer();
                Debug.Log("here");
                break;

            case State.Sliding:
                jumpPower = slideJumpMultiplier;
                CancelSlide();
                myCamera.RotatePlayer();
                //myCamera.RotatePlayer();
                break;

            default:
                jumpPower = 1;
                Debug.Log("Defaulted");
                break;
        }
        
        velocity.y = Mathf.Sqrt(jumpPower * jumpForce * -2f * gravity);
        controller.Move(movement * Time.deltaTime);
        myController.UpdateState(State.Jumping);
    }

    public void SetVelocity()
    {
        if(!sliding)
        {
            velocity.x = 0;
            velocity.z = 0;
        }

        if(myController.currentState != State.Climbing && myController.currentState != State.Vaulting)
        {
            velocity.y += gravity * Time.deltaTime;
            MoveVelocity(velocity);
        }
    }

    public void Crouch()
    {
        ChangeSpeed(crouchSpeed);
        ChangeHeight(crouchHeight, crouchCamHeight, crouchCenterHeight);
        myController.UpdateState(State.Crouching);
    }

    public void UnCrouch()
    {
        RaycastHit heightCheck; // usd to check if we have enough space to stand
        if(Physics.Raycast(transform.position, Vector3.up, out heightCheck, 1.5f)) // checks if theres something above the player preventing them from standing
        {
            if(sliding) // if we can't stand up and we check when sliding we just crouch
            {
                Crouch();
            }
            else
            {
                return; // if theres something blocking us don't stand
            }
        }

        //Debug.Log("Uncrouching");

        ChangeHeight(defaultHeight, defaultCamHeight, defaultCenterHeight);
        if(myController.sprintHeld)
        {
            ChangeSpeed(runSpeed);
            myController.UpdateState(State.Running);
        }
        else
        {
            ChangeSpeed(walkSpeed);

            if(movement == Vector3.zero)
            {
                myController.UpdateState(State.Idle);
            }
            else
            {
                myController.UpdateState(State.Walking);
            }
        }
    }

    public void Slide()
    {
        Debug.Log("Sliding");
        myController.UpdateState(State.Sliding);
        slideMove = movement;
        ChangeHeight(crouchHeight, crouchCamHeight, crouchCenterHeight);
        //Move(myInput.movementInput);
        sliding = true;
        myAnimator.SetBool("Sliding", true);
    }

    public void CancelSlide()
    {
        sliding = false;
        slideTime = slideTimeMax;
        UnCrouch();
        myController.CheckMove();
        myAnimator.SetBool("Sliding", false);
        //velocity = Vector3.zero;
        //SetVelocity();
    }

    void SlideMove()
    {
        controller.Move(slideMove * slideSpeed * Time.deltaTime);
    }

    public void Climb()
    {
        ChangeSpeed(climbSpeed);
        myController.UpdateState(State.Climbing);
    }

    void ChangeSpeed(float newSpeed)
    {
        if(myController.grounded)
        {
            actualSpeed = newSpeed;
        }
    }

    public void ChangeHeight(float newHeight, float newCamHeight, float newColliderCenter) // takes a new height for our player and changes our collider and camera height
    {
        controller.height = newHeight;
        controller.center = new Vector3(0, newColliderCenter, 0);
        myCamera.transform.localPosition = new Vector3(0, newCamHeight, 0);
    }

    public void Vault(Vector3 newLocation) // takes the point that we found in our detection and begins to slerp towards it
    {
        Vector3 pos = transform.position;
        pos.y += 2f;
        transform.position = pos;


        //Debug.Log("Should vault");
        //startTime = Time.time;
        //oldLocation = gameObject.transform.position;
        //Debug.Log("Old location" + oldLocation);
        //gameObject.transform.LookAt(newLocation); // makes our player look at the endpoint
        //journeyDistance = Vector3.Distance(gameObject.transform.position, newLocation);

        //center = (oldLocation + newLocation) * 0.5F;

        //Debug.Log("Center" + center);
        //center -= new Vector3(0, 1, 0);

        //riseCurve = gameObject.transform.position - center;

        //newPosition = newLocation;
        myController.UpdateState(State.Vaulting);
        //myAnimator.SetBool("Vaulting", true);
    }
}
