using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerSprint : MonoBehaviour
{
    [Header("Game Objects")]
    //public GameObject playerObject;
    public PlayerMovement playerMovementScript;
    public Camera cameraComponent;

    [Header("Player Sprint Options")]
    public float sprintSpeed = 1.5f;
    public float cameraSprintFOV = 105;
    float playerDefaultSpeed;
    float cameraDefaultFOV;

    // Start is called before the first frame update
    void Start()
    {
        playerDefaultSpeed = playerMovementScript.speed;
        cameraDefaultFOV = cameraComponent.fieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Sprint") && playerMovementScript.isGrounded)
        {
            playerMovementScript.speed = sprintSpeed;
            cameraComponent.fieldOfView = cameraSprintFOV;
        }

        if (Input.GetButtonUp("Sprint"))
        {
            playerMovementScript.speed = playerDefaultSpeed;
            cameraComponent.fieldOfView = cameraDefaultFOV;
        }
    }
}
