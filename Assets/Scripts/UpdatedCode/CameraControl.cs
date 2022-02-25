#if ENABLE_INPUT_SYSTEM 
using UnityEngine.InputSystem;
#endif

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    // put on camera attach to the player. Will control the way the camera looks around
    public bool affectRotation = true;
    [SerializeField] GameObject player; // saved reference to the player
    [SerializeField] PlayerController myController;

    public float mouseSensitivity = 100f;
    float xRotation = 0f;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
#if ENABLE_INPUT_SYSTEM
        float mouseX = 0, mouseY = 0;

        if (Mouse.current != null)
        {
            var delta = Mouse.current.delta.ReadValue() / 15.0f;
            mouseX += delta.x;
            mouseY += delta.y;
        }
        if (Gamepad.current != null)
        {
            var value = Gamepad.current.rightStick.ReadValue() * 2;
            mouseX += value.x;
            mouseY += value.y;
        }

        mouseX *= mouseSensitivity * Time.deltaTime;
        mouseY *= mouseSensitivity * Time.deltaTime;
#else
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
#endif

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        if (myController.currentState == State.Idle || myController.currentState == State.Walking || myController.currentState == State.Running || myController.currentState == State.Crouching || myController.currentState == State.Wallrunning || myController.currentState == State.Climbing || myController.currentState == State.Jumping) // checks to see if we are in a state that lets the camera change our rotation
        {
            affectRotation = true;
        }
        else
        {
            affectRotation = false;
        }

        if(affectRotation) // prevents us from changing the player's rotation when we don't want to
        {
            player.transform.Rotate(Vector3.up * mouseX);
        }
    }

    public void ChangeAngle(float angleChange)
    {
        Quaternion newAngle = transform.rotation; // gets the initial rotation
        newAngle.z += angleChange; // gets the change of angle

        transform.rotation = newAngle; // changes the angle of the camera
    }

    public void ResetAngle() // resets the camera to the player's rotation
    {
        transform.rotation = player.transform.rotation;
    }

    public void ResetZRotation()
    {
        Quaternion newAngle = transform.rotation; // gets the initial rotation
        newAngle.z = 0; // gets the change of angle

        transform.rotation = newAngle; // changes the angle of the camera
    }

    public void RotatePlayer() // sets the player's rotation to the camera. messes with wall jump camera rotation IDK
    {
        Quaternion newAngle = transform.rotation; // gets the initial rotation


        newAngle.y = player.transform.rotation.y;
        newAngle.x = 0;
        newAngle.z = 0;
        newAngle.w = 1;
        player.transform.rotation = newAngle;
    }
}
