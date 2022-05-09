using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverySpotManager : MonoBehaviour
{
    [SerializeField] GameObject[] deliveryPoints;
    [SerializeField] int currentPoint;
    [SerializeField] private int moneyReward;

    public static DeliverySpotManager instance;


    private void Awake()
    {
        instance = this;
    }

    public void MovePoint()
    {
        deliverySpot.Instance.gameObject.transform.position = deliveryPoints[currentPoint].transform.position;
    }

    public void SetActivePoint(GameObject newPoint)
    {
        bool pointReset = false;

        if(!pointReset)
        {
            for (int i = 0; i < deliveryPoints.Length; i++)
            {
                if(deliveryPoints[i].gameObject == newPoint)
                {
                    currentPoint = i;
                    pointReset = true;
                }
            }
        }

        MovePoint();
    }

    public void CheckPoints()
    {
        MoneyManager.Instance.AddMoney(moneyReward);

        if (currentPoint == deliveryPoints.Length)
        {
            Debug.Log("Last delivery spot hit.");
            TimerManager.instance.StopTimer();
            GameManager.Instance.EndLevel();
            Destroy(deliverySpot.Instance.gameObject);
        }
        else
        {
            currentPoint++;
            MovePoint();
        }
    }
}
