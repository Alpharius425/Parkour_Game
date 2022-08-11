using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "New BasicTooltip", menuName = "Basic Tooltip")]
public class BasicTooltip : ScriptableObject
{
    public string interaction;
    public Sprite icon;
    public string actionDescription;
    public InputActionReference actionReference;
}
