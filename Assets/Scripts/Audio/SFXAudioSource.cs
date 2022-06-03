using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXAudioSource : MonoBehaviour
{
    public AudioClip[] sfxClips;
    public AudioClip[] sfxClipsRandom1;
    public AudioClip[] sfxClipsRandom2;
    public AudioClip[] sfxClipsRandom3;
    
    [Header("Settings")]
    [Space(5)]
    [SerializeField] private bool sfxIsOn;
    [Range(0.0f, 1.0f)]
    [SerializeField] private float debugVolume;
    private float volume;

    private void Update() {
        if (sfxIsOn) {
            // Updates AudioSource volume only if there's been a change.
            if (volume != debugVolume) {
                volume = debugVolume;
                GetComponent<AudioSource>().volume = volume;
            }
        }
    }
    /*
    public void PlaySFXClip(AudioClip clip) {
        if (sfxIsOn) {
            GetComponent<AudioSource>().clip = clip;
            GetComponent<AudioSource>().Play();
        }
    }

    public void PlaySFXClipRandom(AudioClip[] randomClips) {
        if (sfxIsOn) {
            GetComponent<AudioSource>().clip = randomClips[Random.Range(0, randomClips.Length)];
            GetComponent<AudioSource>().Play();
        }
    }
    */
}
