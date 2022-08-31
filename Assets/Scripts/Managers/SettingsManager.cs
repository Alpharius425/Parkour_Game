using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("Crouch/sprint button text displays")]
    public TextMeshProUGUI crouchButtonText;
    public TextMeshProUGUI sprintButtonText;

    public static bool crouchToggledSetting;
    public static bool sprintToggledSetting;

    [Header("Sliders")]
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public Slider lookSensitivitySlider;

    [Header("Mixers")]
    public AudioMixer musicMixer;
    public AudioMixer sfxMixer;

    [Header("PlayerControls actions")]
    public InputActionAsset actions;

    private void Start()
    {
        LoadCrouchAndSprintPreferences();
        LoadSliderPreferences();
        LoadRebinds();
    }

    private void OnDisable()
    {
        // save rebinds 
        var rebinds = actions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds", rebinds);
    }

    public void LoadCrouchAndSprintPreferences()
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

    public void LoadSliderPreferences()
    {
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolumePref", 1);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolumePref", 1);
        lookSensitivitySlider.value = PlayerPrefs.GetFloat("LookSensitivityPref", 100);

        musicMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolumeSlider.value) * 20);
        sfxMixer.SetFloat("SFXVolume", Mathf.Log10(sfxVolumeSlider.value) * 20);
    }

    public void LoadRebinds()
    {
        var rebinds = PlayerPrefs.GetString("rebinds");
        if (!string.IsNullOrEmpty(rebinds))
        {
            actions.LoadBindingOverridesFromJson(rebinds);
        }
    }
}
