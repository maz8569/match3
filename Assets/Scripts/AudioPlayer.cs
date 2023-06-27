using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Clip
{
    PAGE_FLIP,
    BUTTON_PRESS,
    KITCHEN_SOUNDS,
    MENU_MUSIC
}

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private List<AudioClip> _clips;
    [SerializeField] private AudioSource _soundSource;
    [SerializeField] private AudioSource _musicSource;

    private const float BASE_VOLUME = 1.0f;
    public bool audioMuted = false;
    public bool musicMuted = false;
    private float _oldMusicVolume;

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

        _musicSource.loop = true;
    }

    public void PlayAudio(Clip clip, float volume = BASE_VOLUME)
    {
        if (!audioMuted)
        {
            _soundSource.PlayOneShot(_clips[(int)clip], volume);
        }
    }

    public void PlayMusic(Clip clip, float volume = BASE_VOLUME)
    {
        if (!musicMuted)
        {
            _musicSource.Stop();
            _musicSource.clip = _clips[(int)clip];
            _musicSource.volume = volume;
            _musicSource.Play();
        }
    }

    public void MuteMusic(bool val)
    {
        if(val)
        {
            _oldMusicVolume = _musicSource.volume;
            _musicSource.volume = 0.0f;
            musicMuted = true;
        }
        else
        {
            _musicSource.volume = _oldMusicVolume;
            musicMuted = false;
        }
    }
}
