using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SwitchControlSchemeRebinding : MonoBehaviour
{
    public PlayerInput playerInput;
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
            }
            else if (playerInput.currentControlScheme == "Gamepad")
            {
                KeyboardAndMouseRebindingUI.SetActive(false);
                GamepadRebindingUI.SetActive(true);
            }
        }
    }
}
