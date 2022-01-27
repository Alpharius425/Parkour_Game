using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopTimerTrigger : MonoBehaviour
{
    // put on a collider that will stop the timer if the player walks through it.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            TimerManager.instance.StopTimer();
        }
    }
}
