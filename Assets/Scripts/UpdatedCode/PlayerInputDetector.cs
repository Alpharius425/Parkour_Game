using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputDetector : MonoBehaviour
{
    // script controls the players inputs and communicates with the player controller script

    public Vector2 movementInput; // takes input from the player's controller/keyboard
    public bool canInput = true; // determines if the player make inputs used for if we want cutscenes or something
    public PlayerController myController;
    public PlayerMovementUpdated myMovement;
    [SerializeField] PlayerInput myInputs; // saves our input system and allows us to change controls for different controllers or settings


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        myMovement.Move(movementInput);
        
        if(movementInput == Vector2.zero && myController.grounded != false && myController.currentState != State.Idle) // sets our state to idle if we are grounded and not moving
        {
            myController.UpdateState(State.Idle);
        }
    }

    public void GetMoveInput(InputAction.CallbackContext value) // gets our directional movement from the player
    {
        if(canInput)
        {
            if(myController.sprintHeld != true) // checks if we are sprinting
            {
                myController.UpdateState(State.Walking); // if we aren't sprinting update our state to walking normally
            }
            Vector2 newInput = Vector2.zero;
            newInput = value.ReadValue<Vector2>();
            movementInput = newInput;
        }
        else if(!canInput || value.ReadValue<Vector2>() == Vector2.zero)
        {
            movementInput = Vector3.zero;
            myController.UpdateState(State.Idle);
        }
    }

    public void GetCrouchInput(InputAction.CallbackContext value)
    {
        myController.CheckCrouch();
    }

    public void GetJumpInput(InputAction.CallbackContext value)
    {
        myController.CheckJump();
    }

    public void GetSprintInput(InputAction.CallbackContext value)
    {
        if(value.started)
        {
            myController.sprintHeld = true;
            myController.CheckSprint();
        }

        if(value.canceled)
        {
            myController.sprintHeld = false;
            myController.CheckSprint();
        }
    }
}
