using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // for now hold the level variables like how much time it takes and what the score multiplier is
    [SerializeField] float levelGoalTime;
    [SerializeField] float levelMultiplier;

    [SerializeField] GameObject endLevelScreen;
    [SerializeField] TextMeshProUGUI levelGoalTimeText;
    [SerializeField] TextMeshProUGUI levelMultiplierText;
    [SerializeField] TextMeshProUGUI playerTimeText;
    [SerializeField] TextMeshProUGUI totalLevelRewardText;

    private void Awake()
    {
        Instance = this;
        endLevelScreen.SetActive(false); // turns off the end level screen just in case
    }

    public void EndLevel() // calculates our score and ends the timer
    {
        CalculateLevelScore(TimerManager.instance.curTime, levelGoalTime, levelMultiplier);
        TimerManager.instance.ResetTime();

    }

    // calculates how much money the player should get when they finish a level
    public void CalculateLevelScore(float playerTime, float averageLevelTime, float scoreMultiplier)
    {
        float initialScore = (averageLevelTime / playerTime) * scoreMultiplier; // calculates the score from the player's time compared to the level time and score multiplier

        int newScore = Mathf.RoundToInt(initialScore); // rounds our score to the nearest int

        if (newScore < initialScore) // checks if we rounded down and adds one if we did.
        {
            newScore += 1;
        }
        
        // updates the money manager
        MoneyManager.Instance.AddMoney(newScore);

        // updates the UI for our end level menu
        endLevelScreen.SetActive(true);
        levelGoalTimeText.SetText("Level goal time " + levelGoalTime.ToString());
        playerTimeText.SetText("Player time " + TimerManager.instance.curTime.ToString("F2"));
        levelMultiplierText.SetText("Level multiplier " + levelMultiplier.ToString());
        totalLevelRewardText.SetText("Total money earned " + newScore);

        // if we don't beat the level time have the player time turn red if we beat it turn green
        if (playerTime > levelGoalTime)
        {
            playerTimeText.color = Color.red;
        }
        else
        {
            playerTimeText.color = Color.green;
        }
    }
}
