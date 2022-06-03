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

    private void Awake() {
        forceVector3 = new Vector3 (0f,upSpeed,forwardSpeed);
        gameObject.GetComponent<Rigidbody>().AddRelativeForce(forceVector3, ForceMode.Impulse);
        Invoke("DestroyObject", projectileLifetime);

        // SFX - Throwing
        AkSoundEngine.PostEvent("Throw_Box", this.gameObject);
    }

    // SFX
    private void OnCollisionEnter(Collision collision) {
        if ((collision.gameObject.GetComponent<Collider>() != null) && (collision.gameObject.GetComponent<Collider>().isTrigger == false) && collision.gameObject.tag != "Player") {
            Debug.LogWarning("smallPackage Collision needs implemented");
            //AkSoundEngine.PostEvent("Collision", this.gameObject);
        }
    }

    private void DestroyObject() {
        Destroy(gameObject);
    }
}
