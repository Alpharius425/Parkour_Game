using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deliverySpot : MonoBehaviour
{
    public static deliverySpot Instance;

    [Header("Settings")]
    public GameObject[] deliverySpots;
    int deliverySpotCount = 1;
    [SerializeField] private MoneyManager moneyManager;
    [SerializeField] private int moneyReward;

    [Header("VFX")]
    [SerializeField] private GameObject vfxObject;

    private void Awake() {
        Instance = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Package")
        {
            Destroy(other.gameObject);
            moneyManager.AddMoney(moneyReward);

            if (deliverySpotCount == deliverySpots.Length)
            {
                SpawnVFX();

                Debug.Log("Last delivery spot hit.");
                TimerManager.instance.StopTimer();
                GameManager.Instance.EndLevel();
                Destroy(gameObject);
            }
            else
            {
                SpawnVFX();

                gameObject.transform.position = deliverySpots[deliverySpotCount].transform.position;
                deliverySpotCount += 1;
            }
        }
    }

    private void SpawnVFX() {
        Instantiate(vfxObject, gameObject.transform.position, transform.rotation);
    }

    public void ResetPoint(int newPoint)
    {
        if(deliverySpots[deliverySpotCount].gameObject != CheckPointManager.instance.deliveryPoints[newPoint + 1].gameObject)
        {
            gameObject.transform.position = CheckPointManager.instance.deliveryPoints[newPoint].gameObject.transform.position;
            deliverySpotCount = newPoint;
        }
    }
}
