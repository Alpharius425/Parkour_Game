using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class SwitchControlSchemeRebinding : MonoBehaviour
{
    private PlayerInput playerInput;
    public TextMeshProUGUI DeviceLabel;
    public GameObject KeyboardAndMouseRebindingUI;
    public GameObject GamepadRebindingUI;
    public GameObject RebindingOverlay;

    private void Awake()
    {
        playerInput = FindObjectOfType<PlayerInput>();
    }

    private void Update()
    {
        if (!RebindingOverlay.activeInHierarchy)
        {
            // determine whether to show keyboard/mouse or gamepad rebinding panel 
            if (playerInput.currentControlScheme == "KeyboardAndMouse")
            {
                KeyboardAndMouseRebindingUI.SetActive(true);
                GamepadRebindingUI.SetActive(false);
                DeviceLabel.text = "Keyboard and Mouse";
            }
            else if (playerInput.currentControlScheme == "Gamepad")
            {
                KeyboardAndMouseRebindingUI.SetActive(false);
                GamepadRebindingUI.SetActive(true);

                // device label changes based on the type of controller being used 
                if (playerInput.devices[0].description.manufacturer == "Sony Interactive Entertainment")
                {
                    DeviceLabel.text = "Playstation Controller";
                }
                else 
                {
                    DeviceLabel.text = playerInput.devices[0].displayName;
                }

            }
        }
    }
}
