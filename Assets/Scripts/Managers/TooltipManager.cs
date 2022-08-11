using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class TooltipManager : MonoBehaviour
{
    //public GameObject TooltipGroup;
    [Header("UI Panels")]
    public GameObject BasicTooltipUI;
    public GameObject ComboTooltipUI;

    [Header("Basic Tooltip")]
    public TextMeshProUGUI interaction;
    public Image icon;
    public TextMeshProUGUI action;

    [Header("Combo Tooltip")]
    public TextMeshProUGUI description;
    public Image comboIcon1;
    public Image comboIcon2;
    public TextMeshProUGUI comboAction;

    public static TooltipManager instance;

    private void Awake()
    {
        instance = this;
    }

    //void OnEnable()
    //{
    //    KeybindsManager.OnDeviceChange += ChangeActiveSpriteType; 
    //}
    //void OnDisable()
    //{
    //    KeybindsManager.OnDeviceChange -= ChangeActiveSpriteType; 
    //}

    private void Update()
    {
        if (PauseMenu.instance.Paused)
        {
            HideTooltip();
        }
    }

    public void HideTooltip()
    {
        BasicTooltipUI.SetActive(false);
        ComboTooltipUI.SetActive(false);
    }

    public void PopulateBasicTooltip(BasicTooltip tooltip)
    {
        interaction.text = tooltip.interaction;
        icon.sprite = tooltip.icon;
        action.text = tooltip.actionDescription;
        BasicTooltipUI.SetActive(true);
    }

    public void PopulateComboTooltip(ComboTooltip tooltip)
    {
        description.text = tooltip.description;
        comboIcon1.sprite = tooltip.comboIcon1;
        comboIcon2.sprite = tooltip.comboIcon2;
        comboAction.text = tooltip.comboAction;
        ComboTooltipUI.SetActive(true);
    }
    //public void ChangeActiveSpriteType() 
    //{
        
    //}
    //public void GetSprite(string controlPath)
    //{
    //    // call different sprite method based on what type is active 
    //}

    public Sprite GetKeyboardAndMouseSprite(string controlPath)
    {
        switch (controlPath)
        {
            // KEYBOARD: numbers
            case "<Keyboard>/0": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/0-Filled@4x");
            case "<Keyboard>/1": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/1-Filled@4x");
            case "<Keyboard>/2": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/2-Filled@4x");
            case "<Keyboard>/3": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/3-Filled@4x");
            case "<Keyboard>/4": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/4-Filled@4x");
            case "<Keyboard>/5": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/5-Filled@4x");
            case "<Keyboard>/6": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/6-Filled@4x");
            case "<Keyboard>/7": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/7-Filled@4x");
            case "<Keyboard>/8": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/8-Filled@4x");
            case "<Keyboard>/9": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/9-Filled@4x");

            // number pad
            case "<Keyboard>/numpad0": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/0-Filled@4x");
            case "<Keyboard>/numpad1": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/1-Filled@4x");
            case "<Keyboard>/numpad2": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/2-Filled@4x");
            case "<Keyboard>/numpad3": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/3-Filled@4x");
            case "<Keyboard>/numpad4": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/4-Filled@4x");
            case "<Keyboard>/numpad5": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/5-Filled@4x");
            case "<Keyboard>/numpad6": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/6-Filled@4x");
            case "<Keyboard>/numpad7": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/7-Filled@4x");
            case "<Keyboard>/numpad8": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/8-Filled@4x");
            case "<Keyboard>/numpad9": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/9-Filled@4x");

            // letters
            case "<Keyboard>/a": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/A-Key-Filled@4x");
            case "<Keyboard>/b": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/B-Key-Filled@4x");
            case "<Keyboard>/c": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/C-Key-Filled@4x");
            case "<Keyboard>/d": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/D-Key-Filled@4x");
            case "<Keyboard>/e": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/E-Key-Filled@4x");
            case "<Keyboard>/f": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/F-Key-Filled@4x");
            //functions?
            case "<Keyboard>/g": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/G-Key-Filled@4x");
            case "<Keyboard>/h": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/H-Key-Filled@4x");
            case "<Keyboard>/i": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/I-Key-Filled@4x");
            case "<Keyboard>/j": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/J-Key-Filled@4x");
            case "<Keyboard>/k": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/K-Key-Filled@4x");
            case "<Keyboard>/l": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/L-Key-Filled@4x");
            case "<Keyboard>/m": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/M-Key-Filled@4x");
            case "<Keyboard>/n": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/N-Key-Filled@4x");
            case "<Keyboard>/o": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/O-Key-Filled@4x");
            case "<Keyboard>/p": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/P-Key-Filled@4x");
            case "<Keyboard>/q": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/Q-Key-Filled@4x");
            case "<Keyboard>/r": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/R-Key-Filled@4x");
            case "<Keyboard>/s": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/S-Key-Filled@4x");
            case "<Keyboard>/t": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/T-Key-Filled@4x");
            case "<Keyboard>/u": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/U-Key-Filled@4x");
            case "<Keyboard>/v": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/V-Key-Filled@4x");
            case "<Keyboard>/w": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/W-Key-Filled@4x");
            case "<Keyboard>/x": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/H-Key-Filled@4x");
            case "<Keyboard>/y": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/Y-Key-Filled@4x");
            case "<Keyboard>/z": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/Z-Key-Filled@4x");
            
            // miscellaneous 
            case "<Keyboard>/leftAlt": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/Alt-Filled@4x");
            case "<Keyboard>/rightAlt": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/Alt-Filled@4x");
            case "<Keyboard>/downArrow": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/Arrow-Down-Filled@4x");
            case "<Keyboard>/leftArrow": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/Arrow-Left-Filled@4x");
            case "<Keyboard>/rightArrow": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/Arrow-Right-Filled@4x");
            case "<Keyboard>/upArrow": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/Arrow-Up-Filled@4x");
            case "<Keyboard>/backslash": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/Backslash-Filled@4x");//check
            case "<Keyboard>/backspace": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/Backspace-Filled@4x");
            case "<Keyboard>/rightBracket": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/Bracket-Right-Key-Filled@4x");
            case "<Keyboard>/leftBracket": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/Bracket-Left-Key-Filled@4x");
            case "<Keyboard>/capsLock": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/CapsLock-Filled@4x");
            case "<Keyboard>/leftCtrl": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/Ctrl-Filled@4x");
            case "<Keyboard>/rightCtrl": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/Ctrl-Filled@4x");
            case "<Keyboard>/delete": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/Delete-Filled@4x");
            case "<Keyboard>/end": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/End-Filled@4x"); //check
            case "<Keyboard>/equals": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/Equals-Filled@4x");
            case "<Keyboard>/escape": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/Escape-Filled@4x");
            case "<Keyboard>/backquote": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/GraveAccent-Filled@4x");
            case "<Keyboard>/home": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/Home-Filled@4x"); //check
            case "<Keyboard>/insert": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/Insert-Filled@4x"); //check
            case "<Keyboard>/minus": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/Minus-Filled@4x");
            case "<Keyboard>/pageDown": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/PageDown-Filled@4x"); //check
            case "<Keyboard>/pageUp": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/PageUp-Filled@4x"); //check 
            case "<Keyboard>/numpadPlus": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/Plus-Filled@4x"); //check 
            case "<Keyboard>/period": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/Punctuation-Filled@4x");
            case "<Keyboard>/semicolon": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/Semicolon-Filled@4x");
            case "<Keyboard>/leftShift": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/Shift-Filled@4x");
            case "<Keyboard>/rightShift": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/Shift-Filled@4x");
            case "<Keyboard>/quote": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/SingleQuotation-Filled@4x");
            case "<Keyboard>/slash": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/Slash-Filled@4x");
            case "<Keyboard>/space": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/Space-Filled@4x");//check
            case "<Keyboard>/tab": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/Tab-Filled@4x");

            // MOUSE
            case "<Mouse>/leftButton": return Resources.Load<Sprite>("Icons/Keyboard And Mouse/Mouse PNG/Filled/MouseLeftButton-Filled@4x"); //check
            case "<Mouse>/rightButton": return Resources.Load<Sprite>("Icons/Keyboard and Mouse/Keyboard PNG/Filled/MouseRightButton-Filled@4x"); //check

            // scroll wheel? 
            // any side buttons? 

        }
        return null;

    }

    public Sprite GetPlaystationSprite(string controlPath)
    {
        Debug.Log(controlPath);

        switch (controlPath)
        {
            // triggers, bumpers
            case "<Gamepad>/rightTrigger": return Resources.Load<Sprite>("Icons/Playstation/PS4_R2");
            case "<Gamepad>/leftTrigger": return Resources.Load<Sprite>("Icons/Playstation/PS4_L2");
            case "<Gamepad>/rightBumper": return Resources.Load<Sprite>("Icons/Playstation/PS4_R1"); //check
            case "<Gamepad>/leftBumper": return Resources.Load<Sprite>("Icons/Playstation/PS4_L1"); //check

            // buttons 
            case "<Gamepad>/buttonSouth": return Resources.Load<Sprite>("Icons/Playstation/PS4_Cross");
            case "<Gamepad>/buttonEast": return Resources.Load<Sprite>("Icons/Playstation/PS4_Circle");
            case "<Gamepad>/buttonWest": return Resources.Load<Sprite>("Icons/Playstation/PS4_Square");
            case "<Gamepad>/buttonNorth": return Resources.Load<Sprite>("Icons/Playstation/PS4_Triangle");

            // joysticks
            case "<Gamepad>/leftStick": return Resources.Load<Sprite>("Icons/Playstation/PS4_Left_Stick");
            case "<Gamepad>/rightStick": return Resources.Load<Sprite>("Icons/Playstation/PS4_Right_Stick");
            
            // d-pad 
            // option, share button 
        }
        return null;
    }

    public Sprite GetXboxSprite(string controlPath)
    {
        Debug.Log(controlPath);

        switch (controlPath)
        {
            case "<Gamepad>/rightTrigger": return Resources.Load<Sprite>("Icons/Xbox/XboxOne_RT");
            case "<Gamepad>/leftTrigger": return Resources.Load<Sprite>("Icons/Xbox/XboxOne_LT");
            case "<Gamepad>/rightBumper": return Resources.Load<Sprite>("Icons/Xbox/XboxOne_RB"); //check
            case "<Gamepad>/leftBumper": return Resources.Load<Sprite>("Icons/Xbox/XboxOne_LB"); //check 

            case "<Gamepad>/buttonSouth": return Resources.Load<Sprite>("Icons/Xbox/XboxOne_A");
            case "<Gamepad>/buttonEast": return Resources.Load<Sprite>("Icons/Xbox/XboxOne_B");
            case "<Gamepad>/buttonWest": return Resources.Load<Sprite>("Icons/Xbox/XboxOne_X");
            case "<Gamepad>/buttonNorth": return Resources.Load<Sprite>("Icons/Xbox/Xbox_Y");

            // joysticks
            case "<Gamepad>/leftStick": return Resources.Load<Sprite>("Icons/Xbox/XboxOne_Left_Stick");
            case "<Gamepad>/rightStick": return Resources.Load<Sprite>("Icons/Xbox/XboxOne_Right_Stick");

            // d-pad 
            // option, share button 
        }
        return null;
    }

}
