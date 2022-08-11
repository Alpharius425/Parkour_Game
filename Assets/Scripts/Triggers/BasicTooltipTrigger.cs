using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class BasicTooltipTrigger : MonoBehaviour
{
    public BasicTooltip keyboardAndMouseTooltip;
    public BasicTooltip gamepadTooltip;

    //private PlayerInput playerInput;

    //private void Awake()
    //{
    //    playerInput = FindObjectOfType<PlayerInput>();
    //}

    // called once per physics frame 
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (KeybindsManager.currentDevice == "Keyboard and Mouse")
            {
                var binding = keyboardAndMouseTooltip.actionReference.action.bindings[0];
                if (binding.isComposite)
                {
                    binding = keyboardAndMouseTooltip.actionReference.action.bindings[1];
                }
                Debug.Log(binding.effectivePath);
                keyboardAndMouseTooltip.icon = TooltipManager.instance.GetKeyboardAndMouseSprite(binding.effectivePath);
                TooltipManager.instance.PopulateBasicTooltip(keyboardAndMouseTooltip);

            }
            else if (KeybindsManager.currentDevice == "Playstation Controller")
            {
                var binding = gamepadTooltip.actionReference.action.bindings[1];
                Debug.Log(binding.effectivePath);
                gamepadTooltip.icon = TooltipManager.instance.GetPlaystationSprite(binding.effectivePath);
                TooltipManager.instance.PopulateBasicTooltip(gamepadTooltip);
            } 
            else
            {
                var binding = gamepadTooltip.actionReference.action.bindings[0];
                if (binding.isComposite)
                {
                    binding = gamepadTooltip.actionReference.action.bindings[5];
                }
                else
                {
                    binding = gamepadTooltip.actionReference.action.bindings[1];
                }
                Debug.Log(binding.effectivePath);
                gamepadTooltip.icon = TooltipManager.instance.GetXboxSprite(binding.effectivePath);
                TooltipManager.instance.PopulateBasicTooltip(gamepadTooltip);
            }
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
