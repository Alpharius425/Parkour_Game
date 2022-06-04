using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class TooltipTrigger : MonoBehaviour
{
    public Tooltip keyboardAndMouseTooltip;
    public Tooltip gamepadTooltip;
    //public bool triggered = false;

    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = FindObjectOfType<PlayerInput>();
    }

    // called once per physics frame 
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // determine whether to show keyboard/mouse or gamepad tooltip 
            if (playerInput.currentControlScheme == "KeyboardAndMouse")
            {
                TooltipManager.instance.PopulateTooltip(keyboardAndMouseTooltip);
            } 
            else if (playerInput.currentControlScheme == "Gamepad")
            {
                TooltipManager.instance.PopulateTooltip(gamepadTooltip);
            }
            
            //triggered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            TooltipManager.instance.HideTooltip();
        }
    }


}
