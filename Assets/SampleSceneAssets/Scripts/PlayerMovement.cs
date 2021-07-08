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

    [SerializeField] Vector3 velocity;
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

    [SerializeField] bool isSliding; // are we sliding
    [SerializeField] float slideTimeMax; // max possible time we should be sliding
    [SerializeField] float slideTime; // how long we've been sliding

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
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask); // checks if we are on the ground

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;

            if(!isSliding)
            {
                // TODO put this somewhere other than update
                velocity.x = 0;
                velocity.z = 0;
            }
        }

        velocity.y += gravity * Time.deltaTime; // simulates gravity

        if (isSliding) // checks if we're sliding
        {
            if(slideTime < slideTimeMax) // if the max time to slide hasn't been met yet
            {
                slideTime += Time.deltaTime;
            }
            else if (slideTime >= slideTimeMax) // if we reach the maximum time to slide
            {
                CancelSlide();
            }
        }
        else // if we aren't sliding move normally
        {
            move = transform.right * inputMovement.x + transform.forward * inputMovement.y;
            controller.Move(move * curSpeed * Time.deltaTime);
        }
        controller.Move(velocity * Time.deltaTime); // keeps the player's velocity no matter what so they move when sliding 
    }

    public void OnMove(InputAction.CallbackContext value) // collects movement data from inputs on the keyboard or gamepad
    {
        inputMovement = value.ReadValue<Vector2>();
    }

    public void OnJump()
    {
        if (isGrounded)
        {
            isGrounded = false;
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        if (isSliding) // if sliding we'll cancel the slide while keeping our velocity
        {
            CancelSlide();
        }
    }

    public void Sprint(InputAction.CallbackContext value)
    {
        if(isGrounded && value.started) // starts sprinting
        {
            sprinting = true;
            curSpeed = sprintSpeed; // set our speed to the sprint speed
            cameraComponent.fieldOfView = cameraSprintFOV; // change our FOV to the sprint FOV
        }

        if(value.canceled) // stops sprinting
        {
            sprinting = false;
            ResetSpeed();
        }
    }

    public void Crouch(InputAction.CallbackContext value)
    {
        if (isSliding && value.started) // if we are sliding and we just pushed the crouch button
        {
            CancelSlide();
        }

        if (isGrounded) // prevents us from crouching or sliding in air
        {
            if (sprinting && !isSliding) // if we are sprinting and not sliding
            {
                isSliding = true;
                sprinting = false;
                Slide();
            }

            if (!sprinting && !isSliding) // checks if we are not sprinting or sliding
            {
                if (value.started) // if we are holding down the crouch button we will lower the player's collider and camera as well as set our speed to the crouch speed
                {
                    crouched = true;
                    CrouchHeight();
                    curSpeed = crouchSpeed;
                }
                if (value.canceled) // when we let go of crouch we will reset the player collider and camera as well as reseting our speed
                {
                    crouched = false;
                    ResetHeight();
                    ResetSpeed();
                }
            }
        }
    }

    void Slide()
    {
        CrouchHeight();

        velocity = (transform.right * inputMovement.x * sprintSpeed) + (transform.forward * inputMovement.y * sprintSpeed); // sets our velocity
    }

    void CancelSlide()
    {
        isSliding = false;
        slideTime = 0;
        ResetHeight();
        ResetSpeed();
    }

    public void ResetSpeed() // will reset our speed whenever we want the player to go back to their default speed like if they stop crouching or sprinting
    {
        curSpeed = playerDefaultSpeed;
        cameraComponent.fieldOfView = cameraDefaultFOV;
    }

    public void ResetHeight() // resets height to default
    {
        myCollider.height = defaultPlayerHeight;
        myCollider.center = new Vector3(0f, defaultPlayerCenter, 0f);
        cameraComponent.transform.localPosition = new Vector3(0f, defaultCamHeight, 0f);
    }

    void CrouchHeight() // sets height to our crouching height
    {
        myCollider.height = crouchedPlayerHeight;
        myCollider.center = new Vector3(0f, crouchedPlayerCenter, 0f);
        cameraComponent.transform.localPosition = new Vector3(0f, crouchedCamHeight, 0f);
    }
}
