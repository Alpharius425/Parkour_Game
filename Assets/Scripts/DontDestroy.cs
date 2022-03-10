using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    GameObject[] savedObjects;
    private void Awake()
    {
        savedObjects = GameObject.FindGameObjectsWithTag("DontDestroy");

        for (int i = 0; i < savedObjects.Length; i++)
        {
            DontDestroyOnLoad(savedObjects[i]);
            Debug.Log("Saved " + savedObjects[i].name);
        }
    }
}
