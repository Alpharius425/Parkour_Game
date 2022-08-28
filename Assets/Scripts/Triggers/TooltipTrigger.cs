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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            DeviceManager.OnDeviceChange += RefreshTooltip;
            ToggleCrouchAndSprint.OnSettingChange += RefreshTooltip;
            RefreshTooltip();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            TooltipManager.instance.HideTooltip();
            DeviceManager.OnDeviceChange -= RefreshTooltip;
            ToggleCrouchAndSprint.OnSettingChange += RefreshTooltip;
        }
    }

    public void RefreshTooltip()
    {
        // 1) What device is currently being used 
        // 2) What kind of tooltip is used for this action on this device (basic, combo, composite)
        // 3) If basic, is this action crouch or sprint? Change text to 'tap' or 'hold' based on current setting
        // 4) Populate Tooltip ScriptableObject with correct information 

        if (DeviceManager.currentDevice == "Keyboard and Mouse")
        {
            if (keyboardAndMouseTooltip.IsBasicTooltip())
            {
                var basicTooltip = (BasicTooltip)keyboardAndMouseTooltip;
                var binding = basicTooltip.actionReference.action.bindings[0];
                basicTooltip.icon = TooltipManager.instance.GetKeyboardAndMouseSprite(binding.effectivePath);

                if (basicTooltip.actionReference.action.name == "Crouch")
                {
                    if (SettingsManager.crouchToggledSetting)
                    {
                        basicTooltip.interaction = "Tap";
                    }
                    else
                    {
                        basicTooltip.interaction = "Hold";
                    }
                }

                if (basicTooltip.actionReference.action.name == "Sprint")
                {
                    if (SettingsManager.sprintToggledSetting)
                    {
                        basicTooltip.interaction = "Tap";
                    }
                    else
                    {
                        basicTooltip.interaction = "Hold";
                    }
                }

                TooltipManager.instance.PopulateBasicTooltip(basicTooltip);
            } 
            else if (keyboardAndMouseTooltip.IsComboTooltip())
            {
                var comboTooltip = (ComboTooltip)keyboardAndMouseTooltip;

                var binding1 = comboTooltip.actionReference1.action.bindings[0];
                var binding2 = comboTooltip.actionReference2.action.bindings[0];

                comboTooltip.comboIcon1 = TooltipManager.instance.GetKeyboardAndMouseSprite(binding1.effectivePath);
                comboTooltip.comboIcon2 = TooltipManager.instance.GetKeyboardAndMouseSprite(binding2.effectivePath);

                TooltipManager.instance.PopulateComboTooltip(comboTooltip);

            } 
            else if (keyboardAndMouseTooltip.IsCompositeTooltip())
            {
                var compositeTooltip = (CompositeTooltip)keyboardAndMouseTooltip;

                var upBinding = compositeTooltip.actionReference.action.bindings[1];
                var downBinding = compositeTooltip.actionReference.action.bindings[2];
                var leftBinding = compositeTooltip.actionReference.action.bindings[3];
                var rightBinding = compositeTooltip.actionReference.action.bindings[4];

                compositeTooltip.upIcon = TooltipManager.instance.GetKeyboardAndMouseSprite(upBinding.effectivePath);
                compositeTooltip.downIcon = TooltipManager.instance.GetKeyboardAndMouseSprite(downBinding.effectivePath);
                compositeTooltip.leftIcon = TooltipManager.instance.GetKeyboardAndMouseSprite(leftBinding.effectivePath);
                compositeTooltip.rightIcon = TooltipManager.instance.GetKeyboardAndMouseSprite(rightBinding.effectivePath);

                TooltipManager.instance.PopulateCompositeTooltip(compositeTooltip);
            }

        }
        else if (DeviceManager.currentDevice == "Playstation Controller")
        {
            if (gamepadTooltip.IsBasicTooltip())
            {
                var basicTooltip = (BasicTooltip)gamepadTooltip;
                var binding = basicTooltip.actionReference.action.bindings[0];

                // InputActionReference docs: https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/api/UnityEngine.InputSystem.InputActionReference.html
                // ex. bindings array for player movement: ["2D Vector", "W", "A", "S", "D", "leftStick"]
                // "2D Vector" is a composite binding 
                if (binding.isComposite)
                {
                    binding = basicTooltip.actionReference.action.bindings[5];
                }
                else
                {
                    binding = basicTooltip.actionReference.action.bindings[1];
                }

                basicTooltip.icon = TooltipManager.instance.GetPlaystationSprite(binding.effectivePath);

                if (basicTooltip.actionReference.action.name == "Crouch")
                {
                    if (SettingsManager.crouchToggledSetting)
                    {
                        basicTooltip.interaction = "Tap";
                    }
                    else
                    {
                        basicTooltip.interaction = "Hold";
                    }
                }

                if (basicTooltip.actionReference.action.name == "Sprint")
                {
                    if (SettingsManager.sprintToggledSetting)
                    {
                        basicTooltip.interaction = "Tap";
                    }
                    else
                    {
                        basicTooltip.interaction = "Hold";
                    }
                }

                TooltipManager.instance.PopulateBasicTooltip(basicTooltip);
            }
            else if (gamepadTooltip.IsComboTooltip())
            {
                var comboTooltip = (ComboTooltip)gamepadTooltip;

                var binding1 = comboTooltip.actionReference1.action.bindings[1];
                var binding2 = comboTooltip.actionReference2.action.bindings[1];

                comboTooltip.comboIcon1 = TooltipManager.instance.GetPlaystationSprite(binding1.effectivePath);
                comboTooltip.comboIcon2 = TooltipManager.instance.GetPlaystationSprite(binding2.effectivePath);

                TooltipManager.instance.PopulateComboTooltip(comboTooltip);
            }
        } 
        else
        {
            if (gamepadTooltip.IsBasicTooltip())
            {
                var basicTooltip = (BasicTooltip)gamepadTooltip;
                var binding = basicTooltip.actionReference.action.bindings[0];

                if (binding.isComposite)
                {
                    binding = basicTooltip.actionReference.action.bindings[5];
                }
                else
                {
                    binding = basicTooltip.actionReference.action.bindings[1];
                }

                basicTooltip.icon = TooltipManager.instance.GetXboxSprite(binding.effectivePath);

                if (basicTooltip.actionReference.action.name == "Crouch")
                {
                    if (SettingsManager.crouchToggledSetting)
                    {
                        basicTooltip.interaction = "Tap";
                    }
                    else
                    {
                        basicTooltip.interaction = "Hold";
                    }
                }

                if (basicTooltip.actionReference.action.name == "Sprint")
                {
                    if (SettingsManager.sprintToggledSetting)
                    {
                        basicTooltip.interaction = "Tap";
                    }
                    else
                    {
                        basicTooltip.interaction = "Hold";
                    }
                }

                TooltipManager.instance.PopulateBasicTooltip(basicTooltip);
            }
            else if (gamepadTooltip.IsComboTooltip())
            {
                var comboTooltip = (ComboTooltip)gamepadTooltip;

                var binding1 = comboTooltip.actionReference1.action.bindings[1];
                var binding2 = comboTooltip.actionReference2.action.bindings[1];

                comboTooltip.comboIcon1 = TooltipManager.instance.GetXboxSprite(binding1.effectivePath);
                comboTooltip.comboIcon2 = TooltipManager.instance.GetXboxSprite(binding2.effectivePath);

                TooltipManager.instance.PopulateComboTooltip(comboTooltip);
            }
        }
    }

}
