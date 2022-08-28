using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class SwitchControlSchemeRebinding : MonoBehaviour
{
    public TextMeshProUGUI DeviceLabel;
    public GameObject KeyboardAndMouseRebindingUI;
    public GameObject GamepadRebindingUI;
    public GameObject RebindingOverlay;

    private void Update()
    {
        if (!RebindingOverlay.activeInHierarchy)
        {
            DeviceLabel.text = DeviceManager.currentDevice;
            if (DeviceManager.currentDevice == "Keyboard and Mouse")
            {
                KeyboardAndMouseRebindingUI.SetActive(true);
                GamepadRebindingUI.SetActive(false);
            } 
            else
            {
                KeyboardAndMouseRebindingUI.SetActive(false);
                GamepadRebindingUI.SetActive(true);
            }
        }
    }
}
