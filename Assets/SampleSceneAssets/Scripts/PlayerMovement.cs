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

    [SerializeField] Vector2 inputMovement; // saves input from movement keys
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

    [SerializeField] GameObject obstacleDetector; // a empty gameobject with a trigger collider and a VaultingAndClimbingDetection script
    public bool detectsSomething = false;
    [SerializeField] bool vaulting; // tells us if we are vaulting or not
    [SerializeField] float vaultSpeed; // how fast we vault over an object
    [SerializeField] float climbHeight; // how tall we want to be able to climb
    [SerializeField] float reachDis; // how far away we want to grab

    [Header("Player Climbing Options")]
    [SerializeField] float maxClimbTime; // how long the character can climb for
    [SerializeField] float curClimbTime; // how long we have been climbing for
    [SerializeField] bool climbing; // helps us determine whether we are climbing or not
    [SerializeField] float climbSpeed; // how fast we move while climbing
    [SerializeField] float timeBetweenClimb; // cooldown for when we can climb again. might keep might not

    Vector3 myLocation; // used for vaulting saves where the player starts
    Vector3 vaultLocation; // used for vaulting saves where the player should be going
    

    void Start()
    {
        cameraDefaultFOV = cameraComponent.fieldOfView; // saves our default FOV
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask); // checks if we are on the ground

        if (isGrounded && velocity.y < 0 && !vaulting)
        {
            velocity.y = -2f;

            if(!isSliding)
            {
                // TODO put this somewhere other than update
                velocity.x = 0;
                velocity.z = 0;
            }
        }

        if (!vaulting)
        {
            velocity.y += gravity * Time.deltaTime; // simulates gravity
            if (isSliding) // checks if we're sliding
            {
                if (slideTime < slideTimeMax) // if the max time to slide hasn't been met yet
                {
                    slideTime += Time.deltaTime;
                }
                else if (slideTime >= slideTimeMax) // if we reach the maximum time to slide
                {
                    CancelSlide();
                }
            }
            else if(climbing) // if we are climbing
            {
                move = transform.right * inputMovement.x + transform.up * inputMovement.y; // lets us move up or down while climbing
                curClimbTime += Time.deltaTime; // starts ticking the climb time forward
                controller.Move(move * climbSpeed * Time.deltaTime);

                if (curClimbTime >= maxClimbTime) // if we go over the max climb time
                {
                    StopClimbing();
                }
            }
            else if (!isSliding || !climbing) // if we aren't sliding or climbing move normally
            {
                move = transform.right * inputMovement.x + transform.forward * inputMovement.y;
                controller.Move(move * curSpeed * Time.deltaTime);
            }

            if(!climbing) // only affected by gravity when not climbing
            {
                controller.Move(velocity * Time.deltaTime); // keeps the player's velocity no matter what so they move when sliding 
                timeBetweenClimb -= Time.deltaTime; // lowers the climbing cooldown
            }
        }
        else // if we are vaulting
        {
            transform.position = Vector3.Lerp(myLocation, vaultLocation, vaultSpeed); // moves our player to the new location
            myCollider.enabled = false;
            if (gameObject.transform.position == vaultLocation) // resets our bool so we aren't constantly vaulting
            {
                myCollider.enabled = true;
                vaulting = false;
            }
        }

        // Debug rays for our vaulting
        Vector3 origin = transform.position; // sets up our ray to see if the obstacle is something we can climb over
        //origin.y += 1f; // adjust the ray's position to be about the hip height of the player
        Debug.DrawRay(origin, transform.forward * reachDis, Color.blue);

        Vector3 climbCheck = transform.position; // saves a new location to shoot a ray cast from that checks if we have space to climb
        climbCheck.y += climbHeight; // adjust the height of the new ray to take into account our climbing height
        Debug.DrawRay(climbCheck, transform.forward * reachDis, Color.blue);

        Vector3 groundCheck2 = climbCheck + transform.forward; // makes another ray that looks forwardand down to find a place to go to
        //groundCheck2.x += 1;
        Debug.DrawRay(groundCheck2, Vector3.down * climbHeight, Color.blue);

    }

    public void OnMove(InputAction.CallbackContext value) // collects movement data from inputs on the keyboard or gamepad
    {
        inputMovement = value.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext value)
    {
        //TODO: Jumps when activating the vaulting

        if (isSliding) // if sliding we'll cancel the slide while keeping our velocity
        {
            CancelSlide();
        }

        if (value.started) // when we push and hold the button
        {
            obstacleDetector.SetActive(true);

            //if(detectsSomething)
            //{
            //    VaultDetection();
            //}
            if(isGrounded && !detectsSomething) // check if we are on the ground
            {
                isGrounded = false; // tell us we aren't on the ground
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); // jump
            }
        }
        if(value.canceled) // when we let go of the button
        {
            StopClimbing();
            obstacleDetector.SetActive(false);
        }
    }

    public void Sprint(InputAction.CallbackContext value)
    {
        if(isGrounded && value.started && !vaulting) // starts sprinting
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

        if (isGrounded && !vaulting) // prevents us from crouching or sliding in air
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

    public void VaultDetection() // runs if the vault detector collider hits something and then determines if the player can vault over the object that was hit
    {
        // TODO: has trouble with getting over objects slightly too far away
        RaycastHit hit; //tells us if the raycast has hit anything
        Vector3 origin = transform.position; // sets up our ray to see if the obstacle is something we can climb over
        
        if(Physics.Raycast(origin, transform.forward, out hit, reachDis)) // we hit something and it is at least as tall as our hip height
        {

            Vector3 climbCheck = transform.position; // saves a new location to shoot a ray cast from that checks if we have space to climb
            climbCheck.y += climbHeight; // adjust the height of the new ray to take into account our climbing height
            
            if(!Physics.Raycast(climbCheck, transform.forward, out hit, reachDis)) // checks if there is space for the character to climb up
            {

                Vector3 groundCheck = climbCheck + (transform.forward * reachDis); // makes another ray that looks forwardand down to find a place to go to

                if (Physics.Raycast(groundCheck, Vector3.down, out hit, climbHeight)) // checks if there is space to climb up to
                {
                    StopClimbing();
                    Vault(hit.point); // sends the location that the ray hit to the vault function
                }
                else
                {
                    if(timeBetweenClimb <= 0)
                    {
                        climbing = true;
                    }
                }
            }
            else
            {
                if (timeBetweenClimb <= 0)
                {
                    climbing = true;
                }
            }
        }
    }

    void StopClimbing()
    {
        timeBetweenClimb = curClimbTime; // TEMP sets the climb time to be the amount of time we spent climbing 
        climbing = false; // stop us from climbing
        curClimbTime = 0; // resets the amount of time we can climb
    }

    public void Vault(Vector3 newLocation) // calculates the location of where the player should end up when vaulting 
    {
        vaulting = true; // tells the rest of the code we are vaulting now
        myLocation = transform.position; // gets our current location
        vaultLocation = newLocation; // sets the location we want to go to  + transform.localScale
    }
}
