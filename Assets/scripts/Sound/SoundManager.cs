using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    ButtonClick,
    Music,
    FoodCollect,
    PowerCollect,
    SnakeDeath
}
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    public AudioSource soundEffect;
    public AudioSource soundMusic;
    public GameSound[] Sounds;
    public bool IsSfxOn = true;

    [Range(0, 1)]
    public float musicVolume = 0.5f;

    [Range(0, 1)]
    public float sfxVolume = 0.5f;
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
    }
    private void Start()
    {
        PlayMusic(SoundType.Music);
    }

    public void PlayMusic(SoundType soundType)
    {

        AudioClip clip = GetSoundClip(soundType);
        if (clip)
        {
            soundMusic.clip = clip;
            soundMusic.volume = musicVolume;
            soundMusic.Play();
        }
        else
        {
            Debug.LogError("Sound not found : " + soundType.ToString());
        }
    }
    public void Play(SoundType soundType)
    {
        if (!IsSfxOn) return;

        AudioClip clip = GetSoundClip(soundType);
        if (clip)
        {

            soundEffect.PlayOneShot(clip, 1f);
        }
        else
        {
            Debug.LogError("Sound not found : " + soundType.ToString());
        }
    }

    private AudioClip GetSoundClip(SoundType soundType)
    {
        GameSound sound = Array.Find(Sounds, x => x.soundType == soundType);
        if (sound != null)
            return sound.soundClip;
        return null;
    }

    public void ToggleMusic()
    {
        if (soundMusic.isPlaying)
        {
            soundMusic.Stop();
        }
        else
        {
            soundMusic.Play();
        }
    }
}
[Serializable]
public class GameSound
{
    public SoundType soundType;
    public AudioClip soundClip;

}