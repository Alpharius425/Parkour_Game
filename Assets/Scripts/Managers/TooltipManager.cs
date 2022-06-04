using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class TooltipManager : MonoBehaviour
{
    public GameObject TooltipUI;
    public TextMeshProUGUI description;
    public Image icon;

    public static TooltipManager instance;
    //public PlayerInput playerInput;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (PauseMenu.instance.Paused)
        {
            HideTooltip();
        }
    }

    public void ShowTooltip()
    {
        TooltipUI.SetActive(true);
    }

    public void HideTooltip()
    {
        TooltipUI.SetActive(false);
    }

    public void PopulateTooltip(Tooltip tooltip)
    {
        description.text = tooltip.description;
        icon.sprite = tooltip.icon;
        ShowTooltip();
    }
}
