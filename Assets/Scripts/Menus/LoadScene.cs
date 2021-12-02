using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadScene : MonoBehaviour
{
    public static LoadScene instance;

    private void Awake()
    {
        instance = this;
    }

    public void LoadByIndex(int Level)
    {
        SceneManager.LoadScene(Level);
    }
}
