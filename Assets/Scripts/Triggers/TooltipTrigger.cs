using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TooltipTrigger : MonoBehaviour
{
    public Tooltip keyboardAndMouseTooltip;
    public Tooltip gamepadTooltip;
    public bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !triggered)
        {
            TooltipManager.instance.DisplayTooltip(keyboardAndMouseTooltip);
            triggered = true;
        }
    }


}
