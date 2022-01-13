using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowFromPlayer : MonoBehaviour
{
    private void FixedUpdate()
    {
        transform.LookAt(ArrowObject.Instance.targetPosition);
    }
}
