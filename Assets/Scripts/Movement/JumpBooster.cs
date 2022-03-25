using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBooster : MonoBehaviour
{
    [SerializeField] private string Notes;

    [Space(5)]
    public float jumpMultiplier;
    
    [Space(5)]
    public float airSpeed;

    [Space(5)]
    public bool useAirSpeedBoost;
    public float airSpeedBoost;

    private void Start() {
        if (useAirSpeedBoost) {
            airSpeed = airSpeedBoost;
        }
    }
}
