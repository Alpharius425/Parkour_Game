using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementUpdated : MonoBehaviour
{
    // Singleton
    public static PlayerMovementUpdated Instance;

    // script controls the player's movement

    public CharacterController controller;
    public PlayerController myController;
    public CameraControl myCamera;
    public PlayerInputDetector myInput;
    public Animator myAnimator;

    [Header("Moving Settings")]
    public float walkSpeed = 4f;
    [SerializeField] float maxWalkSpeed = 4f;
    [SerializeField] float walkAccel = 1.5f;    // Speed acceleration multiplier from running to walking.
    [SerializeField] float walkDecel = 1.5f;    // Speed deceleration multiplier from running to walking.

    [Header("Sprinting Settings")]
    public float runSpeed = 8f;
    [SerializeField] float maxRunSpeed = 8f;
    [SerializeField] float runAccel = 1.5f;     // Speed acceleration multiplier from walking to running.
    [SerializeField] float runDecel = 1.5f;     // Speed deceleration multiplier from walking to running.

    [Header("Jumping Settings")]
    [SerializeField] float jumpForce;
    [SerializeField] float airSpeed;
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

    [Header("Crouching Settings")]
    [SerializeField] float crouchSpeed = 2f;
    [SerializeField] float crouchHeight;
    [SerializeField] float crouchCamHeight;
    [SerializeField] float crouchCenterHeight;
    [SerializeField] float defaultHeight;
    [SerializeField] float defaultCamHeight;
    [SerializeField] float defaultCenterHeight;

    [Header("Vaulting Settings")]
    [SerializeField] float vaultSpeed;
    Vector3 oldLocation;
    [SerializeField] float journeyDistance; // saves the distance between our start and end points
    [SerializeField] float distanceCovered; // how far we've gone in the journey
    Vector3 riseCurve;
    Vector3 fallCurve;
    [SerializeField] float startTime; // saves reference for when we start moving
    [SerializeField] float timeSpentVaulting;
    Vector3 center;

    [Header("Sliding Settings")]
    [SerializeField] float slideSpeed = 8f;
    [SerializeField] float slideTime;
    [SerializeField] float slideTimeMax;
    public bool sliding = false;
    Vector3 slideMove = Vector3.zero;
    [SerializeField] float slideDetectionRange;

    [Header("Wallrunning Settings")]
    //[SerializeField] float wallRunSpeed = 4f;

    [Header("Climbing Settings")]
    [SerializeField] float climbSpeed = 4f;


    public Vector3 movement = Vector3.zero; // the character's actual movement
    [SerializeField] float gravity;
    [SerializeField] bool airControlsOn = true;
    
    
    [SerializeField] public float actualSpeed; // the actual speed of the character
    public Vector3 velocity = Vector3.zero;
    RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        // Singleton
        Instance = this;

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

            if(Physics.Raycast(transform.position, Vector3.forward, out hit, slideDetectionRange)) // if we slide into something
            {
                CancelSlide(); // stop sliding
            }

            if (slideTime < 0)
            {
                CancelSlide();
            }
        }
    }

    private void FixedUpdate()
    {
        // Zeroes Z and X rotation.
        // Specifically fixing the rotation issues when vaulting.
        if (transform.rotation.z != 0 || transform.rotation.x != 0) {
            CancelZandXRotation();
        }


        if (myController.currentState == State.Vaulting)
        {
            myInput.canInput = false;
            GetComponent<CharacterController>().enabled = false;
            Debug.Log("Vaulting");

            distanceCovered = (Time.time - startTime) / journeyDistance * vaultSpeed;
            float fractionOfJourney = distanceCovered; // saves how much of the distance we've already passed
            gameObject.transform.position = Vector3.Slerp(riseCurve, fallCurve, fractionOfJourney * vaultSpeed);
            transform.position += center;

            myAnimator.SetBool("Vaulting", false);

            timeSpentVaulting += Time.deltaTime;
            if (timeSpentVaulting >= journeyDistance / vaultSpeed)
            {
                myInput.canInput = true;

                Debug.Log("Vaulting finished");

                myController.CheckMove();
                //myCamera.RotatePlayer();

                myAnimator.SetBool("Vaulting", false);

                GetComponent<CharacterController>().enabled = true;
                timeSpentVaulting = 0;
            }
        }
    }

    void ChangeSpeed(float newSpeed)
    {
        if (myController.grounded)
        {
            actualSpeed = newSpeed;
        }
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
                    //ChangeSpeed(walkSpeed);
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
                    //ChangeSpeed(runSpeed);
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
        if (airControlsOn && !myController.grounded)
        {
            controller.Move(movement * actualSpeed * airSpeed * Time.deltaTime);
        }
        else
        {
            //controller.Move(movement * actualSpeed * Time.deltaTime);

            switch (myController.currentState)
            {
                // Linearly adds a multiplier value to the actual speed over time, giving a gradual increase in speed when walking.
                case State.Walking:
                    if (actualSpeed < maxWalkSpeed) {
                        controller.Move(movement * actualSpeed * Time.deltaTime);
                        actualSpeed += walkAccel * Time.deltaTime;
                    }
                    // Once the actual speed meets the maxWalkSpeed value, the player will only move at the maxWalkSpeed value.
                    else controller.Move(movement * maxWalkSpeed * Time.deltaTime);
                    break;
                case State.Running:
                    if (actualSpeed < maxRunSpeed)
                    {
                        controller.Move(movement * actualSpeed * Time.deltaTime);
                        actualSpeed += runAccel * Time.deltaTime;
                    }
                    // Once the actual speed meets the maxWalkSpeed value, the player will only move at the maxWalkSpeed value.
                    else controller.Move(movement * maxRunSpeed* Time.deltaTime);
                    break;
            }
            
        }
    }

    public void MoveVelocity(Vector3 movement) // called if we are moving via velocity like when we slide, jump or fall
    {
        controller.Move(movement * Time.deltaTime);
    }

    public void SetVelocity()
    {
        if (!sliding)
        {
            velocity.x = 0;
            velocity.z = 0;
        }

        if (myController.currentState != State.Climbing && myController.currentState != State.Vaulting)
        {
            velocity.y += gravity * Time.deltaTime;
            MoveVelocity(velocity);
        }
    }

    public void ResetVelocity()
    {
        velocity = Vector3.zero;
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

        if(Physics.Raycast(transform.position, Vector3.down, out hit, 2f))
        {
            if(hit.collider.gameObject.GetComponent<JumpBooster>() != null)
            {
                velocity.y = Mathf.Sqrt(jumpPower * jumpForce * -2f * gravity * hit.collider.gameObject.GetComponent<JumpBooster>().jumpMultiplier);
            }
            else
            {
                velocity.y = Mathf.Sqrt(jumpPower * jumpForce * -2f * gravity);
            }
        }
        
        controller.Move(movement * Time.deltaTime);
        myController.UpdateState(State.Jumping);
    }

    public void ChangeHeight(float newHeight, float newCamHeight, float newColliderCenter) // takes a new height for our player and changes our collider and camera height
    {
        controller.height = newHeight;
        controller.center = new Vector3(0, newColliderCenter, 0);
        myCamera.transform.localPosition = new Vector3(0, newCamHeight, 0);
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
        if (Physics.Raycast(transform.position, Vector3.up, out heightCheck, 1.5f)) // checks if theres something above the player preventing them from standing
        {
            if (sliding) // if we can't stand up and we check when sliding we just crouch
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
        if (myController.sprintHeld)
        {
            ChangeSpeed(runSpeed);
            myController.UpdateState(State.Running);
        }
        else
        {
            ChangeSpeed(walkSpeed);

            if (movement == Vector3.zero)
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

    public void Vault(Vector3 newLocation) // takes the point that we found in our detection and begins to slerp towards it
    {
        //Vector3 pos = transform.position;
        //pos.y += 2f;
        //transform.position = pos;
        
        Debug.Log("Should vault");
        startTime = Time.time;
        oldLocation = gameObject.transform.position;
        gameObject.transform.LookAt(newLocation); // makes our player look at the endpoint
        journeyDistance = Vector3.Distance(oldLocation, newLocation);

        center = (oldLocation + newLocation) * 0.5F;

        Debug.Log("oldLocation" + oldLocation + "newLocation" + newLocation);
        center -= new Vector3(0, 1, 0);

        riseCurve = oldLocation - center;
        fallCurve = newLocation - center;

        fallCurve += Vector3.up;
        Debug.Log("Fall curve" + fallCurve);
        myController.UpdateState(State.Vaulting);
        myAnimator.SetBool("Vaulting", true);
    }

    private void CancelZandXRotation() {
        Quaternion quat = transform.rotation;

        quat.z = 0f;
        quat.x = 0f;

        transform.rotation = quat;
    }
}
