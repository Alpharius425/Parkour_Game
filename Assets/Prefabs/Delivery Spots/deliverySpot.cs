using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deliverySpot : MonoBehaviour
{
    public static deliverySpot Instance;
    [Header("VFX")]
    [SerializeField] private GameObject vfxObject;

    private void Awake()
    {
        Instance = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Package")
        {
            Destroy(other.gameObject);
            SpawnVFX();
            DeliverySpotManager.instance.CheckPoints();
        }
    }

    private void SpawnVFX() {
        Instantiate(vfxObject, gameObject.transform.position, transform.rotation);
    }
}
