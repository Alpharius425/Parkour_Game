using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class packageThrow : MonoBehaviour
{
    [Header("Game Objects")]
    public GameObject playerProjectilePosition;
    public GameObject projectileObject;
    public float shootCountdown = 1f;
    float shootTimer = 1f;

    Vector3 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        targetPosition = playerProjectilePosition.transform.position;
    }

    void FixedUpdate()
    {
        if (targetPosition != playerProjectilePosition.transform.position)
        {
            targetPosition = playerProjectilePosition.transform.position;
        }

        if (shootTimer > 0)
        {
            shootTimer -= Time.deltaTime;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (shootTimer < 0)
            {
                Instantiate(projectileObject, targetPosition, playerProjectilePosition.transform.rotation);
                shootTimer = shootCountdown;
            }
        }
    }
}
