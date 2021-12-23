using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusDeliverySpot : MonoBehaviour
{
    [SerializeField] private GameObject bonusPackageObject;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject == bonusPackageObject) {
            other.GetComponent<BonusPackage>().BonusPackageDelivered();
            Destroy(gameObject);
        }
        else 
        return;
    }
}
