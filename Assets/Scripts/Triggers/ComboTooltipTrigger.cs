using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ComboTooltipTrigger : MonoBehaviour
{
    public ComboTooltip keyboardAndMouseTooltip;
    public ComboTooltip gamepadTooltip;
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
                TooltipManager.instance.PopulateComboTooltip(keyboardAndMouseTooltip);
            }
            else if (playerInput.currentControlScheme == "Gamepad")
            {
                TooltipManager.instance.PopulateComboTooltip(gamepadTooltip);
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
