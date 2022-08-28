using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "New CompositeTooltip", menuName = "Composite Tooltip")]
public class CompositeTooltip : Tooltip
{
    public string interaction;
    public Sprite upIcon;
    public Sprite downIcon;
    public Sprite leftIcon;
    public Sprite rightIcon;
    public string actionDescription;
    public InputActionReference actionReference;

    public override bool IsBasicTooltip()
    {
        return false;
    }

    public override bool IsComboTooltip()
    {
        return false;
    }

    public override bool IsCompositeTooltip()
    {
        return true;
    }
}
