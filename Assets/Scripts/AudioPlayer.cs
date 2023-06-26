using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Clip
{
    PAGE_FLIP,
    BUTTON_PRESS
}

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private List<AudioClip> _clips;
    [SerializeField] private AudioSource _audioSource;

    private const float BASE_VOLUME = 1.0f;
    public bool audioMuted = false;

    public static AudioPlayer Instance { get; private set; }
    private void Awake() 
    { 
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this) 
        { 
            Destroy(this.gameObject); 
        } 
        else 
        { 
            Instance = this; 
        } 

        DontDestroyOnLoad(this.gameObject);
    }

    public void PlayAudio(Clip clip, float volume = BASE_VOLUME)
    {
        if (!audioMuted)
        {
            _audioSource.PlayOneShot(_clips[(int)clip], volume);
        }
    }
}
