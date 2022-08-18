using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetLookSensitivity : MonoBehaviour
{
    public Slider slider;

    public delegate void LookSensitivityChange();
    public static event LookSensitivityChange OnLookSensitivityChange;
    void Start()
    {
        slider.value = PlayerPrefs.GetFloat("LookSensitivityPref", 100);
    }
    public void SetLevel(float sliderValue)
    {
        PlayerPrefs.SetFloat("LookSensitivityPref", sliderValue);
        OnLookSensitivityChange?.Invoke();
    }
}
