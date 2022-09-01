using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepsAudioSource : MonoBehaviour
{
    public AudioClip[] soundClips;

    public GameObject playerControllerObject;
    public PlayerController playerController;
    
    [Header("Settings")]
    [Space(5)]
    [SerializeField] private bool soundIsOn;
    [Range(0.0f,1.0f)]
    [SerializeField] private float debugVolume;
    private float volume;

    
    private void Awake() {
        /*
        if (soundIsOn) {
            PlayRandomMusicClip();
        }
        */

        playerController = playerControllerObject.GetComponent<PlayerController>();
    }
    



    private void Update() {
        if (soundIsOn) {
            // Updates AudioSource volume only if there's been a change.
            if (volume != debugVolume) {
                volume = debugVolume;
                GetComponent<AudioSource>().volume = volume;
            }

            if (playerController.grounded) {
                if (playerController.currentState == State.Walking) {
                    Debug.Log("Sound playing");
                    PlayRandomSoundClip();
                }
            }

            /*
            // Plays another random music clip after the current clip playing is finished.
            if (GetComponent<AudioSource>().isPlaying == false) {
                PlayRandomMusicClip();
            }
            */
        } 
    }

    // Function to randomly play music clips from musicClips[].
    public void PlayRandomSoundClip() {
        if (soundIsOn) {
            GetComponent<AudioSource>().clip = soundClips[Random.Range(0, soundClips.Length)];
            GetComponent<AudioSource>().Play();
        }
    }
}
