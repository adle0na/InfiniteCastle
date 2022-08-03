using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum BGMEnum
{
    HomeScene = 0,
    PlayScene,
}

public enum SFXEnum
{
    Button = 0,
    Jump,
    Attack,
    Bomb,
    MonsterDie,
    PlayerDie,
}

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Inst => instance;

    public AudioMixer audioMixer;
    public AudioSource bgmAudioSource;
    public AudioSource sfxAudioSource;

    [Header("음악 클립들")]
    public AudioClip[] bgmClips;
    public AudioClip[] sfxClips;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        bgmAudioSource = transform.Find("BGM").GetComponent<AudioSource>();
        sfxAudioSource = transform.Find("SFX").GetComponent<AudioSource>();
    }

    public void PlayBGM(BGMEnum clip)
    {
        bgmAudioSource.Stop();
        bgmAudioSource.clip = bgmClips[(int) clip];
        bgmAudioSource.Play();
    }

    public void PlaySFX(SFXEnum clip)
    {
        //sfxAudioSource.Stop();
        sfxAudioSource.clip = sfxClips[(int) clip];
        sfxAudioSource.Play();
    }
}
