using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tooltip", menuName = "Tooltips")]
public class Tooltip : ScriptableObject
{
    public string description;
    public Sprite icon;
}
