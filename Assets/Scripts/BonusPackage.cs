using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusPackage : MonoBehaviour
{
    public static BonusPackage Instance;
    
    [SerializeField] private PickupCollider pickupColliderScript;

    //[HideInInspector] public bool packagePickedUp = false;
    [HideInInspector] public bool movePackage = false;
    
    private GameObject moveToTarget; // Set to the player's MainCamera on Awake().
    [SerializeField] private float itemPickupSpeed;

    [SerializeField] private GameObject PackageDeliveryPoint;

    private void Awake() {
        Instance = this;
        moveToTarget = GameObject.FindGameObjectWithTag("MainCamera");
    }

    private void Update() {
        if (movePackage) {
            transform.position = Vector3.MoveTowards(transform.position, moveToTarget.transform.position, Time.deltaTime * itemPickupSpeed);
            if (transform.position == moveToTarget.transform.position) {
                packageInHand();
                movePackage = false;
            }
        }
    }

    public void packagePickedUp() {
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<MeshCollider>().isTrigger = true;
    }

    private void packageInHand() {
        pickupColliderScript.bonusPackageInHand = true;
        pickupColliderScript.itemPickedUp(gameObject);
        gameObject.SetActive(false);
    }
}
