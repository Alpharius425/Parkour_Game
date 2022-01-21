using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlerpObject : MonoBehaviour
{
    public Transform startPos;
    public Transform endPos;
    public float journeyTime;
    public float speed;

    float startTime;
    Vector3 centerPoint;
    Vector3 centerStartPoint;
    Vector3 centerEndPoint;

    [SerializeField] float fracOfJourneyCompleted;

    // Update is called once per frame
    void Update()
    {
        GetCenter(Vector3.up);
        fracOfJourneyCompleted = (Time.time - startTime) / journeyTime * speed;
        transform.position = Vector3.Slerp(centerStartPoint, centerEndPoint, fracOfJourneyCompleted * speed);
        transform.position += centerPoint;
    }

    void GetCenter(Vector3 direction)
    {
        centerPoint = (startPos.position + endPos.position) * 0.5f;
        centerPoint -= direction;
        centerStartPoint = startPos.position - centerPoint;
        centerEndPoint = endPos.position - centerPoint;
    }
}
