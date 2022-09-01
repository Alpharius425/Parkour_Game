using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepsAudioSource : MonoBehaviour
{
    public AudioClip[] soundClips;
    public AudioClip jumpSound;
    public AudioClip landSound;
    public AudioClip slideSound;

    [Space(5)]
    public GameObject playerControllerObject;
    public PlayerController playerController;
    
    [Header("Settings")]
    [Space(5)]
    [SerializeField] private bool soundIsOn;
    [Range(0.0f,1.0f)]
    [SerializeField] private float debugVolume;
    [SerializeField] private float walkInterval;
    [SerializeField] private float runningInterval;
    private float volume;
    private float countdown;

    
    private void Awake() {
        /*
        if (soundIsOn) {
            PlayRandomMusicClip();
        }
        */

        playerController = playerControllerObject.GetComponent<PlayerController>();
    }
    



    private void FixedUpdate() {
        if (soundIsOn) {
            // Updates AudioSource volume only if there's been a change.
            if (volume != debugVolume) {
                volume = debugVolume;
                GetComponent<AudioSource>().volume = volume;
            }

            if (playerController.grounded) {
                if (playerController.currentState == State.Walking || playerController.currentState == State.Crouching) {
                    if (countdown >= walkInterval)
                    {
                        PlayRandomSoundClip();
                        countdown = 0f;
                    }
                    else { countdown += Time.fixedDeltaTime; }  
                }

                if (playerController.currentState == State.Running)
                {
                    if (countdown >= runningInterval)
                    {
                        PlayRandomSoundClip();
                        countdown = 0f;
                    }
                    else { countdown += Time.fixedDeltaTime; }
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

    public void PlayJumpSound()
    {
        if (soundIsOn)
        {
            GetComponent<AudioSource>().clip = jumpSound;
            GetComponent<AudioSource>().Play();
        }
    }
}
