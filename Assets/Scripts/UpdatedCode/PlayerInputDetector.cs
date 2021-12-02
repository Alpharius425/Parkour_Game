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
    public PauseMenu myPauseMenu;

    public bool crouchToggled; // if true then the crouch is then on toggle detection instead of tap


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(canInput)
        {
            if (myController.currentState != State.Sliding)
            {
                myMovement.Move(movementInput);
            }

            if (movementInput == Vector2.zero && myController.grounded != false) // Checks if we are grounded and not moving
            {
                if (myController.crouchHeld == true) // checks if we are holding down the crouch button
                {
                    myController.UpdateState(State.Crouching); // if we holding crouch set us to crouching
                                                               //Debug.Log("I'm crouching");
                }
                else
                {
                    if (myController.currentState != State.Crouching)
                    {
                        myController.UpdateState(State.Idle); // if we aren't crouching set us to idle
                                                              //Debug.Log("I'm idle");
                    }
                }
            }
        }
    }

    public void GetMoveInput(InputAction.CallbackContext value) // gets our directional movement from the player
    {
        if(canInput)
        {
            if(myController.sprintHeld != true) // checks if we are sprinting
            {
                if(myController.crouchHeld != true)
                {
                    myController.UpdateState(State.Walking); // if we aren't sprinting or crouching update our state to walking normally
                }
            }
            Vector2 newInput = Vector2.zero;
            newInput = value.ReadValue<Vector2>();
            movementInput = newInput;
        }
        else if(!canInput || value.ReadValue<Vector2>() == Vector2.zero)
        {
            movementInput = Vector3.zero;
            //myController.UpdateState(State.Idle);
        }
    }

    public void GetCrouchInput(InputAction.CallbackContext value)
    {
        if(canInput)
        {
            if (!crouchToggled) // not toggled
            {
                if (value.started && myController.crouchHeld == false) // when we push down on the button
                {
                    myController.crouchHeld = true;
                    myController.CheckCrouch();
                }
                if (value.canceled && myController.crouchHeld == true) // when we let go of the button
                {
                    myController.crouchHeld = false;
                    if (!myMovement.sliding)
                    {
                        myController.CheckCrouch();
                    }
                }
            }
            else // toggled
            {
                if (value.started && myController.crouchHeld && myController.currentState == State.Crouching)
                {
                    myController.crouchHeld = false;
                    myController.CheckCrouch();
                }
                else if (value.started && myController.crouchHeld == false && myController.currentState != State.Crouching)
                {
                    myController.crouchHeld = true;
                    myController.CheckCrouch();
                }
            }
        }
    }

    public void GetJumpInput(InputAction.CallbackContext value)
    {
        if(canInput)
        {
            myController.CheckJump();
        }
    }

    public void GetSprintInput(InputAction.CallbackContext value)
    {
        if(canInput)
        {
            if (value.started)
            {
                myController.crouchHeld = false;
                myController.sprintHeld = true;
                myController.CheckSprint();
            }

            if (value.canceled)
            {
                myController.sprintHeld = false;

                if (!myMovement.sliding)
                {
                    myController.CheckSprint();
                }
            }
        }
    }

    public void TogglePause(InputAction.CallbackContext value)
    {
        myPauseMenu.PauseToggle();

        if(myPauseMenu.Paused)
        {
            canInput = false;
        }
        else
        {
            canInput = true;
        }
    }

    public void ChangeSelection(InputAction.CallbackContext value)
    {
        if(myPauseMenu.PauseMenuUI.activeInHierarchy)
        {

        }
    }
}
