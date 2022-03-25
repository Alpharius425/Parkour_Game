using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public int curScore;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public void CalculateLevelScore(float playerTime, float averageLevelTime, float scoreMultiplier)
    {
        float initialScore = (averageLevelTime / playerTime) * scoreMultiplier; // calculates the score from the player's time compared to the level time and score multiplier

        int newScore = Mathf.RoundToInt(initialScore); // rounds our score to the nearest int

        if(newScore < initialScore) // checks if we rounded down and adds one if we did.
        {
            newScore += 1;
        }
    }
}
