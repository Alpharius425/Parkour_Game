using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "New ComboTooltip", menuName = "Combo Tooltip")]
public class ComboTooltip : Tooltip
{
    public string description;
    public Sprite comboIcon1;
    public Sprite comboIcon2; 
    public string comboAction;
    public InputActionReference actionReference1;
    public InputActionReference actionReference2;

    public override bool IsBasicTooltip()
    {
        return false;
    }

    public override bool IsComboTooltip()
    {
        return true;
    }

    public override bool IsCompositeTooltip()
    {
        return false;
    }
}
