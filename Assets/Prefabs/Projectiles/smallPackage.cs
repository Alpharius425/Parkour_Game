using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class smallPackage : MonoBehaviour
{
    public static smallPackage Instance;

    [Header("Projectile Options")]
    public float forwardSpeed;
    public float upSpeed;
    public float projectileLifetime;
    public Vector3 forceVector; // For Bonus Package Throw in packageThrow script.

    Vector3 forceVector3;

    // Start is called before the first frame update
    void Awake() {
        Instance = this;
        forceVector3 = new Vector3 (0f,upSpeed,forwardSpeed);
        forceVector = forceVector3; // For Bonus Package Throw in packageThrow script.
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
