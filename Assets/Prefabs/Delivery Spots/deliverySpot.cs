using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deliverySpot : MonoBehaviour
{
    public static deliverySpot Instance;

    [Header("Settings")]
    public GameObject[] deliverySpots;
    int deliverySpotCount = 0;
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
}
