using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject PauseMenuUI;  //A panel parented to a canvas with buttons parented to it
    [SerializeField] GameObject restartButton;
    public bool Paused = false;
    public int currentLevel;
    [SerializeField] PlayerInputDetector myInputs;
    public static PauseMenu instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        PauseMenuUI.SetActive(false);   //turns off the pause menu at the start of the level
        currentLevel = SceneManager.GetActiveScene().buildIndex;
    }

    public void PauseToggle()
    {
        Debug.Log("pause toggled");
        if(Paused == true)
        {
            Paused = false;
            Time.timeScale = 1;             //makes time go at 100% of the usual rate so it move
            PauseMenuUI.SetActive(false);    //deactivates the pause menu
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Paused = true;
            Time.timeScale = 0;             //makes time go at 0% of the usual rate so it does not move
            PauseMenuUI.SetActive(true);    //activates the pause menu
            Cursor.lockState = CursorLockMode.None;
        }
        
        if (Paused)
        {
            myInputs.canInput = false;
        }
        else
        {
            myInputs.canInput = true;
        }
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(currentLevel);
    }
}
