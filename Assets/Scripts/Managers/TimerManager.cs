using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    public static TimerManager instance;

    public float curTime;
    [SerializeField] bool countingUp;
    [SerializeField] bool startTimerOnStart;
    [SerializeField] bool overTime;

    [SerializeField] private TextMeshProUGUI timerText;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if(startTimerOnStart)
        {
            countingUp = true;
        }

        overTime = false;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(countingUp) // once we start the timer update the time and the timer text
        {
            curTime += Time.deltaTime;
            timerText.SetText(curTime.ToString("f2"));
        }

        if(curTime > GameManager.Instance.levelGoalTime && overTime != true)
        {
            overTime = true;
            timerText.color = Color.red;
        }
    }

    public void ResetTime()
    {
        curTime = 0;
    }

    public void ChangeTime(float changedTime)
    {
        curTime += changedTime;
    }

    public void StartTimer()
    {
        countingUp = true;
    }

    public void StopTimer()
    {
        countingUp = false;
    }
}
