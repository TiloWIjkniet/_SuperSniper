using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralWeaponSettings : Singleton<GeneralWeaponSettings>
{
    public List<InpactData> inpactDatas = new List<InpactData>();
    public float grafity = 9.81f;
    public Vector3 whint = Vector3.zero;

    public enum HitObjects{Stone, Metal, Wood, Sand, Flech}
}
[System.Serializable]
public class InpactData
{
    public float stopForce = 10;
    public GameObject[] InpactGameObject;
    public AudioClip[] InpactAudioClip;
}
