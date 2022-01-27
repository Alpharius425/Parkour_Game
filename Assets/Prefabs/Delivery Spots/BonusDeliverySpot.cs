using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusDeliverySpot : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject bonusPackageObject;

    [Header("VFX")]
    [Space(5)]
    [SerializeField] private GameObject vfxObject;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject == bonusPackageObject) {
            other.GetComponent<BonusPackage>().BonusPackageDelivered();
            SpawnVFX();
            Destroy(gameObject);
        }
        else 
        return;
    }

    private void SpawnVFX() {
        Instantiate(vfxObject, gameObject.transform.position, transform.rotation);
    }
}
