#if ENABLE_INPUT_SYSTEM 
using UnityEngine.InputSystem;
#endif

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController controller;
    [SerializeField] PlayerInput myInputs; // saves our input system and allows us to change controls for different controllers or settings

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    [Header("Game Objects")]
    public Camera cameraComponent; // saves our camera so we can edit it if needed

    [Header("Player Movement Variables")]
    [SerializeField] float playerDefaultSpeed;
    public float curSpeed = 12f; // how fast we are currently going
    public float gravity = -10f; // how hard we are affected by gravity
    public float jumpHeight = 2f; // how much force we use to jump

    Vector3 velocity;
    public bool isGrounded; // checks if we are on the ground or in the air

    Vector2 inputMovement; // saves input from movement keys
    Vector3 move; // controls our movement and speed

    [Header("Player Sprint Options")] // information for sprinting
    public float sprintSpeed = 15f; // speed multiplier
    public float cameraSprintFOV = 105; // FOV when sprinting
    float cameraDefaultFOV; // used to save our default FOV
    public bool sprinting; // checks if the player is sprinting

    [Header("Player Crouch Options")]
    public float defaultPlayerHeight; // height for our collider when not crouching
    public float crouchedPlayerHeight; // height for our collider when crouched
    public float defaultPlayerCenter; // height for our collider when not crouching
    public float crouchedPlayerCenter; // height for our collider when crouched
    [SerializeField] CapsuleCollider myCollider; // saves our collider so we can edit it
    [SerializeField] float crouchSpeed;

    public float defaultCamHeight; // height for our camera when not crouching
    public float crouchedCamHeight; // height for our camera when crouched
    [SerializeField] bool crouched = false; // tells us if we are crouched
    

    void Start()
    {
        cameraDefaultFOV = cameraComponent.fieldOfView; // saves our default FOV
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        
        velocity.y += gravity * Time.deltaTime;
        move = transform.right * inputMovement.x + transform.forward * inputMovement.y;
        controller.Move(move * curSpeed * Time.deltaTime);
        controller.Move(velocity * Time.deltaTime);
    }

    public void OnMove(InputAction.CallbackContext value)
    {
        inputMovement = value.ReadValue<Vector2>();
    }

    public void OnJump()
    {
        if(isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    public void Sprint(InputAction.CallbackContext value)
    {
        if(isGrounded && value.started)
        {
            sprinting = true;
            curSpeed = sprintSpeed; // set our speed to the sprint speed
            cameraComponent.fieldOfView = cameraSprintFOV; // change our FOV to the sprint FOV
        }

        if(value.canceled)
        {
            sprinting = false;
            ResetSpeed();
        }
    }

    public void Crouch(InputAction.CallbackContext value)
    {
        if(!sprinting) // checks if we are sprinting or not
        {
            if (value.started) // if we are sprinting and holding down the crouch button we will lower the player's collider and camera as well as set our speed to the crouch speed
            {
                crouched = true;
                myCollider.height = crouchedPlayerHeight;
                myCollider.center = new Vector3(0f, crouchedPlayerCenter, 0f);
                cameraComponent.transform.localPosition = new Vector3(0f, crouchedCamHeight, 0f);
                curSpeed = crouchSpeed;
            }
            if (value.canceled) // when we let go of crouch we will reset the player collider and camera as well as reseting our speed
            {
                crouched = false;
                myCollider.height = defaultPlayerHeight;
                myCollider.center = new Vector3(0f, defaultPlayerCenter, 0f);
                cameraComponent.transform.localPosition = new Vector3(0f, defaultCamHeight, 0f);
                ResetSpeed();
            }
        }
        else // if we are sprinting TODO Sliding
        {

        }
    }

    public void ResetSpeed() // will reset our speed whenever we want the player to go back to their default speed like if they stop crouching or sprinting
    {
        curSpeed = playerDefaultSpeed;
        cameraComponent.fieldOfView = cameraDefaultFOV;
    }
}
