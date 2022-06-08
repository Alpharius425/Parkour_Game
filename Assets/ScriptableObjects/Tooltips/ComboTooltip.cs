using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ComboTooltip", menuName = "Combo Tooltip")]
public class ComboTooltip : ScriptableObject
{
    public string description;
    public Sprite comboIcon1;
    public Sprite comboIcon2; 
    public string comboAction;
}
