using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowObject : MonoBehaviour
{
    public static ArrowObject Instance;

    [Header("Game Objects")]
    [SerializeField] private GameObject targetObject;

    private void Awake() {
        Instance = this;
    }

    private void FixedUpdate() {
        //transform.LookAt(targetObject.transform);
        transform.LookAt(deliverySpot.Instance.transform);
    }

    public void SetTarget(GameObject incomingTarget) {
        targetObject = incomingTarget;
    }
}
