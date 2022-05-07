using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    //public static CheckPointManager instance;
    //public GameObject[] deliveryPoints;

    //private void Awake()
    //{
    //    instance = this;
    //}

    //public void UpdateCheckPoint(GameObject[] activeDeliveryPoints)
    //{
    //    int firstPoint = 0; // will save what the first active point should be

    //    for (int i = 0; i < deliveryPoints.Length; i++) // turns all delivery points off
    //    {
    //        deliveryPoints[i].gameObject.SetActive(false);
    //    }

    //    for (int i = 0; i < deliveryPoints.Length; i++) // looks at all the delivery points in the level
    //    {
    //        for (int o = 0; o < activeDeliveryPoints.Length; o++) // looks at all the ones given by the check point
    //        {
    //            if (activeDeliveryPoints[o].gameObject == deliveryPoints[i].gameObject) // if the check point is on the list
    //            {
    //                deliveryPoints[i].SetActive(true); // turn on the delivery point

    //                if(firstPoint == 0)
    //                {
    //                    firstPoint = i;
    //                }
    //            }
    //        }
    //    }

    //    deliverySpot.Instance.ResetPoint(firstPoint);
    //}
}
