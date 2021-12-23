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

    private void Awake()
    {
        Instance = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BonusPackage") && !bonusPackageInHand)
        {
            other.GetComponent<BonusPackage>().movePackage = true;
            other.GetComponent<BonusPackage>().packagePickedUp();
            return;
        }
        else return;
    }

    public void itemPickedUp(GameObject itemPickedUp) {
        if (itemPickedUp.tag == "BonusPackage") {
            bonusPackageUI.SetActive(true);
            packageThrowScript.BonusPackageInHand(itemPickedUp);
        }
    }

    public void BonusPackageThrown() {
        bonusPackageUI.SetActive(false);
        Invoke("BonusPackageThrownInvoke", 2);
    }

    public void BonusPackageThrownInvoke() {
        bonusPackageInHand = false;
    }

    // GET EVERYTHING RELATED TO BONUS PACKAGES INTO THIS SCRIPT (AT LEAST AS MUCH AS YOU CAN).
    // This script will be the main "manager" for bonus package scripts: This script will determine what to do when the bonus package is in-hand or not, etc.
}
