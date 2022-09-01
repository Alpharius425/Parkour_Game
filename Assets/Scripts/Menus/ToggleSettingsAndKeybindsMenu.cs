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
    //public GameObject CreditsMenuPanel;

    public bool settingsMenuActive = false;
    private bool keybindsMenuActive = false;

    public delegate void SettingsOrKeybindsMenuToggled();
    public static event SettingsOrKeybindsMenuToggled OnSettingsMenuToggle;

    private void OnEnable()
    {
        //sstarts credits menu listener
        ToggleCreditsMenu.OnCreditsShown += HideSettingsAndKeybindsMenu;
    }

    private void OnDisable()
    {
        //remove credtis menu listener
        ToggleCreditsMenu.OnCreditsShown -= HideSettingsAndKeybindsMenu;
    }

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
            //CreditsMenuPanel.SetActive(false);
            settingsMenuActive = true;
            SettingsMenuPanel.SetActive(true);
            OnSettingsMenuToggle?.Invoke();

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
            //CreditsMenuPanel.SetActive(false);
            keybindsMenuActive = true;
            KeybindsMenuGroup.SetActive(true);

            settingsMenuActive = false;
            SettingsMenuPanel.SetActive(false);
            OnSettingsMenuToggle?.Invoke();
        }
        else
        {
            keybindsMenuActive = false;
            KeybindsMenuGroup.SetActive(false);
        }
    }

    public void HideSettingsAndKeybindsMenu()
    {
        settingsMenuActive = false;
        keybindsMenuActive = false;
        SettingsMenuPanel.SetActive(false);
        KeybindsMenuGroup.SetActive(false);
    }
}
