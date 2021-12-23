using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusPackage : MonoBehaviour
{
    public static BonusPackage Instance;
    
    private PickupCollider pickupColliderScript;

    //[HideInInspector] public bool packagePickedUp = false;
    [HideInInspector] public bool movePackage = false;
    
    private GameObject moveToTarget; // Set to the player's MainCamera on Awake().
    [SerializeField] private float itemPickupSpeed;

    [SerializeField] private GameObject PackageDeliveryPoint;

    private void Awake() {
        Instance = this;
        moveToTarget = GameObject.FindGameObjectWithTag("MainCamera");
        //pickupColliderScript = PickupCollider.Instance;
        pickupColliderScript = GameObject.Find("PickupCollider").GetComponent<PickupCollider>();
    }

    private void Update() {
        if (movePackage) {
            transform.position = Vector3.MoveTowards(transform.position, moveToTarget.transform.position, Time.deltaTime * itemPickupSpeed);
            if (transform.position == moveToTarget.transform.position) {
                //packageInHand();
                pickupColliderScript.BonusPackagePickedUp();
                movePackage = false;
            }
        }
    }
}
