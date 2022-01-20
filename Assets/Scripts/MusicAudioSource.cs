using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicAudioSource : MonoBehaviour
{
    public AudioClip[] musicClips;
    [SerializeField] private bool musicIsOn;

    private void Awake() {
        if (musicIsOn) {
            GetComponent<AudioSource>().clip = musicClips[Random.Range(0, musicClips.Length)];
            GetComponent<AudioSource>().Play();
        }
    }
}
