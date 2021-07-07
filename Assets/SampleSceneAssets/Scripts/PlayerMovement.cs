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

    [SerializeField] float playerDefaultSpeed;
    public float curSpeed = 12f;
    public float gravity = -10f;
    public float jumpHeight = 2f;

    Vector3 move; // controls our movement and speed

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    

    Vector3 velocity;
    public bool isGrounded;

    [Header("Game Objects")]
    public Camera cameraComponent;

    [Header("Player Sprint Options")] // information for sprinting
    public float sprintSpeed = 15f; // speed multiplier
    public float cameraSprintFOV = 105; // FOV when sprinting
    float cameraDefaultFOV; // used to save our default FOV
    public bool sprinting;

    //#if ENABLE_INPUT_SYSTEM
    //public InputAction movement;
    //public InputAction jump;

    void Start()
    {
        cameraDefaultFOV = cameraComponent.fieldOfView; // saves our default FOV

        //movement = new InputAction("PlayerMovement", binding: "<Gamepad>/leftStick");
        //movement.AddCompositeBinding("Dpad")
        //    .With("Up", "<Keyboard>/w")
        //    .With("Up", "<Keyboard>/upArrow")
        //    .With("Down", "<Keyboard>/s")
        //    .With("Down", "<Keyboard>/downArrow")
        //    .With("Left", "<Keyboard>/a")
        //    .With("Left", "<Keyboard>/leftArrow")
        //    .With("Right", "<Keyboard>/d")
        //    .With("Right", "<Keyboard>/rightArrow");
        
        //jump = new InputAction("PlayerJump", binding: "<Gamepad>/a");
        //jump.AddBinding("<Keyboard>/space");

        //movement.Enable();
        //jump.Enable();
    }
//#endif

    // Update is called once per frame
    void Update()
    {
        //        float x;
        //        float z;
        //        bool jumpPressed = false;

        //#if ENABLE_INPUT_SYSTEM
        //        var delta = movement.ReadValue<Vector2>();
        //        x = delta.x;
        //        z = delta.y;
        //        jumpPressed = Mathf.Approximately(jump.ReadValue<float>(), 1);
        //#else
        //        x = Input.GetAxis("Horizontal");
        //        z = Input.GetAxis("Vertical");
        //        jumpPressed = Input.GetButtonDown("Jump");
        //#endif

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        //        Vector3 move = transform.right * x + transform.forward * z;

        //        controller.Move(move * curSpeed * Time.deltaTime);

        //        if(jumpPressed && isGrounded)
        //        {
        //            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        //        }

        velocity.y += gravity * Time.deltaTime;

        //        controller.Move(velocity * Time.deltaTime);

        //if (Input.GetButtonDown("Sprint") && isGrounded) // if we press the sprint button and are on the ground
        //{
        //    curSpeed = sprintSpeed; // set our speed to the sprint speed
        //    cameraComponent.fieldOfView = cameraSprintFOV; // change our FOV to the sprint FOV
        //}

        //if (Input.GetButtonUp("Sprint")) // if we let go of the sprint button
        //{
        //    curSpeed = playerDefaultSpeed; // set our speed back to normal
        //    cameraComponent.fieldOfView = cameraDefaultFOV; // set our FOV back to normal
        //}

        controller.Move(move * curSpeed * Time.deltaTime);
        controller.Move(velocity * Time.deltaTime);
    }

    public void OnMove(InputAction.CallbackContext value)
    {
        Vector2 inputMovement = value.ReadValue<Vector2>();

        move = transform.right * inputMovement.x + transform.forward * inputMovement.y;

        //controller.Move(move * curSpeed * Time.deltaTime);

        Debug.Log("I moved");
    }

    public void OnJump(InputAction.CallbackContext value)
    {
        if(isGrounded && value.started)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            Debug.Log("I jumped");
        }
    }

    public void Sprint(InputAction.CallbackContext value)
    {
        sprinting = value.started;

        if(isGrounded && sprinting)
        {
            curSpeed = sprintSpeed; // set our speed to the sprint speed
            cameraComponent.fieldOfView = cameraSprintFOV; // change our FOV to the sprint FOV

            Debug.Log("I'm Sprinting");
        }
    }
}
