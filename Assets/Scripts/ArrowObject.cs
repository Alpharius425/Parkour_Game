using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowObject : MonoBehaviour
{
    public static ArrowObject Instance;

    [Header("Game Objects")]
    private GameObject targetObject;
    [SerializeField] private GameObject bonusArrowObject;
    public GameObject bonusPackageObject;

    [Space(10)]
    [Header("Options")]
    public bool lookingAtBonus;
    public bool lookAtBonusBool;
    public bool lookAtDeliveryBool;
    
    private void Awake() {
        Instance = this;

        lookingAtBonus = false;
        lookAtDeliveryBool = true;
    }

    private void FixedUpdate() {
        // Decides where to look if lookingAtBonus is true or not.
        if (lookingAtBonus) {
            LookAtBonusDeliveryPoint();
        }
        else LookAtDeliveryPoint();
    }

    // Arrow object will lookAt() deliverySpot object until that object is null, to which the Arrow object will SetActive(false);
    private void LookAtDeliveryPoint() {
        
        // Runs once to switch-enable renderers between the normal ArrowObject and BonusArrowObject.
        if (deliverySpot.Instance != null) {
            if (lookAtDeliveryBool) {
                gameObject.GetComponent<Renderer>().enabled = true;
                bonusArrowObject.GetComponent<Renderer>().enabled = false;
                //lookAtBonusBool = true;
                lookAtDeliveryBool = false;
            }

            transform.LookAt(deliverySpot.Instance.transform);
        }
        else {
            gameObject.SetActive(false);
        }
    }

    // Arrow object will lookAt() bonusDeliverySpot object until that object is null, to which the Arrow object will change back to LookAtDeliveryPoint().
    private void LookAtBonusDeliveryPoint() {
        if ((bonusPackageObject != null) && PickupCollider.Instance.bonusPackageInHand == true) {

            // Runs once to switch-enable renderers between the normal ArrowObject and BonusArrowObject.
            if (lookAtBonusBool) {
                bonusArrowObject.GetComponent<Renderer>().enabled = true;
                gameObject.GetComponent<Renderer>().enabled = false;
                //lookAtDeliveryBool = true;
                lookAtBonusBool = false;
            }

            transform.LookAt(bonusPackageObject.transform);
        }
        else lookingAtBonus = false;
    }
}


