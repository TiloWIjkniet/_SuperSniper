using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralSettings : Singleton<GeneralSettings>
{
    public float GameVolume = 1;
    public float MusicVolume = 1;


    public float GameVolumeOvset {get; set;}
    public float SetGameVolume {get;set;}

    public AudioSource[] GamVolumeAudioSource;
    void Awake()
    {
        SetAudio();
        GamVolumeAudioSource = Resources.FindObjectsOfTypeAll(typeof(AudioSource)) as AudioSource[];
    }

    void Update()
    {
        if(SetGameVolume != GameVolume + GameVolumeOvset)
        {
            SetAudio();
        }
    }

    public void SetAudio()
    {
        SetGameVolume = GameVolume + GameVolumeOvset;       
        for (int i = 0; i < GamVolumeAudioSource.Length; i++)
        {
            GamVolumeAudioSource[i].volume =  GameVolume + GameVolumeOvset;
        }
    }

    


}
