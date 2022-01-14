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
        if (deliverySpot.Instance != null)
        {
            transform.LookAt(deliverySpot.Instance.transform);
        }
        else {
            gameObject.SetActive(false);
        }
    }
}
