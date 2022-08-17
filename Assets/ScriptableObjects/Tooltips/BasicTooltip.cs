using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "New BasicTooltip", menuName = "Basic Tooltip")]
public class BasicTooltip : Tooltip
{
    public string interaction;
    public Sprite icon;
    public string actionDescription;
    public InputActionReference actionReference;

    public override bool IsBasicTooltip()
    {
        return true;
    }

    public override bool IsComboTooltip()
    {
        return false;
    }

    public override bool IsCompositeTooltip()
    {
        return false;
    }
}
