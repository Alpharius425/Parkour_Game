using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterpolateFollow : MonoBehaviour
{

    [SerializeField] private float interpolationRation;
    [SerializeField] private GameObject interpolateTargetObject;
    private Vector3 interpolateTargetPosition;
    private Quaternion interpolateTargetRotation;

    void Update()
    {
        interpolateTargetPosition = interpolateTargetObject.transform.position;
        interpolateTargetRotation = interpolateTargetObject.transform.rotation;

        transform.position = Vector3.Lerp(transform.position, interpolateTargetPosition, interpolationRation * Time.fixedDeltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, interpolateTargetRotation, interpolationRation * Time.fixedDeltaTime);


    }
}
