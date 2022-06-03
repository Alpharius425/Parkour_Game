using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicAudioSource : MonoBehaviour
{
    
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
        } 
    }
    // Function to randomly play music clips from musicClips[].
    public void PlayRandomMusicClip() {
        if (musicIsOn) {
            Debug.LogWarning("Need to implement Music @MusicAudioSource.cs");
            // AkSoundEngine.PostEvent("Play_Music", this.gameObject);
        }
    }
}
