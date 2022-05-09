using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateItem : MonoBehaviour
{
    // script saves a list of gameobjects and activates them with a trigger collider

    GameObject[] activatedObjects;
    [SerializeField] bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if(!triggered)
        {
            foreach (GameObject item in activatedObjects)
            {
                item.SetActive(true);
            }

            triggered = true;
        }
    }
}
