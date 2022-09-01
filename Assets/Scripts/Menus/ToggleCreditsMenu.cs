using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleCreditsMenu : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject CreditsMenuPanel;
    //public GameObject SettingsMenuPanel;
    //public GameObject KeybindsMenuGroup;

    private bool creditsMenuActive = false;

    public delegate void CreditsMenuShown();
    public static event CreditsMenuShown OnCreditsShown;

    private void OnEnable()
    {
        // starts settings and keybind menu listner
        ToggleSettingsAndKeybindsMenu.OnSettingsMenuToggle += HideCreditsMenu;
    }

    private void OnDisable()
    {
        // stops settings and keybind menu listner
        ToggleSettingsAndKeybindsMenu.OnSettingsMenuToggle -= HideCreditsMenu;
    }

    // Start is called before the first frame update
    void Start()
    {
        CreditsMenuPanel.SetActive(false);
    }

    public void ToggleMenu()
    {
        if (creditsMenuActive == false)
        {
            //SettingsMenuPanel.SetActive(false);
            //KeybindsMenuGroup.SetActive(false);
            creditsMenuActive = true;
            CreditsMenuPanel.SetActive(true);
            OnCreditsShown?.Invoke();
        }
        else
        {
            creditsMenuActive = false;
            CreditsMenuPanel.SetActive(false);
        }
    }

    public void HideCreditsMenu()
    {
        creditsMenuActive = false;
        CreditsMenuPanel.SetActive(false);
    }
}
