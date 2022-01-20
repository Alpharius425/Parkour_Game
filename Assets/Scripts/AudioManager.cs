using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // SOURCE #1:
    // https://www.daggerhartlab.com/unity-audio-and-sound-manager-singleton-script/

    public static AudioManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxAudioSource;
    [SerializeField] private AudioSource musicAudioSource;

    [Header("Random SFX Pitch")]
    [Space(5)]
    // Random pitch adjustment range.
    private float LowPitchRange; // Default: 0.95f
    private float HighPitchRange; // Default: 1.05f

    private void Awake() {
        // Singleton instance
        if (Instance == null) {
            Instance = this;
        }
        else if (Instance != this) Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    // Play a single clip through the sound effects source.
    public void Play(AudioClip clip) {
        sfxAudioSource.clip = clip;
        sfxAudioSource.Play();
    }

    // Play a single clip through the music source.
    public void PlayMusic(AudioClip clip) {
        musicAudioSource.clip = clip;
        musicAudioSource.Play();
    }

    // Play a random clip from an array, and randomize the pitch slightly.
    public void RandomSoundEffect(params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(LowPitchRange, HighPitchRange);

        sfxAudioSource.pitch = randomPitch;
        sfxAudioSource.clip = clips[randomIndex];
        sfxAudioSource.Play();
    }
}
