using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusPackage : MonoBehaviour
{
    private PickupCollider pickupColliderScript;

    [HideInInspector] public bool movePackage = false;
    
    private GameObject moveToTarget; // Set to the player's MainCamera on Awake().
    [SerializeField] private int deliveryReward;
    [SerializeField] private float itemPickupSpeed;

    public GameObject PackageDeliveryPoint;

    private void Awake() {
        moveToTarget = GameObject.FindGameObjectWithTag("MainCamera");
        pickupColliderScript = GameObject.Find("PickupCollider").GetComponent<PickupCollider>();
    }

    private void Update() {
        if (movePackage) {
            transform.position = Vector3.MoveTowards(transform.position, moveToTarget.transform.position, Time.deltaTime * itemPickupSpeed);
            if (transform.position == moveToTarget.transform.position) {
                pickupColliderScript.BonusPackagePickedUp();
                movePackage = false;
            }
        }
    }

    public void BonusPackageDelivered() {
        FindObjectOfType<MoneyManager>().AddMoney(deliveryReward);
        Destroy(gameObject);
    }
}
