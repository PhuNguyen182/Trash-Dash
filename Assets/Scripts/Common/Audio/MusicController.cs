using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum SourceAudio
{
    Music = 0, 
    SFX = 1
}

public class MusicController : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource soundEffectSource;

    public static event Action<float> OnMasterVolumeChange;
    public static event Action<float> OnMusicVolumeChange;
    public static event Action<float> OnSoundVolumeChange;

    private const string MASTER_VOLUME = "MasterVolume";
    private const string MUSIC_VOLUME = "MusicVolume";
    private const string SOUND_VOLUME = "SoundVolume";

    private const string MASTER_KEY = "_masterVolume";
    private const string MUSIC_KEY = "_musicVolume";
    private const string SOUND_KEY = "_soundVolume";

    private static MusicController _instance;

    public float MasterVolume
    {
        get { return PlayerPrefs.GetFloat(MASTER_KEY, 1); }
        set
        {
            PlayerPrefs.SetFloat(MASTER_KEY, value);
            OnMasterVolumeChange?.Invoke(value);
        }
    }

    public float MusicVolume
    {
        get { return PlayerPrefs.GetFloat(MUSIC_KEY, 1); }
        set
        {
            PlayerPrefs.SetFloat(MUSIC_KEY, value);
            OnMusicVolumeChange?.Invoke(value);
        }
    }

    public float SoundVolume
    {
        get { return PlayerPrefs.GetFloat(SOUND_KEY, 1); }
        set
        {
            PlayerPrefs.SetFloat(SOUND_KEY, value);
            OnSoundVolumeChange?.Invoke(value);
        }
    }

    public static MusicController Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<MusicController>();
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else
        {
            if (this != _instance)
                Destroy(this.gameObject);
        }

        DontDestroyOnLoad(_instance);
    }

    private void Start()
    {
        OnMasterVolumeChange += SetMasterVolume;
        OnMusicVolumeChange += SetMusicVolume;
        OnSoundVolumeChange += SetSoundVolume;

        MasterVolume += 0;
        MusicVolume += 0;
        SoundVolume += 0;
    }

    private void OnDestroy()
    {
        OnMasterVolumeChange -= SetMasterVolume;
        OnMusicVolumeChange -= SetMusicVolume;
        OnSoundVolumeChange -= SetSoundVolume;
    }

    private void SetMasterVolume(float volume)
    {
        float masterVolume = Mathf.Log10(volume) * 20;
        audioMixer.SetFloat(MASTER_VOLUME, masterVolume);
    }

    private void SetMusicVolume(float volume)
    {
        float musicVolume = Mathf.Log10(volume) * 20;
        audioMixer.SetFloat(MUSIC_VOLUME, musicVolume);
    }

    private void SetSoundVolume(float volume)
    {
        float soundVolume = Mathf.Log10(volume) * 20;
        audioMixer.SetFloat(SOUND_VOLUME, soundVolume);
    }

    public void PlayOneShot(AudioClip clip, SourceAudio sourceType = SourceAudio.SFX, float volumeScale = 1)
    {
        if(clip != null)
        {
            switch (sourceType)
            {
                case SourceAudio.Music:
                    musicSource.PlayOneShot(clip, volumeScale);
                    break;
                case SourceAudio.SFX:
                    soundEffectSource.PlayOneShot(clip, volumeScale: volumeScale);
                    break;
            }
        }
    }

    public void PlaySingle(AudioClip clip, bool loop, SourceAudio sourceType = SourceAudio.Music)
    {
        if(clip != null)
        {
            switch (sourceType)
            {
                case SourceAudio.Music:
                    musicSource.clip = clip;
                    musicSource.loop = loop;
                    musicSource.Play();
                    break;
                case SourceAudio.SFX:
                    soundEffectSource.clip = clip;
                    soundEffectSource.loop = loop;
                    soundEffectSource.Play();
                    break;
            }
        }
    }
}
