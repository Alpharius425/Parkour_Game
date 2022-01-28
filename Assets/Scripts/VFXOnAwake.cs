using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXOnAwake : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float destroyInSeconds;
    [SerializeField] private bool destroyAfterAudioClip;
    [SerializeField] private bool childOfPlayer;

    [Header("Audio - Optional")]
    [Space(5)]
    [SerializeField] private AudioClip audioClip;

    private void Awake() {
        if (audioClip != null) {
            GetComponent<AudioSource>().PlayOneShot(audioClip);
        }

        // Sets VFX object to child of the player projectile position.
        if (childOfPlayer) {
            gameObject.transform.parent = FindObjectOfType<ProjectileSpawnPosition>().transform;
        }

        // Destroys VFX object either when the played audioClip ends, or within a set amount of time.
        if (destroyAfterAudioClip) {
            Destroy(gameObject, audioClip.length);
        }
        else {
            Destroy(gameObject, destroyInSeconds);
        }
        
        
    }
}
