using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilencerAttachment : Attachments1
{
    public float        muzzleFlashScaleMultiplier = 1;
    public Transform    gunBerrel;
    [SerializeField] public AudioSetings[]  audioClips;
    public GameObject[] muzzleFlashGameObject;

    float               setMuzzleFlashScaleMultiplier = 1;
    Transform           setGunBerrel;
    AudioSetings[]         setAudioClips;
    GameObject[]        setMuzzleFlashGameObject;

    public Gun gun;
    public override void Slect()
    {
        setMuzzleFlashScaleMultiplier   = gun.muzzleFlashScaleMultiplier;
        setGunBerrel                    = gun.gunBerrol;
        setAudioClips                   = gun.muzzleFlashAudio;
        setMuzzleFlashGameObject        = gun.muzzleFlashGameObject;

        gun.muzzleFlashScaleMultiplier  =muzzleFlashScaleMultiplier ;
        gun.gunBerrol                   = gunBerrel;
        gun.muzzleFlashAudio            = audioClips;
        gun.muzzleFlashGameObject       = muzzleFlashGameObject;
        slect();
    }

    public override void DeeSlect()
    {
        deeSlect();
        gun.muzzleFlashScaleMultiplier  = setMuzzleFlashScaleMultiplier;
        gun.gunBerrol                   = setGunBerrel;
        gun.muzzleFlashAudio            = setAudioClips;
        gun.muzzleFlashGameObject       = setMuzzleFlashGameObject;

    }


 
}

