using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementUpdated : MonoBehaviour
{
    // script controls the player's movement

    public CharacterController controller;
    public PlayerController myController;
    public CameraControl myCamera;

    public Vector3 movement = Vector3.zero; // the character's actual movement
    [SerializeField] float gravity;

    // speed info
    [SerializeField] float walkSpeed = 4f;
    [SerializeField] float runSpeed = 8f;
    [SerializeField] float crouchSpeed = 2f;
    [SerializeField] float climbSpeed = 4f;
    [SerializeField] float wallRunSpeed = 4f;
    [SerializeField] private float actualSpeed; // the actual speed of the character

    // crouch/slide info
    [SerializeField] float crouchHeight;
    [SerializeField] float slideTime;
    [SerializeField] float slideTimeMax;

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
        if (myController.currentState != State.Climbing || myController.currentState != State.Wallrunning) // simulate gravity
        {
            controller.Move(velocity * Time.deltaTime);
        }
    }

    public void Move(Vector3 movementInput) // called from the player input script and gets the inputs from the player
    {
        movement = transform.right * movementInput.x + transform.forward * movementInput.y;

        switch (myController.currentState) // checks our state and affects our movement depending on the state
        {
            case State.Walking:
                ChangeSpeed(walkSpeed);
                //movement.y = gravity;
                break;

            case State.Running:
                ChangeSpeed(runSpeed);
                //movement.y = gravity;
                break;

            case State.Crouching:
                ChangeSpeed(crouchSpeed);
                //movement.y = gravity;
                break;

            case State.Climbing: // changes our forward and back input to up and down input, locks left and right movement
                ChangeSpeed(climbSpeed);
                Vector3 climbMovement = Vector3.zero;
                //climbMovement.y = movement.z;
                climbMovement.y = 1; // todo has problem with moving up and down depending on the movement direction this is only a temperary work around
                movement = Vector3.zero + climbMovement;
                break;

            case State.Wallrunning: // locks all movement except forward and back movement
                movement.x = 0;
                movement.y = 0;
                break;

            case State.Sliding: // locks our movement to the direction we were running in
                movement.y = gravity;
                break;

            case State.Jumping:
                movement.y = gravity;
                break;

            default:
                return;
        }
        controller.Move(movement * actualSpeed * Time.deltaTime);
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
        movement.y = jumpPower * jumpForce;
        controller.Move(movement * Time.deltaTime);
        myController.UpdateState(State.Jumping);
    }

    public void SetVelocity()
    {

    }

    public void Crouch()
    {
        ChangeSpeed(crouchSpeed);
        myController.UpdateState(State.Crouching);
    }

    public void UnCrouch()
    {
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
        myController.UpdateState(State.Sliding);
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
}
