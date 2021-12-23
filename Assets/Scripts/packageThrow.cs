using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class packageThrow : MonoBehaviour
{
    public static packageThrow Instance;

    [Header("Game Objects")]
    public GameObject playerProjectilePosition;
    public GameObject projectileObject;
    public float shootCountdown = 1f;
    float shootTimer = 1f;
    
    [SerializeField] private smallPackage SmallPackageScript;
    [SerializeField] private PickupCollider pickupColliderScript;
    [HideInInspector] public GameObject bonusPackageObject;

    Vector3 targetPosition;

    private void Awake() {
        Instance = this;
        targetPosition = playerProjectilePosition.transform.position;
    }

    void FixedUpdate()
    {
        if (targetPosition != playerProjectilePosition.transform.position)
        {
            targetPosition = playerProjectilePosition.transform.position;
        }

        if (shootTimer > 0)
        {
            shootTimer -= Time.deltaTime;
        }
    }

    public void ThrowPackage()
    {
        if (shootTimer < 0)
        {
            if (pickupColliderScript.bonusPackageInHand)
            {
                // Enables physics for bonusPackageInHand.
                bonusPackageObject.GetComponent<Rigidbody>().useGravity = true;
                bonusPackageObject.GetComponent<MeshCollider>().isTrigger = false;

                // Moves and adds force to bonusPackageInHand.
                bonusPackageObject.transform.position = targetPosition;
                bonusPackageObject.transform.rotation = playerProjectilePosition.transform.rotation;


                pickupColliderScript.BonusPackageThrown();
                bonusPackageObject = null;
            }
            else {
                Instantiate(projectileObject, targetPosition, playerProjectilePosition.transform.rotation);
            }
            shootTimer = shootCountdown;
        }
    }
}
