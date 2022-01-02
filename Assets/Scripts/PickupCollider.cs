using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupCollider : MonoBehaviour
{
    public static PickupCollider Instance;

    public bool bonusPackageInHand = false;
    [SerializeField] private GameObject bonusPackageUI;
    [SerializeField] private packageThrow packageThrowScript;
    private GameObject bonusPackageObject = null;
    private BonusPackage bonusPackageScript = null;

    // When bonus package is thrown.
    private float forwardSpeed = 20f;
    private float upSpeed = 5f;
    private Vector3 forceVector3;

    private void Awake()
    {
        Instance = this;

        // When bonus package is thrown.
        forceVector3 = new Vector3(0f, upSpeed, forwardSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BonusPackage") && !bonusPackageInHand)
        {
            // Bonus Package Object & Script
            bonusPackageObject = other.gameObject;
            bonusPackageScript = other.GetComponent<BonusPackage>();
            bonusPackageScript.movePackage = true;

            bonusPackageObject.GetComponent<Rigidbody>().useGravity = false;
            bonusPackageObject.GetComponent<MeshCollider>().isTrigger = true;

            // Package Throw Script
            packageThrowScript.bonusPackageObject = other.gameObject;

            bonusPackageInHand = true;
            return;
        }
        else return;
    }

    public void BonusPackagePickedUp() {
        bonusPackageInHand = true;

        // UI
        bonusPackageUI.SetActive(true);

        // Bonus Package Delivery Point
        bonusPackageObject.GetComponent<BonusPackage>().PackageDeliveryPoint.SetActive(true);

        // Bonus Package Object
        bonusPackageObject.SetActive(false);
    }

    public void BonusPackageThrown() {
        bonusPackageUI.SetActive(false);

        bonusPackageObject.SetActive(true);
        bonusPackageObject.GetComponent<Rigidbody>().AddRelativeForce(forceVector3, ForceMode.Impulse);

        Invoke("BonusPackageThrownInvoke", 1);

        // Bonus Package Delivery Point
        Invoke("DisableBonusDeliveryPoint", 3);
    }

    public void BonusPackageThrownInvoke() {
        bonusPackageInHand = false;
    }

    public void DisableBonusDeliveryPoint() {
        if (!bonusPackageInHand) {
            bonusPackageObject.GetComponent<BonusPackage>().PackageDeliveryPoint.SetActive(false);
        }
        else return;
    }
}
