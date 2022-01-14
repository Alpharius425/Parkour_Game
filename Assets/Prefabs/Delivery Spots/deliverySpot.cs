using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deliverySpot : MonoBehaviour
{
    public static deliverySpot Instance;

    public GameObject[] deliverySpots;
    int deliverySpotCount = 0;
    [SerializeField] private MoneyManager moneyManager;
    [SerializeField] private int moneyReward;

    private void Awake() {
        Instance = this;

        //ArrowObject.Instance.SetTarget(gameObject); // Sets next target object for ArrowObject.
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Package")
        {
            Destroy(other.gameObject);
            moneyManager.AddMoney(moneyReward);

            if (deliverySpotCount == deliverySpots.Length)
            {
                Debug.Log("Last delivery spot hit.");
                Destroy(gameObject);
            }
            else
            {
                gameObject.transform.position = deliverySpots[deliverySpotCount].transform.position;
                deliverySpotCount += 1;
            }
        }
    }
}
