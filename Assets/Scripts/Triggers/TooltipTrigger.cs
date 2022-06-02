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
    public bool triggered = false;
    public bool keyboardAndMouseActive;

    private PlayerInput input;

    private void Awake()
    {
         input = TooltipManager.instance.input;
    }

    //private void Update()
    //{
        
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !triggered)
        {
            // determine whether to show keyboard/mouse or gamepad tooltip 
            if (input.currentControlScheme == "KeyboardAndMouse")
            {
                TooltipManager.instance.DisplayTooltip(keyboardAndMouseTooltip);
            } 
            else if (input.currentControlScheme == "Gamepad")
            {
                TooltipManager.instance.DisplayTooltip(gamepadTooltip);
            }
            
            triggered = true;
        }
    }


}
