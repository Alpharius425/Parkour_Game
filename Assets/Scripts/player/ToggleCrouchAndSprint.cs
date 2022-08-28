using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ToggleCrouchAndSprint : MonoBehaviour
{
    [Header("Button text displays")]
    public TextMeshProUGUI crouchButtonText;
    public TextMeshProUGUI sprintButtonText;

    public delegate void SettingChange();
    public static event SettingChange OnSettingChange;

    public void ToggleCrouchSetting()
    {
        if (SettingsManager.crouchToggledSetting == true)
        {
            PlayerPrefs.SetInt("CrouchTogglePref", 0);
            crouchButtonText.text = "Hold";
            SettingsManager.crouchToggledSetting = false;
        }
        else if (SettingsManager.crouchToggledSetting == false)
        {
            PlayerPrefs.SetInt("CrouchTogglePref", 1);
            crouchButtonText.text = "Tap";
            SettingsManager.crouchToggledSetting = true;
        }

        OnSettingChange?.Invoke();
    }
    public void ToggleSprintSetting()
    {
        if (SettingsManager.sprintToggledSetting == true)
        {
            PlayerPrefs.SetInt("SprintTogglePref", 0);
            sprintButtonText.text = "Hold";
            SettingsManager.sprintToggledSetting = false;
        }
        else if (SettingsManager.sprintToggledSetting == false)
        {
            PlayerPrefs.SetInt("SprintTogglePref", 1);
            sprintButtonText.text = "Tap";
            SettingsManager.sprintToggledSetting = true;
        }

        OnSettingChange?.Invoke();
    }
}
