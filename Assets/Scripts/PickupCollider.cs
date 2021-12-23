using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupCollider : MonoBehaviour
{
    public static PickupCollider Instance;

    public bool bonusPackageInHand = false;
    [SerializeField] private GameObject bonusPackageUI;
    [SerializeField] private packageThrow packageThrowScript;
    private GameObject bonusPackageObject = null;
    private BonusPackage bonusPackageScript = null;

    private void Awake()
    {
        Instance = this;
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
            return;
        }
        else return;
    }

    public void BonusPackagePickedUp() {
        bonusPackageInHand = true;

        // UI
        bonusPackageUI.SetActive(true);

        // Bonus Package Object
        bonusPackageObject.SetActive(false);
    }

    public void BonusPackageThrown() {
        bonusPackageUI.SetActive(false);

        bonusPackageObject.SetActive(true);
        
        
        Invoke("BonusPackageThrownInvoke", 1);
    }

    public void BonusPackageThrownInvoke() {
        bonusPackageInHand = false;
    }
}
