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

    void Awake() {
        forceVector3 = new Vector3 (0f,upSpeed,forwardSpeed);
        //forceVector = forceVector3; // For Bonus Package Throw in packageThrow script.
        gameObject.GetComponent<Rigidbody>().AddRelativeForce(forceVector3, ForceMode.Impulse);
        Invoke("DestroyObject", projectileLifetime);
    }

    private void DestroyObject() {
        Destroy(gameObject);
    }
}
