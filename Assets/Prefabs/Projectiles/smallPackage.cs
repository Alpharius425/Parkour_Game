using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class smallPackage : MonoBehaviour
{
    [Header("Projectile Options")]
    public float forwardSpeed;
    public float upSpeed;
    public float projectileLifetime;

    Vector3 forceVector3;

    // Start is called before the first frame update
    void Start()
    {
        forceVector3 = new Vector3 (0f,upSpeed,forwardSpeed);
        
        gameObject.GetComponent<Rigidbody>().AddRelativeForce(forceVector3, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        projectileLifetime -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        if (projectileLifetime < 0)
        {
            Destroy(gameObject);
        }
    }
}
