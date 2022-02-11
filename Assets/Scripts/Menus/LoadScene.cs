using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadScene : MonoBehaviour
{
    public static LoadScene instance;
    [SerializeField] int level;

    private void Awake()
    {
        instance = this;
    }

    public void LoadByIndex()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(level);
    }
}
