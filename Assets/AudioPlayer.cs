using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioZone[] audioZones;
    private AudioZone _audioZone;
    
    public float MusicSpeed
    {
        get { return audioSource.pitch;}
        set { audioSource.pitch = value; }
    }

    enum MusicPhase
    {
        Enter,
        Loop,
        Exit,
        Transition
    }
    
    private MusicPhase _musicPhase;

    public void ChangeZone(int index)
    {
        _musicPhase = MusicPhase.Exit;
    }

    private void Update()
    {
        switch (_musicPhase)
        {
            case MusicPhase.Enter:
                if (!audioSource.isPlaying)
                {
                    _musicPhase = MusicPhase.Loop;
                    ChangeToLoop();
                }
                break;
            case MusicPhase.Exit:
                _musicPhase = MusicPhase.Transition;
                ChangeToExit();
                break;
            case MusicPhase.Transition:
                if (!audioSource.isPlaying)
                {
                    _musicPhase = MusicPhase.Enter;
                    ChangeToEnter();
                }
                break;
        }
    }

    private void ChangeToLoop()
    {
        audioSource.clip = _audioZone.LoopMusic;
        audioSource.loop = true;
        audioSource.Play();
    }

    private void ChangeToExit()
    {
        audioSource.clip = _audioZone.ExitMusic;
        audioSource.loop = false;
        audioSource.Play();
    }

    private void ChangeToEnter()
    {
        audioSource.clip = _audioZone.IntroMusic;
        audioSource.loop = false;
        audioSource.Play();
    }
    
    public static AudioPlayer Instance {get; private set;}

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        _audioZone = audioZones[0];
        _musicPhase = MusicPhase.Transition;
    }
}
