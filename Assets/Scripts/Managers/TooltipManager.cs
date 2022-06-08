using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class TooltipManager : MonoBehaviour
{
    //public GameObject TooltipGroup;
    [Header("UI Panels")]
    public GameObject BasicTooltipUI;
    public GameObject ComboTooltipUI;

    [Header("Basic Tooltip")]
    public TextMeshProUGUI interaction;
    public Image icon;
    public TextMeshProUGUI action;

    [Header("Combo Tooltip")]
    public TextMeshProUGUI description;
    public Image comboIcon1;
    public Image comboIcon2;
    public TextMeshProUGUI comboAction;

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

    //public void ShowTooltip()
    //{
    //    TooltipUI.SetActive(true);
    //}

    public void HideTooltip()
    {
        BasicTooltipUI.SetActive(false);
        ComboTooltipUI.SetActive(false);
    }

    public void PopulateBasicTooltip(BasicTooltip tooltip)
    {
        interaction.text = tooltip.interaction;
        icon.sprite = tooltip.icon;
        action.text = tooltip.action;
        //ShowTooltip();
        BasicTooltipUI.SetActive(true);
    }

    public void PopulateComboTooltip(ComboTooltip tooltip)
    {
        description.text = tooltip.description;
        comboIcon1.sprite = tooltip.comboIcon1;
        comboIcon2.sprite = tooltip.comboIcon2;
        comboAction.text = tooltip.comboAction;
        //ShowTooltip();
        ComboTooltipUI.SetActive(true);
    }
}
