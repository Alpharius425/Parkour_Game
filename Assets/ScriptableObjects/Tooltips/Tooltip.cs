using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//[CreateAssetMenu(fileName = "New Tooltip", menuName = "Tooltip")]
public class Tooltip : ScriptableObject
{
    //public InputActionReference actionReference;
    //public InputActionReference actionReference2;

    public virtual bool IsBasicTooltip()
    {
        return false;
    }

    public virtual bool IsComboTooltip()
    {
        return false;
    }

    public virtual bool IsCompositeTooltip()
    {
        return false;
    }
}
