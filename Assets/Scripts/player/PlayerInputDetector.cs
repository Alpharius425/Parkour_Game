using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class PlayerInputDetector : MonoBehaviour
{
    // script controls the players inputs and communicates with the player controller script

    public Vector2 movementInput; // takes input from the player's controller/keyboard
    public bool canInput = true; // determines if the player make inputs used for if we want cutscenes or something
    public PlayerController myController;
    public PlayerMovementUpdated myMovement;
    [SerializeField] PlayerInput myInputs; // saves our input system and allows us to change controls for different controllers or settings
    public PauseMenu myPauseMenu;
    public packageThrow myPackages;

    [Header("Toggle Settings")]
    public bool crouchToggled; // if true then the crouch is then on toggle detection instead of tap
    public bool sprintToggled;

    private int crouchToggledInt;
    private int sprintToggledInt;

    //public Text crouchButtonText;
    //public Text sprintButtonText;
    public TextMeshProUGUI crouchButtonText;
    public TextMeshProUGUI sprintButtonText;

    // Start is called before the first frame update
    void Start()
    {
        GetCrouchAndSprintSettings();
    }

    private void FixedUpdate()
    {
        if (myController.currentState != State.Sliding)
        {
            myMovement.Move(movementInput);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(canInput)
        {
            // Left mouse click for ThrowPackage. Uses Input Manager, since the Input System can't listen for any mouse inputs at the moment.
            if (Input.GetMouseButtonDown(0))
            {
                
                //Debug.Log("Left Click");
                myPackages.ThrowPackage();
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

            if (value.canceled) {
                PlayerMovementUpdated.Instance.actualSpeed = PlayerMovementUpdated.Instance.startWalkSpeed;
            }
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
                if (value.started) // when we push down on the button  && myController.crouchHeld == false
                {
                    myController.crouchHeld = true;
                    myController.CheckCrouch();
                }

                if(value.canceled) // when we let go of the button  && myController.crouchHeld == true
                {
                    myController.crouchHeld = false;
                    myMovement.UnCrouch();
                }
            }
            else // toggled
            {
                if (value.started && myController.crouchHeld && (myController.currentState == State.Crouching || myController.currentState == State.Sliding))
                {
                    myController.crouchHeld = false;
                    myController.CheckCrouch();
                }
                else if (value.started && myController.crouchHeld == false && (myController.currentState != State.Crouching || myController.currentState != State.Sliding))
                {
                    myController.crouchHeld = true;
                    myController.CheckCrouch();
                }
            }
        }
    }

    public void GetJumpInput(InputAction.CallbackContext value)
    {
        if(canInput && value.started)
        {
            Debug.Log("JumpInput");
            myController.CheckJump();
        }
    }

    public void GetSprintInput(InputAction.CallbackContext value)
    {
        if(canInput)
        {
            if(!sprintToggled)
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
            else
            {
                if(value.started && !myController.sprintHeld)
                {
                    myController.crouchHeld = false;
                    myController.sprintHeld = true;
                    myController.CheckSprint();
                }
                else if(value.started && myController.sprintHeld)
                {
                    myController.sprintHeld = false;

                    if (!myMovement.sliding)
                    {
                        myController.CheckSprint();
                    }
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
    public void GetCrouchAndSprintSettings()
    {
        crouchToggledInt = PlayerPrefs.GetInt("CrouchTogglePref", 1); // get player preferences or set both to toggle detection by default 
        sprintToggledInt = PlayerPrefs.GetInt("SprintTogglePref", 1);

        // set crouch toggle setting according to player preference 
        if (crouchToggledInt == 0)
        {
            crouchToggled = false;
            crouchButtonText.text = "Hold";
        }
        else if (crouchToggledInt == 1)
        {
            crouchToggled = true;
            crouchButtonText.text = "Tap";
        }

        // set sprint toggle setting according to player preference 
        if (sprintToggledInt == 0)
        {
            sprintToggled = false;
            sprintButtonText.text = "Hold";
        }
        else if (sprintToggledInt == 1)
        {
            sprintToggled = true;
            sprintButtonText.text = "Tap";
        }

    }

    public void ToggleCrouchSetting()
    {
        if (crouchToggled == true)
        {
            crouchToggled = false;
            PlayerPrefs.SetInt("CrouchTogglePref", 0);
            crouchButtonText.text = "Hold";
            //Debug.Log("Crouch toggled to hold");
        } 
        else if (crouchToggled == false)
        {
            crouchToggled = true;
            PlayerPrefs.SetInt("CrouchTogglePref", 1);
            crouchButtonText.text = "Tap";
            //Debug.Log("Crouch toggled to tap");
        }

    }
    public void ToggleSprintSetting()
    {
        if (sprintToggled == true)
        {
            sprintToggled = false;
            PlayerPrefs.SetInt("SprintTogglePref", 0);
            sprintButtonText.text = "Hold";
            //Debug.Log("Sprint toggled to hold");

        }
        else if (sprintToggled == false)
        {
            sprintToggled = true;
            PlayerPrefs.SetInt("SprintTogglePref", 1);
            sprintButtonText.text = "Tap";
            //Debug.Log("Sprint toggled to tap");
        }
    }

    public void ChangeSelection(InputAction.CallbackContext value)
    {
        if(myPauseMenu.PauseMenuUI.activeInHierarchy)
        {

        }
    }

    public void ThrowInput(InputAction.CallbackContext value)
    {
        if(canInput && value.started)
        {
            myPackages.ThrowPackage();
        }
    }
}
