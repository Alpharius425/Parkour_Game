using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class SettingsMenu : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject SettingsMenuUI;
    public GameObject KeybindsMenuUI;

    [Header("Button text displays")]
    public TextMeshProUGUI crouchButtonText;
    public TextMeshProUGUI sprintButtonText;

    [Header("Toggle settings")]
    public bool crouchToggledSetting;
    public bool sprintToggledSetting;

    private bool settingsMenuActive = false;
    private bool keybindsMenuActive = false;

    public static SettingsMenu instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        SettingsMenuUI.SetActive(false);
        KeybindsMenuUI.SetActive(false);
        GetCrouchAndSprintPreferences();
    }

    public void ToggleSettingsMenu()
    {
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
    public void ToggleKeybindsMenu()
    {
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

    public void GetCrouchAndSprintPreferences()
    {
        // get player preferences or set both to toggle detection by default 
        int crouchToggledInt = PlayerPrefs.GetInt("CrouchTogglePref", 1); 
        int sprintToggledInt = PlayerPrefs.GetInt("SprintTogglePref", 1);

        if (crouchToggledInt == 0)
        {
            crouchButtonText.text = "Hold";
            crouchToggledSetting = false;
        }
        else if (crouchToggledInt == 1)
        {
            crouchButtonText.text = "Tap";
            crouchToggledSetting = true;
        }

        if (sprintToggledInt == 0)
        {
            sprintButtonText.text = "Hold";
            sprintToggledSetting = false;
        }
        else if (sprintToggledInt == 1)
        {
            sprintButtonText.text = "Tap";
            sprintToggledSetting = true;
        }

    }

    public void ToggleCrouchSetting()
    {
        if (crouchToggledSetting == true)
        {
            PlayerPrefs.SetInt("CrouchTogglePref", 0);
            crouchButtonText.text = "Hold";
            crouchToggledSetting = false;
        }
        else if (crouchToggledSetting == false)
        {
            PlayerPrefs.SetInt("CrouchTogglePref", 1);
            crouchButtonText.text = "Tap";
            crouchToggledSetting = true;
        }
    }
    public void ToggleSprintSetting()
    {
        if (sprintToggledSetting == true)
        {
            PlayerPrefs.SetInt("SprintTogglePref", 0);
            sprintButtonText.text = "Hold";
            sprintToggledSetting = false;

        }
        else if (sprintToggledSetting == false)
        {
            PlayerPrefs.SetInt("SprintTogglePref", 1);
            sprintButtonText.text = "Tap";
            sprintToggledSetting = true;
        }
    }

}
