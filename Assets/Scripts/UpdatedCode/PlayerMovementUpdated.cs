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

    public Vector3 movement = Vector3.zero; // the character's actual movement
    [SerializeField] float gravity;
    [SerializeField] bool airControlsOn = true;

    [SerializeField] CapsuleCollider myCollider;

    // speed info
    [SerializeField] float walkSpeed = 4f;
    [SerializeField] float runSpeed = 8f;
    [SerializeField] float crouchSpeed = 2f;
    [SerializeField] float climbSpeed = 4f;
    [SerializeField] float wallRunSpeed = 4f;
    [SerializeField] float slideSpeed = 8f;
    [SerializeField] float airSpeed;
    [SerializeField] private float actualSpeed; // the actual speed of the character

    // crouch/slide info
    [SerializeField] float crouchHeight;
    [SerializeField] float defaultHeight;

    [SerializeField] float slideTime;
    [SerializeField] float slideTimeMax;
    public bool sliding = false;

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

    

    // Start is called before the first frame update
    void Start()
    {
        ChangeSpeed(walkSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        if (myController.currentState != State.Climbing && myController.currentState != State.Wallrunning) // simulate gravity
        {
            if(velocity.y < 0 && myController.grounded && myController.currentState != State.Sliding) // if we're on the ground and our velocity is high
            {
                myController.CheckMove();
                velocity.y = -2;
                
            }
            
            SetVelocity();
        }

        //if(sliding) // controls how long the player slides
        //{
        //    velocity = movement;
        //    SetVelocity();
        //    slideTime -= Time.deltaTime;
            
        //    if(slideTime < 0)
        //    {
        //        CancelSlide();
        //    }
        //}
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
                    break;

                case State.Running:
                    if (!airControlsOn && myController.grounded == false)
                    {
                        return;
                    }
                    ChangeSpeed(runSpeed);
                    //movement.y = gravity;
                    MoveInput();
                    break;

                case State.Crouching:
                    if (!airControlsOn && myController.grounded == false)
                    {
                        return;
                    }
                    ChangeSpeed(crouchSpeed);
                    //movement.y = gravity;
                    MoveInput();
                    break;

                case State.Climbing: // changes our forward and back input to up and down input, locks left and right movement
                    ChangeSpeed(climbSpeed);
                    Vector3 climbMovement = Vector3.zero;
                    //climbMovement.y = movement.z;
                    climbMovement.y = 1; // todo has problem with moving up and down depending on the movement direction this is only a temperary work around
                    movement = Vector3.zero + climbMovement;
                    MoveInput();
                    break;

                case State.Wallrunning: // locks all movement except forward and back movement
                    movement.x = 0;
                    movement.y = 0;
                    MoveInput();
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
                    velocity = movement;
                    Debug.Log(velocity);
                    SetVelocity();
                    //ChangeHeight(crouchHeight);
                    break;

                case State.Jumping:
                    //movement.y = gravity;
                    break;

                default:
                    return;
            }
        }
    }

    public void MoveInput() // called if we are movng via player input
    {
        if(airControlsOn)
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
        //if(sliding)
        //{
        //    controller.Move(movement * slideSpeed * Time.deltaTime);
        //}
        //else
        //{
        //    controller.Move(movement * Time.deltaTime);
        //}

        controller.Move(movement * Time.deltaTime);
    }

    public void Jump() // allows us to jump with a multiplier if we want
    {
        float jumpPower = 0;

        
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
                break;

            case State.Climbing:
                jumpPower = ClimbJumpMultiplier;
                myCamera.RotatePlayer();
                break;

            case State.Wallrunning:
                jumpPower = walkJumpMultiplier;
                myCamera.ResetZRotation();
                myCamera.RotatePlayer();
                break;

            case State.Sliding:
                jumpPower = slideJumpMultiplier;
                myCamera.RotatePlayer();
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
        Debug.Log("Crouching");

        ChangeSpeed(crouchSpeed);
        ChangeHeight(crouchHeight);
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

        Debug.Log("Uncrouching");

        ChangeHeight(defaultHeight);
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
        //Move(myInput.movementInput);
        sliding = true;
    }

    public void CancelSlide()
    {
        sliding = false;
        slideTime = slideTimeMax;
        UnCrouch();
        myController.CheckMove();
        velocity = Vector3.zero;
        SetVelocity();
    }

    public void Climb()
    {
        ChangeSpeed(climbSpeed);
        myController.UpdateState(State.Climbing);
    }

    void ChangeSpeed(float newSpeed)
    {
        actualSpeed = newSpeed;
    }

    public void ChangeHeight(float newHeight) // takes a new height for our player and changes our collider and camera height
    {
        myCollider.height = newHeight;
        //myCollider.center = new Vector3(0, newHeight, 0);
        //myCamera.transform.localPosition = new Vector3(0, newHeight, 0);
    }
}
