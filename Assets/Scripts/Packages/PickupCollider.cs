using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupCollider : MonoBehaviour
{
    public static PickupCollider Instance;

    [Header("Settings")]
    public bool bonusPackageInHand = false;
    [SerializeField] private GameObject bonusPackageUI;
    [SerializeField] private packageThrow packageThrowScript;
    private GameObject bonusPackageObject = null;
    private BonusPackage bonusPackageScript = null;

    // When bonus package is thrown.
    private float forwardSpeed = 20f;
    private float upSpeed = 5f;
    private Vector3 forceVector3;


    [Header("Sound")]
    [Space(5)]
    [SerializeField] private GameObject sfxAudioSourceObject;
    //private SFXAudioSource sfxAudioSourceScript;
    

    private void Awake()
    {
        Instance = this;

        // SFX
        //sfxAudioSourceScript = sfxAudioSourceObject.GetComponent<SFXAudioSource>();

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

        // SFX - Item Pickup
        sfxAudioSourceObject.GetComponent<SFXAudioSource>().PlaySFXClipRandom(sfxAudioSourceObject.GetComponent<SFXAudioSource>().sfxClipsRandom1);

        // Bonus Package Delivery Point
        bonusPackageObject.GetComponent<BonusPackage>().PackageDeliveryPoint.SetActive(true);

        // Bonus Package Object
        bonusPackageObject.SetActive(false);

        // Arrow Object
        ArrowObject.Instance.bonusPackageObject = bonusPackageObject.GetComponent<BonusPackage>().PackageDeliveryPoint;
        ArrowObject.Instance.lookingAtBonus = true;
        ArrowObject.Instance.lookAtBonusBool = true;
    }

    public void BonusPackageThrown() {
        bonusPackageUI.SetActive(false);

        bonusPackageObject.SetActive(true);
        bonusPackageObject.GetComponent<Rigidbody>().AddRelativeForce(forceVector3, ForceMode.Impulse);

        // SFX - Item Throw
        bonusPackageObject.GetComponent<SFXAudioSource>().PlaySFXClipRandom(bonusPackageObject.GetComponent<SFXAudioSource>().sfxClipsRandom1);

        // Arrow Object
        ArrowObject.Instance.bonusPackageObject = null;
        ArrowObject.Instance.lookingAtBonus = false;
        ArrowObject.Instance.lookAtDeliveryBool = true;

        Invoke("BonusPackageThrownInvoke", 0.5f);

        // Bonus Package Delivery Point
        Invoke("DisableBonusDeliveryPoint", 3);
    }

    public void BonusPackageThrownInvoke() {
        bonusPackageInHand = false;
    }

    public void DisableBonusDeliveryPoint() {
        if ((!bonusPackageInHand) && (bonusPackageObject != null)) {
            bonusPackageObject.GetComponent<BonusPackage>().PackageDeliveryPoint.SetActive(false);
        }
        else return;
    }

    public void LosePackage()
    {
        bonusPackageObject.GetComponent<BonusPackage>().PackageDeliveryPoint.SetActive(false); // turns off the delivery point

        // turns off the arrow and UI
        ArrowObject.Instance.bonusPackageObject = null;
        ArrowObject.Instance.lookingAtBonus = false;
        ArrowObject.Instance.lookAtDeliveryBool = true;
        bonusPackageUI.SetActive(false);

        // disables the actual package
        bonusPackageInHand = false;
        bonusPackageScript = null;
        bonusPackageObject = null;
    }
}
