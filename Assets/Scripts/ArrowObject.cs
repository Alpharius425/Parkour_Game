using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowObject : MonoBehaviour
{
    public static ArrowObject Instance;

    [Header("Game Objects")]
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private GameObject arrowFromPlayer;
    [SerializeField] private GameObject targetObject;
    //[SerializeField] private Transform fromTransform;
    public Vector3 targetPosition;
    public Quaternion targetRotation;

    [Space(10)]
    [Header("Debug")]
    [SerializeField] private Vector3 debugDirectionVector;

    private void Awake() {
        Instance = this;
    }

    private void FixedUpdate() {
        //ArrowLookAt();

        transform.LookAt(targetObject.transform);
    }

    private void ArrowLookAt() {
        //playerCameraTransform = playerCamera.transform;
        //transform.rotation = Quaternion.FromToRotation(playerCamera.transform.forward, targetPosition);

        //transform.rotation = arrowFromPlayer.transform.rotation;


        // Debug
        //debugDirectionVector = ((targetPosition - playerCamera.transform.position) + playerCamera.transform.rotation.eulerAngles).normalized;

        //transform.rotation = debugDirectionVector;

        //transform.rotation = Quaternion.RotateTowards(playerCamera.transform.rotation, targetRotation, 1000 * Time.deltaTime);
    }

    public void SetTarget(GameObject incomingTarget) {
        //targetPosition = incomingTarget.transform.position;
        //targetRotation = incomingTarget.transform.rotation;
        targetObject = incomingTarget;
    }
}
