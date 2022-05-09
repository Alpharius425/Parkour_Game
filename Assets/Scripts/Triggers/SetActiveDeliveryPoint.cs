using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveDeliveryPoint : MonoBehaviour
{
    [SerializeField] GameObject deliveryPoint;
    [SerializeField] bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && !triggered)
        {
            DeliverySpotManager.instance.SetActivePoint(deliveryPoint);
            triggered = true;
        }
    }
}
