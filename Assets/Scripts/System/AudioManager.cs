using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager _instance;
   

    public AudioSource[] audioSources;
    public AudioItem[] soundClips;
    [Space] 
    [Range(0f,1f)]
    public float sfxVolume;
    
    private Dictionary<string, AudioClip> _audioList;
    
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
        
        _audioList=new Dictionary<string, AudioClip>();
        foreach (var clips in soundClips)
            _audioList.Add(clips.name, clips.clip);
    }

    private void Start()
    {
       startPlay();
    }

    public void startPlay()
    {
        Play("RawMelody",0);
        Play("AdditionalEffects",1);
        SetMuteState(1, true);
    }

    public void SetMuteState(int audioSourceNumber, bool isMute)
    {
        audioSources[audioSourceNumber].mute = isMute;
    }
    
    public void Play(String name, int audioSourceNumber, int duration = -1)
    {
        if (!PlayerProfile.Instance.MusicOn) return;
        
        AudioClip clip = _audioList[name];
        if (clip == null) return;
        if (duration != -1 && audioSources[audioSourceNumber].isPlaying) return;

        if (duration != -1)
            StartCoroutine(StopAfterTime(duration, audioSourceNumber));
        
        SetAndPlayClip(audioSources[audioSourceNumber], clip);
    }

    private void SetAndPlayClip(AudioSource audioSource, AudioClip clip)
    {
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void PlaySFX(String name)
    {
        if (!PlayerProfile.Instance.SoundOn) return;
        audioSources[0].PlayOneShot(_audioList[name],sfxVolume);
    }

    private IEnumerator StopAfterTime(int time,int audioSourceNumber)
    {
        yield return new WaitForSeconds(time);
        audioSources[audioSourceNumber].Stop();
    }

    public void StopAllPlayers()
    {
        for (int i = 0; i < audioSources.Length; i++) 
            audioSources[i].Stop();
    }

    public void StopPlayer(int i)
    {
        audioSources[i].Stop();
    }
}

[Serializable]
public class AudioItem
{
    public string name;
    public AudioClip clip;
}