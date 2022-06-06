using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstButtonSelected : MonoBehaviour
{
    public Button firstButtonSelected;

    public void OnEnable()
    {
        firstButtonSelected.Select();
    }
}
