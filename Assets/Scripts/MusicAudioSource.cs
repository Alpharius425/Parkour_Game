using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicAudioSource : MonoBehaviour
{
    [Header("Settings")]
    public AudioClip[] musicClips;
    [SerializeField] private bool musicIsOn;

    [Header("Debug")]
    [Space(5)]
    [Range(0.0f,1.0f)]
    [SerializeField] private float debugVolume;
    private float volume;

    private void Awake() {
        if (musicIsOn) {
            PlayRandomMusicClip();
        }
    }

    private void Update() {
        if (musicIsOn) {
            // Updates AudioSource volume only if there's been a change.
            if (volume != debugVolume) {
                volume = debugVolume;
                GetComponent<AudioSource>().volume = volume;
            }

            // Plays another random music clip after the current clip playing is finished.
            if (GetComponent<AudioSource>().isPlaying == false) {
                PlayRandomMusicClip();
            }
        } 
    }

    public void PlayRandomMusicClip() {
        GetComponent<AudioSource>().clip = musicClips[Random.Range(0, musicClips.Length)];
        GetComponent<AudioSource>().Play();
    }
}
