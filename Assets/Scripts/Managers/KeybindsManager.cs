using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.PlayerInput;

public class KeybindsManager : MonoBehaviour
{
    private PlayerInput playerInput;

    private string previousDevice;
    public static string currentDevice;

    public delegate void DeviceChange();
    public static event DeviceChange OnDeviceChange;

    private void Awake()
    {
        playerInput = FindObjectOfType<PlayerInput>();
        //previousDevice = "";
        //ChangeCurrentDevice();
    }

    private void Update()
    {
        ChangeCurrentDevice();
    }

    private void ChangeCurrentDevice()
    {
        if (playerInput.currentControlScheme == "KeyboardAndMouse")
        {
            currentDevice = "Keyboard and Mouse";
        }
        else if (playerInput.devices[0].description.manufacturer == "Sony Interactive Entertainment")
        {
            currentDevice = "Playstation Controller";
        }
        else
        {
            currentDevice = playerInput.devices[0].displayName;
        }

        //if (!String.Equals(currentDevice, previousDevice))
        //{
        //    OnDeviceChange?.Invoke();
        //}

        //previousDevice = currentDevice;
    }

}
