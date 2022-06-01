using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TooltipManager : MonoBehaviour
{
    public GameObject toolTipPanel;
    public TextMeshProUGUI description;
    public Image icon;

    public static TooltipManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void DisplayTooltip(Tooltip tooltip)
    {
        toolTipPanel.SetActive(true);
        description.text = tooltip.description;
        icon.sprite = tooltip.icon;
        //PopulateTooltip(tooltip);
    }

    //public void PopulateTooltip(Tooltip tooltip)
    //{
        //description.text = tooltip.description;
        //icon.sprite = tooltip.icon;
    //}
}
