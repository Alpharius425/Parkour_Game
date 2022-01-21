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
        GetComponent<SFXAudioSource>().PlaySFXClipRandom(GetComponent<SFXAudioSource>().sfxClipsRandom1);
    }

    private void OnCollisionEnter(Collision collision) {
        if ((collision.gameObject.GetComponent<Collider>() != null) && (collision.gameObject.GetComponent<Collider>().isTrigger == false) && collision.gameObject.tag != "Player") {
            GetComponent<SFXAudioSource>().PlaySFXClipRandom(GetComponent<SFXAudioSource>().sfxClipsRandom2);
        }
    }

    private void DestroyObject() {
        Destroy(gameObject);
    }
}
