using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ESoundType
{
    BackgroundMusic,
    ButtonClickSound
}

[Serializable]
public struct Sound
{
    public ESoundType Type;
    public AudioClip AudioClip;
    public bool IsMusic;
}

public class SoundController : MonoBehaviour
{
    public static SoundController Instance { get; private set; }
    public bool IsMusicMute { get; private set; } = false;
    public bool IsSoundMute { get; private set; } = false;

    [SerializeField] private AudioSource _MusicSource;
    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private List<Sound> _sounds = new();

    private Dictionary<ESoundType, Sound> _soundMap = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        InitSound();
    }

    private void InitSound()
    {
        foreach (var item in _sounds)
        {
            _soundMap[item.Type] = item;
        }
    }

    public void Play(ESoundType type)
    {
        if(_soundMap[type].IsMusic)
            PlayMusic(type);
        else 
            PlaySound(type);
    }

    public bool ToggleSound()
    {
        IsSoundMute = !IsSoundMute;
        _audioSource.mute = IsSoundMute;
        return IsSoundMute;
    }

    public bool ToggleMusic()
    {
        IsMusicMute = !IsMusicMute;
        _MusicSource.mute = IsMusicMute;
        return IsMusicMute;
    }

    private void PlayMusic(ESoundType type)
    {
        _MusicSource.clip = _soundMap[type].AudioClip;
        _MusicSource.Play();
    }

    private void PlaySound(ESoundType type)
    {
        _audioSource.PlayOneShot(_soundMap[type].AudioClip);
    }
}
