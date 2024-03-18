using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class Weapon : MonoBehaviour
{
    [Header("Basic weapon settings")]
    [SerializeField, Tooltip("The amount of damage the weapon does")]  float Damage;
    [SerializeField, Tooltip("The range of the weapon")  ,  Range(0,1)] float Range;
    [SerializeField, Tooltip("How accurate the weapon is"), Range(0,1)]float Acuration;
    [SerializeField, Tooltip("How manageable a weapon is"), Range(0,1)]float Manageable;
    [SerializeField, Tooltip("rate of fire")]          float FireRate;
    [SerializeField, Tooltip("The amount of ammo in a clip")] int AmmunitionCapacity;
    [SerializeField, Tooltip("The amount of noce ")]          float Noice;
    [SerializeField] float BulletSpeed;   
    
    public float damage           {get{return Damage             + damageMin;}                set{Damage = value;}}
    public float range            {get{return Range              + rangeMin;}                 set{Range = value;}}
    public float acuration        {get{return Mathf.Clamp(Acuration          + acurationMin,0,1);}             set{Acuration = value;}}
    public float manageable       {get{return 1 - Manageable     - manageableMin;}            set{Manageable = value;}}
    public float fireRate         {get{return FireRate           + fireRateMin;}              set{FireRate = value;}}
    public int ammunitionCapacity {get{return AmmunitionCapacity + ammunitionCapacityMin;}    set{AmmunitionCapacity = value;}}
    public float noice            {get{return Noice              + noiceMin;}              set{Noice = value;}}
    public float bulletSpeed      {get{return BulletSpeed        * range;} set{BulletSpeed = value;}}
    public float damageMin {get; set;}    
    
    [HideInInspector]public float rangeMin;       
    [HideInInspector]public float acurationMin;     
    [HideInInspector]public float manageableMin;
    [HideInInspector]public float fireRateMin;     
    [HideInInspector]public int   ammunitionCapacityMin;
    [HideInInspector]public float noiceMin;     


   
    public Voedstap voedstaps;
    public Animator animator            {get; private set;}
    public Brains_Player brains_Player  {get; private set;}

    public AudioSource audioSource;
    public  float speed {get;set;}
    float voedstapDelay;
    
    public virtual  void  Awake()
    {
        AwakeWeapon();
    }

    public void AwakeWeapon()
    {
        animator      = GetComponentInChildren<Animator>();
        brains_Player = GetComponentInParent<Brains_Player>();
    }
    public virtual  void  Start()
    {
        StartWeapon();
    }

    public void StartWeapon()
    {
        brains_Player.PlayerManageable(manageable);
    }

    public virtual  void  Update()
    {
        UpdateWeapon();
    }

    public void UpdateWeapon()
    {
        speed = Mathf.Lerp(speed,brains_Player.generalUsage_Player.fixedSpeed, Time.deltaTime * 10);
        animator.SetFloat("AnimationSpeed", 1 - manageable * (1f/4f));
        animator.SetFloat("Speed", speed * (1 - manageable * (1f/4f)));


        voedstapDelay -= Time.deltaTime * speed;
        if(voedstapDelay<= 0)
        {
            HandelsVoedstap();
        }
        
    }
    
    public Terrain terrain;
    void HandelsVoedstap()
    {
        voedstapDelay = 2.75f;
        if(!brains_Player.isGrounded) return;


        AudioClip[] audioClip = voedstaps.audioClip;
        audioSource.clip = audioClip[Random.Range(0,audioClip.Length)];

        audioSource.pitch = Random.Range(0.9f,1.1f);
        audioSource.volume = voedstaps.volume[brains_Player.generalUsage_Player.GetStateAsInt(brains_Player.stateMachine_Player.currentState)];
        audioSource.Play();
    }

    public bool  GetNextAnimatorStateName(string name)
    {
        return animator.GetNextAnimatorStateInfo(0).IsName(name) || animator.GetCurrentAnimatorStateInfo(0).IsName(name);
    }

    public bool  GetNextAnimatorStateTag(string tag)
    {
        return animator.GetNextAnimatorStateInfo(0).IsTag(tag) || animator.GetCurrentAnimatorStateInfo(0).IsTag(tag);
    }

   


}

[System.Serializable]
public class Voedstap 
{
    public AudioClip[] audioClip;
    public AudioClip[] vall;
    public float[] volume;
}
