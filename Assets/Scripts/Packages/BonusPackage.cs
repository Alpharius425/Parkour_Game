using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusPackage : MonoBehaviour
{
    private PickupCollider pickupColliderScript;

    [HideInInspector] public bool movePackage = false;
    
    [Header("Settings")]
    private GameObject moveToTarget; // Set to the player's MainCamera on Awake().
    [SerializeField] private int deliveryReward;
    [SerializeField] private float itemPickupSpeed;

    [Header("Game Objects")]
    [Space(5)]
    public GameObject PackageDeliveryPoint;

    private void Awake() {
        moveToTarget = GameObject.FindGameObjectWithTag("MainCamera");
        pickupColliderScript = GameObject.Find("PickupCollider").GetComponent<PickupCollider>();

        // SFX - Throwing
        //GetComponent<SFXAudioSource>().PlaySFXClipRandom(GetComponent<SFXAudioSource>().sfxClipsRandom1);
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

    // SFX
    private void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.GetComponent<Collider>() != null) && (collision.gameObject.GetComponent<Collider>().isTrigger == false) && collision.gameObject.tag != "Player")
        {
            AkSoundEngine.PostEvent("Pickup_Box", this.gameObject);
        }
    }

    public void BonusPackageDelivered() {
        FindObjectOfType<MoneyManager>().AddMoney(deliveryReward);
        Destroy(gameObject);
    }
}
