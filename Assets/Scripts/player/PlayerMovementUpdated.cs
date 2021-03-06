using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementUpdated : MonoBehaviour
{
    // script controls the player's movement

    public static PlayerMovementUpdated Instance;       // Singleton

    public CharacterController controller;
    public PlayerController myController;
    public CameraControl myCamera;
    public PlayerInputDetector myInput;
    public Animator myLegAnimator;
    public Animator myArmAnimator;

    [Header("Acceleration Settings")]
    [SerializeField] float moveAccel = 4f;
    [SerializeField] float sprintAccel = 4f;
    [SerializeField] float moveDecel = 4f;

    [Header("Moving Settings")]
    [SerializeField] float walkSpeed = 4f;
    public float startWalkSpeed = 6f;

    [Header("Sprinting Settings")]
    [SerializeField] float runSpeed = 8f;

    [Header("Jumping Settings")]
    [SerializeField] float jumpForce;
    public float airSpeed;
    public float savedAirSpeed;
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
    [SerializeField] float crouchGroundCheckHeight;
    [SerializeField] float defaultHeight;
    [SerializeField] float defaultCamHeight;
    [SerializeField] float defaultCenterHeight;
    [SerializeField] float defaultGroundCheckHeight;

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
    [SerializeField] LayerMask slideDetectionLayers;

    [Header("Wallrunning Settings")]
    [SerializeField] float wallRunMinSpeed = 1f;

    [Header("Climbing Settings")]
    [SerializeField] float climbSpeed = 4f;


    public Vector3 movement = Vector3.zero; // the character's actual movement
    [SerializeField] public float gravity;
    [SerializeField] bool airControlsOn = true;


    public float actualSpeed; // the actual speed of the character
    public Vector3 velocity = Vector3.zero;
    RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        actualSpeed = startWalkSpeed;

        savedAirSpeed = airSpeed;
        Instance = this;        // Singleton
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        // Zeroes Z and X rotation.
        // Specifically fixing the rotation issues when vaulting.
        if (transform.rotation.z != 0 || transform.rotation.x != 0)
        {
            CancelZandXRotation();
        }

        if (sliding) // controls how long the player slides
        {
            SlideMove();
            slideTime -= Time.deltaTime;
            myArmAnimator.SetBool("Sliding", true);
            Debug.DrawRay(controller.transform.position, gameObject.transform.forward * slideDetectionRange, Color.red);
            if (Physics.Raycast(controller.transform.position, gameObject.transform.forward, out hit, slideDetectionRange, slideDetectionLayers)) // if we slide into something
            {
                CancelSlide(); // stop sliding
            }

            if (slideTime < 0)
            {
                CancelSlide();
            }
        }

        if (myController.currentState != State.Climbing && myController.currentState != State.Wallrunning && myController.currentState != State.noMove && myController.currentState != State.Vaulting) // simulate gravity
        {
            if (velocity.y < 0 && myController.grounded && myController.currentState != State.Sliding) // if we're on the ground and our velocity is high
            {
                myController.CheckMove();
                //Debug.Log("Here");
                velocity.y = -2;

            }

            SetVelocity();
        }

        if(myController.currentState == State.Wallrunning && myController.attachedObject == false) // might stop us from getting hung up when we finish wall running
        {
            SetVelocity();
        }

        if (myController.currentState == State.Vaulting)
        {
            myInput.canInput = false;
            GetComponent<CharacterController>().enabled = false;
            //Debug.Log("Vaulting");
            //Debug.Log(actualSpeed);

            distanceCovered = (Time.time - startTime) / journeyDistance * vaultSpeed;
            float fractionOfJourney = distanceCovered; // saves how much of the distance we've already passed
            gameObject.transform.position = Vector3.Slerp(riseCurve, fallCurve, fractionOfJourney * vaultSpeed);
            transform.position += center;

            //myLegAnimator.SetBool("Vaulting", false);
            //myArmAnimator.SetBool("Vaulting", false);

            timeSpentVaulting += 2f * Time.deltaTime;
            if (timeSpentVaulting >= journeyDistance / vaultSpeed)
            {
                myInput.canInput = true;

                //Debug.Log("Vaulting finished");

                myController.CheckMove();
                //myCamera.RotatePlayer();

                //myLegAnimator.SetBool("Vaulting", false);
                
                GetComponent<CharacterController>().enabled = true;
                timeSpentVaulting = 0;
            }
        }
    }

    void ChangeSpeed(float newSpeed) {
        if (myController.grounded) {
            // If the current speed is less than the newSpeed, then the current speed will increase at the acceleration rate.
            if (actualSpeed < newSpeed) {
                controller.Move(movement * actualSpeed * Time.fixedDeltaTime / 2f);
                
                // Checks whether the current speed will accelerate at the sprinting accel value, or at the regular accel value.
                if (myController.currentState == State.Running && actualSpeed >= walkSpeed) {
                    actualSpeed += sprintAccel * Time.fixedDeltaTime / 2f;
                }
                else actualSpeed += moveAccel * Time.fixedDeltaTime / 2f;
            }

            // If the current speed is more than the newSpeed, then the current speed will decrease at the deceleration rate.
            else if (actualSpeed > newSpeed) {
                controller.Move(movement * actualSpeed * Time.fixedDeltaTime / 2f);
                actualSpeed -= moveDecel * Time.fixedDeltaTime / 2f;
            }

            // Once the current speed reaches the newSpeed value, then it the current speed will be set to the newSpeed value instead.
            else if (actualSpeed == newSpeed) {
                controller.Move(movement * newSpeed * Time.fixedDeltaTime / 2f);
            }
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
                    ChangeSpeed(walkSpeed);
                    //movement.y = gravity;
                    MoveInput();
                    //myLegAnimator.SetBool("Idle", true);
                    myArmAnimator.SetBool("Walking", true);
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
                    //myLegAnimator.SetBool("Running", true);
                    myArmAnimator.SetBool("Running", true);
                    break;

                case State.Crouching:
                    if (!airControlsOn && myController.grounded == false)
                    {
                        return;
                    }
                    ChangeSpeed(crouchSpeed);
                    //movement.y = gravity;
                    MoveInput();
                    //myLegAnimator.SetBool("Idle", true);
                    myArmAnimator.SetBool("Crouched", true);
                    //myAnimator.SetBool("Running", true);

                    if(movementInput == Vector3.zero)
                    {
                        myArmAnimator.SetBool("Idle", true);
                        myArmAnimator.SetBool("Walking", false);
                    }
                    else
                    {
                        myArmAnimator.SetBool("Idle", false);
                        myArmAnimator.SetBool("Walking", true);
                    }
                    break;

                case State.Climbing: // changes our forward and back input to up and down input, locks left and right movement
                    if(myController.curClimbTime < 0)
                    {
                        return;
                    }
                    //ChangeSpeed(climbSpeed);
                    Vector3 climbMovement = Vector3.zero;
                    //climbMovement.y = movement.z;
                    climbMovement.y = 1f; // todo has problem with moving up and down depending on the movement direction this is only a temperary work around
                    movement = Vector3.zero + climbMovement;
                    //velocity.y = 0f;
                    ResetVelocity();
                    //MoveInput();
                    controller.Move(movement * actualSpeed * airSpeed * Time.deltaTime);
                    break;

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
                        //myLegAnimator.SetBool("Idle", true);
                        myArmAnimator.SetBool("Idle", true);
                        //myLegAnimator.SetBool("Running", false);
                        myArmAnimator.SetBool("Running", false);
                    }
                    break;

                case State.Wallrunning:
                    if (myController.attachedObject != null)
                    {
                        //movement.x = 0;
                        //if(movement.z < 0)
                        //{
                        //    movement.z *= -1;
                        //}
                        //if (movement.z < wallRunMinSpeed)
                        //{
                        //    myController.attachedObject.Stop();
                        //}

                        MoveInput();
                    }
                    break;

                default:
                    return;
            }
        }

        if(myController.currentState != State.Idle)
        {
            myArmAnimator.SetBool("Idle", false);
        }
        if(myController.grounded == false && myController.currentState != State.Climbing && myController.currentState != State.Wallrunning)
        {
            myController.UpdateState(State.Jumping);
        }

        if(myController.currentState == State.Jumping && !myController.grounded)
        {
            //myLegAnimator.SetBool("Jumping", true);
            myArmAnimator.SetBool("Jumping", true);
            //myLegAnimator.SetBool("Idle", false);
            myArmAnimator.SetBool("Idle", false);
            //myLegAnimator.SetBool("Running", false);
            myArmAnimator.SetBool("Running", false);
        }

        if(myController.currentState != State.Idle)
        {
            //myLegAnimator.SetBool("Idle", false);
            //myArmAnimator.SetBool("Idle", false);
        }
    }

    public void MoveInput() // called if we are movng via player input
    {
        if (airControlsOn && !myController.grounded) // && !myController.grounded
        {
            controller.Move(movement * actualSpeed * airSpeed * Time.deltaTime);
        }
        //else
        //{
        //    controller.Move(movement * actualSpeed * Time.deltaTime);
        //}
    }

    public void MoveVelocity(Vector3 movement) // called if we are moving via velocity like when we slide, jump or fall
    {
        //if (myController.currentState == State.Jumping)
        //{
        //    controller.Move(movement * Time.deltaTime * airSpeed);
        //}
        //else
        //{
        //    controller.Move(movement * Time.deltaTime);
        //}

        controller.Move(movement * Time.deltaTime);
    }

    public void SetVelocity()
    {
        if (!sliding)
        {
            velocity.x = 0;
            velocity.z = 0;
        }

        if (myController.currentState != State.Climbing || myController.currentState != State.Vaulting)
        {
            velocity.y += gravity * Time.deltaTime;
            MoveVelocity(velocity);
        }

        if (myController.currentState == State.Climbing || myController.currentState == State.Vaulting) {
            //velocity.y = 0f;
            ResetVelocity();
        }
    }

    public void ResetVelocity()
    {
        velocity = Vector3.zero;
    }

    public void Jump() // allows us to jump with a multiplier if we want
    {
        float jumpPower = 0;
        //myLegAnimator.SetBool("Jumping", true);
        myArmAnimator.SetBool("Jumping", true);
        //myLegAnimator.SetBool("Running", false);
        //myArmAnimator.SetBool("Running", false);
        //myLegAnimator.SetBool("Idle", false);
        myArmAnimator.SetBool("Idle", false);
        //Debug.Log("about to jump");
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
                //myCamera.RotatePlayer();
                break;

            case State.Wallrunning:
                if(myController.attachedObject)
                {
                    movement = myCamera.gameObject.transform.forward * myController.attachedObject.speed;
                    myController.attachedObject.Stop();
                }
                jumpPower = wallRunJumpMultiplier;
                //myCamera.RotatePlayer();
                //Debug.Log("here");
                break;

            case State.Sliding:
                jumpPower = slideJumpMultiplier;
                CancelSlide();
                //myCamera.RotatePlayer();
                break;

            default:
                jumpPower = 1;
                //Debug.Log("Defaulted");
                break;
        }
        JumpMove(jumpPower, movement);
    }

    public void JumpMove(float jumpPower, Vector2 moveDirection)
    {
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 2f))
        {
            if (hit.collider.gameObject.GetComponent<JumpBooster>() != null)
            {
                velocity.y = Mathf.Sqrt(jumpPower * jumpForce * -2f * gravity * hit.collider.gameObject.GetComponent<JumpBooster>().jumpMultiplier);
                controller.Move((moveDirection * hit.collider.gameObject.GetComponent<JumpBooster>().jumpMultiplier) * Time.fixedDeltaTime);
                airSpeed = hit.collider.gameObject.GetComponent<JumpBooster>().airSpeed;
            }
            else
            {
                airSpeed = savedAirSpeed;
                velocity.y = Mathf.Sqrt(jumpPower * jumpForce * -2f * gravity);
                controller.Move(moveDirection * Time.fixedDeltaTime);
            }
        }
        //myController.grounded = false;
        myController.UpdateState(State.Jumping);
    }

    public void ChangeHeight(float newHeight, float newCamHeight, float newColliderCenter, float groundCheckHeight) // takes a new height for our player and changes our collider and camera height
    {
        controller.height = newHeight;
        controller.center = new Vector3(0, newColliderCenter, 0);
        myCamera.transform.localPosition = new Vector3(0, newCamHeight, 0);
        myController.groundCheck.localPosition = new Vector3 (0, groundCheckHeight, 0);
    }

    public void Crouch()
    {
        ChangeSpeed(crouchSpeed);
        ChangeHeight(crouchHeight, crouchCamHeight, crouchCenterHeight, crouchGroundCheckHeight);
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

        ChangeHeight(defaultHeight, defaultCamHeight, defaultCenterHeight, defaultGroundCheckHeight);
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
        //Debug.Log("Sliding");
        myController.UpdateState(State.Sliding);
        slideMove = movement;
        ChangeHeight(crouchHeight, crouchCamHeight, crouchCenterHeight, crouchGroundCheckHeight);
        //Move(myInput.movementInput);
        sliding = true;
        //myLegAnimator.SetBool("Sliding", true);
        myArmAnimator.SetBool("Sliding", true);
    }

    public void CancelSlide()
    {
        sliding = false;
        slideTime = slideTimeMax;
        UnCrouch();
        myController.CheckMove();
        //myLegAnimator.SetBool("Sliding", false);
        myArmAnimator.SetBool("Sliding", false);
        myArmAnimator.SetTrigger("SlideCancel");
        myController.crouchHeld = false;
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
        
        //Debug.Log("Should vault");
        startTime = Time.time;
        oldLocation = gameObject.transform.position;
        //gameObject.transform.LookAt(newLocation); // makes our player look at the endpoint
        journeyDistance = Vector3.Distance(oldLocation, newLocation);

        center = (oldLocation + newLocation) * 0.5F;

        //Debug.Log("oldLocation" + oldLocation + "newLocation" + newLocation);
        center -= new Vector3(0, 1, 0);

        riseCurve = oldLocation - center;
        fallCurve = newLocation - center;

        fallCurve += Vector3.up;
        //Debug.Log("Fall curve" + fallCurve);
        myController.UpdateState(State.Vaulting);
        //myLegAnimator.SetBool("Vaulting", true);
        //myArmAnimator.SetTrigger("Vaulting");
        myArmAnimator.SetTrigger("Vault");
    }

    private void CancelZandXRotation() {
        Quaternion quat = transform.rotation;

        quat.z = 0f;
        quat.x = 0f;

        transform.rotation = quat;
    }
}
