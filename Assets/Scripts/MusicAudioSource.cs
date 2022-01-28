using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicAudioSource : MonoBehaviour
{
    public AudioClip[] musicClips;
    
    [Header("Settings")]
    [Space(5)]
    [SerializeField] private bool musicIsOn;
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

    // Function to randomly play music clips from musicClips[].
    public void PlayRandomMusicClip() {
        if (musicIsOn) {
            GetComponent<AudioSource>().clip = musicClips[Random.Range(0, musicClips.Length)];
            GetComponent<AudioSource>().Play();
        }
    }
}
