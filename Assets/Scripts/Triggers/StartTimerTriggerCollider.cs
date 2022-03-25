using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTimerTriggerCollider : MonoBehaviour
{
    // put on a collider that will start the timer if the player walks through it.
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            TimerManager.instance.StartTimer();
        }
    }
}
