using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager instance;

    [SerializeField] bool crouchingToggled;
    [SerializeField] bool sprintingToggled;
    [SerializeField] float mouseSensitivity;
    public Slider mouseSensitivitySlider;

    [SerializeField] float volume;
    public Slider volumeSlider;

    GameObject player;
    public GameObject settingsHolder;
    void UpdateSettings()
    {
        if(player != null)
        {
            player.GetComponent<PlayerInputDetector>().crouchToggled = crouchingToggled;
            player.GetComponent<PlayerInputDetector>().sprintToggled = sprintingToggled;

            player.GetComponentInChildren<CameraControl>().mouseSensitivity = mouseSensitivity;
        }
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        //settingsHolder.transform.SetParent(GameObject.FindGameObjectWithTag("UI").transform);

        if (GameObject.FindGameObjectWithTag("Player"))
        {
            player = GameObject.FindGameObjectWithTag("Player");
            UpdateSettings();
        }
        else
        {
            player = null;
        }

        //if(PauseMenu.instance != null)
        //{
        //    PauseMenu.instance.settingsMenu = gameObject;
        //}
    }

    public void ApplySettings()
    {
        UpdateSettings();
    }

    public void ResetSettings()
    {
        if(player != null)
        {
            crouchingToggled = player.GetComponent<PlayerInputDetector>().crouchToggled;
            sprintingToggled = player.GetComponent<PlayerInputDetector>().sprintToggled;

            mouseSensitivity = player.GetComponentInChildren<CameraControl>().mouseSensitivity;
        }
    }

    public void ToggleCrouch()
    {
        crouchingToggled = !crouchingToggled;
    }

    public void ToggleSprint()
    {
        sprintingToggled = !sprintingToggled;
    }
}
