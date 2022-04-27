using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public GameObject SettingsMenuUI;
    public bool settingsMenuActive;
    public static SettingsMenu instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("settings start method ran");
        SettingsMenuUI.SetActive(false);
    }

    public void SettingsMenuToggle()
    {
        Debug.Log("settings menu toggled");
        if (settingsMenuActive == false)
        {
            settingsMenuActive = true;
            SettingsMenuUI.SetActive(true);
        }
        else
        {
            settingsMenuActive = false;
            SettingsMenuUI.SetActive(false);
        }

    }
}
