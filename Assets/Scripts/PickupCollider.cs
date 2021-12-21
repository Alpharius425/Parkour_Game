using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupCollider : MonoBehaviour
{
    public static PickupCollider Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BonusPackage"))
        {
            other.GetComponent<BonusPackage>().movePackage = true;
            other.GetComponent<BonusPackage>().packagePickedUp();
            return;
        }
        else return;
    }
}
