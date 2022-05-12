using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public GameObject SettingsMenuUI;
    public GameObject KeybindsMenuUI;
    public bool settingsMenuActive = false;
    public bool keybindsMenuActive = false;
    public static SettingsMenu instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("settings menu start");
        SettingsMenuUI.SetActive(false);
        KeybindsMenuUI.SetActive(false);
    }

    public void SettingsMenuToggle()
    {
        Debug.Log("settings menu toggled");
        if (settingsMenuActive == false)
        {
            settingsMenuActive = true;
            SettingsMenuUI.SetActive(true);

            if (keybindsMenuActive == true)
            {
                keybindsMenuActive = false;
                KeybindsMenuUI.SetActive(false);
            }
        } 
        else
        {
            settingsMenuActive = false;
            SettingsMenuUI.SetActive(false);
        }
    }
    public void KeybindsMenuToggle()
    {
        Debug.Log("keybinds menu toggled");
        if (keybindsMenuActive == false)
        {
            keybindsMenuActive = true;
            KeybindsMenuUI.SetActive(true);

            settingsMenuActive = false;
            SettingsMenuUI.SetActive(false);
        }
        else
        {
            keybindsMenuActive = false;
            KeybindsMenuUI.SetActive(false);
        }
    }
}
