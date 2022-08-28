using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class ToggleSettingsAndKeybindsMenu : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject SettingsMenuPanel;
    public GameObject KeybindsMenuGroup;

    private bool settingsMenuActive = false;
    private bool keybindsMenuActive = false;

    //Start is called before the first frame update
    void Start()
    {
        SettingsMenuPanel.SetActive(false);
        KeybindsMenuGroup.SetActive(false);
    }

    public void ToggleSettingsMenu()
    {
        if (settingsMenuActive == false)
        {
            settingsMenuActive = true;
            SettingsMenuPanel.SetActive(true);

            if (keybindsMenuActive == true)
            {
                keybindsMenuActive = false;
                KeybindsMenuGroup.SetActive(false);
            }
        } 
        else
        {
            settingsMenuActive = false;
            SettingsMenuPanel.SetActive(false);
        }
    }
    public void ToggleKeybindsMenu()
    {
        if (keybindsMenuActive == false)
        {
            keybindsMenuActive = true;
            KeybindsMenuGroup.SetActive(true);

            settingsMenuActive = false;
            SettingsMenuPanel.SetActive(false);
        }
        else
        {
            keybindsMenuActive = false;
            KeybindsMenuGroup.SetActive(false);
        }
    }

}
