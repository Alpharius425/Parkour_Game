using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateObjectToggle : MonoBehaviour
{
    [SerializeField] bool deactivateObject;
    [SerializeField] GameObject[] targetObjects;

    public void Toggle()
    {
        for (int i = 0; i < targetObjects.Length; i++)
        {
            if(deactivateObject)
            {
                targetObjects[i].SetActive(false);
            }
            else
            {
                targetObjects[i].SetActive(true);
            }
        }
    }
}
