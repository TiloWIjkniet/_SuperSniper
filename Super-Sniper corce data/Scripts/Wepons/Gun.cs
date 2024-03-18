using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;


public class Gun : Weapon
{   

    

    [Header("Recoil settings")]
    public float AimMultiplier= 0.5f;
    public float snappiness   = 10;
    public float returnSpeed  = 3;
    public Vector3 recoil;

    [Header("Muzzle flash settings ")]
    public float        muzzleFlashScaleMultiplier = 1;
    [SerializeField]    public AudioSetings[]  muzzleFlashAudio;
    public GameObject[] muzzleFlashGameObject;

    [Header("Cartridge")]
    public Vector2 cartridgeForce;
    public Vector3 cartridgeTorque;
    public GameObject cartridgeGameObject;
    public GameObject cartridgeSparks;

    [Header("Magazine")]
    public Transform magazineLocation;
    public GameObject magazine;

    [Header("Attachments")]
    public GameObject[] attachmentIndicator;
    public GameObject frame;
    public GameObject[] berrelAttachment;
    public GameObject[] scopeAttachment;
    public GameObject[] underGunAttachment;
    public int curentBerrelAttachment {get;set;}
    public int curentScopeAttachment {get;set;}
    public int curentUnderGunAttachment {get;set;}

    public float staminaDecrease;
    public float MaxsAimSpeed = 6;
    public float aimFieldOfViewSpeed = 15f;
    public float aimFieldOfView = 60;
    public LayerMask layerMask;
    [HideInInspector]public float aimSens = 20;

    
    public Transform crosAirPos;
    public Transform cartridgeLocation;
    public Transform gunBerrol;
    public GameObject bullet;
    public float speedLeftHandConstraint = 6;
    public TwoBoneIKConstraint LeftHandConstraint;
    public MultiParentConstraint MagazineConstraint;


    public bool isAim           {get{return UnityEngine.Input.GetMouseButton(1) && speed < MaxsAimSpeed && !reloadAudio.isPlaying && !gunNearWall;} set{isAim = value;}}
    public bool isShooting      {get{return UnityEngine.Input.GetMouseButton(0) && CurrentAmmoCapacity > 0 &&  !gunNearWall;} set{isShooting = value;}}
    public bool isChangeLodaut  {get{return UnityEngine.Input.GetKey(KeyCode.C);} set{isChangeLodaut = value;}}
    public bool isReloade       {get{return UnityEngine.Input.GetKeyDown(KeyCode.R) || CurrentAmmoCapacity <= 0;} set{isReloade = value;}}

    public bool gunNearWall {get; set;}
    public int CurrentAmmoCapacity {get;set;}


    public UnityEngine.UI.Image image;
    public  AudioSource reloadAudio {get;set;}
    CameraRecoil cameraRecoil;
    public Camera gunCamera {get;set;}
    Vector3 setPos;




    public override void Awake()
    {
        AwakeWeapon();
        reloadAudio = GetComponent<AudioSource>();
        cameraRecoil = GetComponentInParent<CameraRecoil>();
        gunCamera = cameraRecoil.transform.GetChild(0).GetChild(0).GetComponent<Camera>();
    }

    public override  void Start()
    {
        
        StartWeapon();
        CurrentAmmoCapacity = ammunitionCapacity;
        setPos = transform.parent.localPosition;
    } 

    public override void Update() 
    {   

        brains_Player.sensitivityX = Mathf.Lerp(brains_Player.sensitivityX, isAim? aimSens: 20, Time.deltaTime * 5);
        brains_Player.sensitivityY=  Mathf.Lerp(brains_Player.sensitivityY, isAim? aimSens: 20, Time.deltaTime * 5);

        if(speed> 6) brains_Player.Stamina(staminaDecrease * Time.deltaTime);
        animator.SetFloat("FireRate",1 + fireRate * 0.25f);
        UpdateWeapon();
        Constraints();
        HandlesAttachments();

        animator.SetBool("IsShooting", isShooting);
        animator.SetBool("IsAim", isAim);
        animator.SetBool("ChangeLodaut",isChangeLodaut);
        animator.SetBool("Reload", isReloade);


        float wantetFieldOfView = (isAim && !reloadAudio.isPlaying && !gunNearWall) ? aimFieldOfView : 60;
        gunCamera.fieldOfView = Mathf.Lerp(gunCamera.fieldOfView, wantetFieldOfView , Time.deltaTime * aimFieldOfViewSpeed);

        Vector3 wantetPos = (isAim && crosAirPos != null) ?  crosAirPos.localPosition : setPos;
        transform.parent.localPosition = Vector3.Lerp(transform.parent.localPosition, wantetPos , Time.deltaTime * 15 );

        if(reloadAudio.isPlaying && (!animator.GetCurrentAnimatorStateInfo(0).IsName("Recharge") && !animator.GetNextAnimatorStateInfo(0).IsName("Recharge")))reloadAudio.Stop();
  
  
        AimImage();
  
  
  
  
  
  
  
    }

    public virtual void AimImage()
    {
        
        Color wantetColor = isAim? new Color(255,255,255,0.5f) : new Color(255,255,255,0);
        image.color = Color.Lerp(image.color, wantetColor, Time.deltaTime * aimFieldOfViewSpeed * 0.25f);
    }


    void HandlesAttachments()
    {

        if((animator.GetCurrentAnimatorStateInfo(0).IsName("changeWeaponLodauto") || attachmentIndicator[0].transform.localScale.x > 0 )) 
        {
            AttachmentSwitch((attachmentIndicator[0].transform.localScale.x >= 0 && !isAim && !reloadAudio.isPlaying  && isChangeLodaut && (brains_Player.generalUsage_Player.fixedSpeed <= 5)) ? true : false);
        }
        else  
        {
            Cursor.lockState = CursorLockMode.Locked;
            brains_Player.useCamera = true;
            return;
        }

        Cursor.lockState = animator.GetCurrentAnimatorStateInfo(0).IsName("changeWeaponLodauto") ? CursorLockMode.None: CursorLockMode.Locked;
        brains_Player.useCamera = !animator.GetCurrentAnimatorStateInfo(0).IsName("changeWeaponLodauto");
    }

    public void StartReload()
    {
        reloadAudio.Play();
    }

    public void LetGoMagazine()
    {
        var Magazine  = Instantiate(magazine, magazineLocation);
        Magazine.transform.SetParent(brains_Player.transform);
        Magazine.transform.localScale = new Vector3(1,1,1);
        Magazine.GetComponent<Rigidbody>().AddForce(-magazineLocation.up * 60,ForceMode.Force);
    }

    public void HandelReload()
    {
        CurrentAmmoCapacity = ammunitionCapacity;
    }

    void AttachmentSwitch (bool valu)
    {
        for (int i = 0; i < attachmentIndicator.Length; i++)
        {
           attachmentIndicator[i].transform.localScale = Vector3.Lerp(attachmentIndicator[i].transform.localScale, new Vector3(1,1,1) * (valu? 1: 0), Time.deltaTime * 10 );
           attachmentIndicator[i].SetActive(true);
        }
    }

    public void BerrelIncrese()
    {
        berrelAttachment[curentBerrelAttachment].GetComponent<Attachments1>().DeeSlect();
        StartCoroutine(SetActiveTime(berrelAttachment[curentBerrelAttachment], false, 0.05f));
        curentBerrelAttachment = (curentBerrelAttachment + 1 < berrelAttachment.Length)? curentBerrelAttachment + 1 : curentBerrelAttachment = 0;
        StartCoroutine(SetActiveTime(berrelAttachment[curentBerrelAttachment], true, 0.05f));
        berrelAttachment[curentBerrelAttachment].GetComponent<Attachments1>().Slect();
    }

    public void scopeIncrese()
    {
        scopeAttachment[curentScopeAttachment].GetComponent<Attachments1>().DeeSlect();
        StartCoroutine(SetActiveTime(scopeAttachment[curentScopeAttachment], false, 0.05f));
        curentScopeAttachment = (curentScopeAttachment + 1 < scopeAttachment.Length)? curentScopeAttachment + 1 : curentScopeAttachment = 0;
        StartCoroutine(SetActiveTime(scopeAttachment[curentScopeAttachment], true, 0.05f));
        StartCoroutine(SetActiveTime(frame,curentScopeAttachment + curentUnderGunAttachment != 0, 0.05f));
        scopeAttachment[curentScopeAttachment].GetComponent<Attachments1>().Slect();
    }

    public void UnderGuntIncrese()
    {
        underGunAttachment[curentUnderGunAttachment].GetComponent<Attachments1>().DeeSlect();
        StartCoroutine(SetActiveTime(underGunAttachment[curentUnderGunAttachment], false, 0.05f));
        curentUnderGunAttachment = (curentUnderGunAttachment + 1 < underGunAttachment.Length)? curentUnderGunAttachment + 1 : curentUnderGunAttachment = 0;
        StartCoroutine(SetActiveTime(underGunAttachment[curentUnderGunAttachment], true, 0.05f));
        StartCoroutine(SetActiveTime(frame,curentScopeAttachment + curentUnderGunAttachment != 0, 0.05f));
        underGunAttachment[curentUnderGunAttachment].GetComponent<Attachments1>().Slect();
    }

    public void Shood()
    {
        cameraRecoil.Shood(recoil * manageable ,AimMultiplier,snappiness, returnSpeed, isAim);
        HandlesMuzzleFlash();
        CurrentAmmoCapacity --;
        StartCoroutine(BulletBehavior());
    }

    public void HandlesCartridge()
    {
        GameObject cartridge = Instantiate(cartridgeGameObject, cartridgeLocation);
        cartridge.transform.parent = brains_Player.transform;
        
        Rigidbody CartridgeRB = cartridge.GetComponent<Rigidbody>();
        CartridgeRB.AddForce ((-cartridgeLocation.right * cartridgeForce.x + cartridgeLocation.up * cartridgeForce.y)* Random.Range(0.8f,1.2f), ForceMode.Impulse);
        CartridgeRB.AddTorque(cartridgeTorque , ForceMode.Impulse);      
    

        if(cartridgeSparks == null) return;
        GameObject sparks = Instantiate(cartridgeSparks, cartridgeLocation);
        ParticleSystem particleSystem = sparks.GetComponent<ParticleSystem>();
        var main = particleSystem.main;
        main.maxParticles = Random.Range(0, 5); 
        main.customSimulationSpace = brains_Player.transform;

        particleSystem.Play();
        Destroy(sparks,5f);
    }

    void HandlesMuzzleFlash()
    {
        GameObject MuzzleFlash              = Instantiate(muzzleFlashGameObject[Random.Range(0, muzzleFlashGameObject.Length -1)], gunBerrol);
        MuzzleFlash.transform.localScale    *= muzzleFlashScaleMultiplier;
        
        AudioSource MuzzleFlashAudioSource  = MuzzleFlash.GetComponent<AudioSource>();
        int  MuzzleFlashAudioInt= Random.Range(0, muzzleFlashAudio.Length);
        MuzzleFlashAudioSource.clip         = muzzleFlashAudio[MuzzleFlashAudioInt].audioClip;
        MuzzleFlashAudioSource.volume       = muzzleFlashAudio[MuzzleFlashAudioInt].volume * Random.Range(0.8f,1.2f);
        MuzzleFlashAudioSource.pitch        = muzzleFlashAudio[MuzzleFlashAudioInt].pitch * Random.Range(0.8f,1.2f);
        MuzzleFlashAudioSource.Play();
        Destroy(MuzzleFlash, 2f);
        
    }

    public virtual void  Constraints()
    {
        float LeftHanWantetWaiht =  
        (
            GetNextAnimatorStateTag("LeftHand") || 
            GetNextAnimatorStateTag("LeftHand_Magazine") ||
            brains_Player.generalUsage_Player.fixedSpeed >= speedLeftHandConstraint
            ? 0: 1
        );

        LeftHandConstraint.weight = Mathf.Lerp(LeftHandConstraint.weight, LeftHanWantetWaiht, Time.deltaTime * 10);
        float MagazinenWantetWaiht = GetNextAnimatorStateTag("LeftHand_Magazine") ? 0: 1;

        MagazineConstraint.weight = Mathf.Lerp(MagazineConstraint.weight, MagazinenWantetWaiht, Time.deltaTime * 100);
    }

    IEnumerator SetActiveTime(GameObject gameObject, bool value, float Time)
    {
        yield return new WaitForSeconds(Time);
        gameObject.SetActive(value);
    }


    public IEnumerator BulletBehavior()
    {
        float grafity = GeneralWeaponSettings.Instance.grafity;
        Vector3 whind = GeneralWeaponSettings.Instance.whint;
        Vector3 ofset = gunCamera.transform.right *  (Random.Range(-(1-acuration) * 100,(1-acuration) * 100)/ 10000f) + gunCamera.transform.up *  (Random.Range(-(1-acuration) * 100,(1-acuration) * 100)/ 10000f);
        ofset   *= isAim ? AimMultiplier : 1;
//        Debug.Log(ofset);
        BulletTrajectory bulletTrajectory = new BulletTrajectory((gunCamera.transform.forward + ofset) * bulletSpeed , gunCamera.transform.position, whind, grafity, 0.007f, 0.2f, layerMask, gunCamera.transform);
        Debug.DrawLine(bulletTrajectory.bulletPos, bulletTrajectory.bulletPos + bulletTrajectory.bulletDer * Time.fixedDeltaTime , Color.green, 1);

        RaycastHit hit;
        
        var Bullet = Instantiate(bullet, gunBerrol.position, gunBerrol.rotation);

        do
        {
            hit = bulletTrajectory.HitObject(Time.fixedDeltaTime);
             Debug.DrawRay(bulletTrajectory.bulletPos, bulletTrajectory.bulletDer *Time.fixedDeltaTime, Color.white, 5);
            yield return new WaitForFixedUpdate();

            Bullet.transform.position = Vector3.Lerp(gunBerrol.position, bulletTrajectory.bulletPos, bulletTrajectory.livetime * 10);
            if(hit.collider != null && hit.collider.GetComponentInParent<HitSettings>() != null) 
            {
                Debug.DrawLine(gunCamera.transform.position, hit.point, Color.green, 1); 
                Debug.DrawRay(bulletTrajectory.bulletPos, Vector3.up, Color.blue, 0.5f);

                HitSettings hitSettings = hit.collider.GetComponentInParent<HitSettings>();
                hitSettings.Hit(hit, damage);
                
                InpactData inpactData = GeneralWeaponSettings.Instance.inpactDatas[(int)(hitSettings.hitObject)];
                HandelBulletParticel(hit ,inpactData, 1);
                
                float stopForce =   ObjectThickNes(bulletTrajectory.bulletPos, bulletTrajectory.bulletDer) * inpactData.stopForce;
                bulletTrajectory.bulletDer -= bulletTrajectory.bulletDer.normalized * stopForce;

                if(bulletTrajectory.bulletSpeed <= 10) {break;}

                RaycastHit[] hits =  Physics.RaycastAll(bulletTrajectory.bulletPos + bulletTrajectory.bulletDer * 30,-bulletTrajectory.bulletDer, 30, layerMask);
                HandelBulletParticel(GetClosest(hits), inpactData, 0.5f);


            if(hit.collider.GetComponent<Rigidbody>() != null)
            {
                hit.collider.GetComponent<Rigidbody>().AddForceAtPosition(-bulletTrajectory.bulletDer.normalized * bulletSpeed * 0.5f, hit.point, ForceMode.Impulse);
            }
                // if(hit.collider.GetComponent<Rigidbody>() != null)
                // {
                //     Animator ani = hit.collider.GetComponentInParent<Animator>();
                //     ani.enabled = false;
                //     Rigidbody rb =hit.collider.GetComponent<Rigidbody>();
        
                //     rb.isKinematic = false;
                //     rb.AddForceAtPosition(-bulletTrajectory.bulletDer.normalized * bulletSpeed * 0.5f, hit.point, ForceMode.Impulse);

                //     yield return new WaitForSeconds(0.05f);

                //     rb.isKinematic = true;
                //     ani.enabled = true;
                // }
                break;
            }
            else if(hit.collider != null)
            {
                Debug.DrawRay(bulletTrajectory.bulletPos, Vector3.up, Color.blue, 0.5f);
                Debug.DrawLine(gunCamera.transform.position, hit.point, Color.green, 1); 
               
                
                InpactData inpactData = GeneralWeaponSettings.Instance.inpactDatas[GetLayerInt(hit.collider.gameObject.layer)];
                HandelBulletParticel(hit ,inpactData, 1);
                
                float stopForce =   ObjectThickNes(bulletTrajectory.bulletPos, bulletTrajectory.bulletDer) * inpactData.stopForce;
                bulletTrajectory.bulletDer -= bulletTrajectory.bulletDer.normalized * stopForce;

                if(bulletTrajectory.bulletSpeed < 0) { break;}

                RaycastHit[] hits =  Physics.RaycastAll(bulletTrajectory.bulletPos + bulletTrajectory.bulletDer * 30,-bulletTrajectory.bulletDer, 30, layerMask);
                Debug.DrawRay(bulletTrajectory.bulletPos, Vector3.up, Color.blue, 0.5f);
                HandelBulletParticel(GetClosest(hits), inpactData, 0.5f);
                if(hit.collider.GetComponent<Rigidbody>()  != null)
                {

                    // AnimatorController ac = hit.collider.GetComponentInParent<Animator>().runtimeAnimatorController as AnimatorController;
                    // AnimatorControllerLayer[] acLayers = ac.layers;
                    // AnimatorControllerLayer acLayer = ac.layers[0]; //Replace 0 with your layer index
                    // AvatarMask mask = acLayer.avatarMask;
                    // Debug.Log(mask);
                  //  mask.SetHumanoidBodyPartActive(AvatarMaskBodyPart.Head,false);

                    Rigidbody rb =hit.collider.GetComponent<Rigidbody>();
                    rb.isKinematic = false;
                


                    rb.AddForceAtPosition(-bulletTrajectory.bulletDer.normalized * bulletSpeed, hit.point, ForceMode.Impulse);
                }
                break;
            }
            
        } 
        while (bulletTrajectory.bulletSpeed >= 0 && bulletTrajectory.livetime < 200f && bulletTrajectory.bulletPos.y > -1f);
        Destroy(Bullet);
    }

    
    void HandelBulletParticel(RaycastHit hit, InpactData inpactData, float scale)
    {
        if(hit.collider == null ) return;
        var inpactParticel = Instantiate(inpactData.InpactGameObject[Random.Range(0,inpactData.InpactGameObject.Length)], hit.point, hit.collider.transform.rotation);
        inpactParticel.transform.parent = hit.collider.transform;
        inpactParticel.transform.LookAt(brains_Player.transform.position);
        inpactParticel.transform.localScale *= scale;

        AudioSource audioSorce = inpactParticel.GetComponent<AudioSource>();
        audioSorce.clip = inpactData.InpactAudioClip[Random.Range(0,inpactData.InpactGameObject.Length)];
        audioSorce.volume *= Random.Range(0.8f,1.2f);
        audioSorce.pitch = Random.Range(0.9f,1.1f);
        audioSorce.Play();
        Destroy(inpactParticel, 5);

    }

    float ObjectThickNes(Vector3 pos, Vector3 der)
    {
        RaycastHit[] hits =  Physics.RaycastAll(pos + der * 30,-der, 30, layerMask);
        return Vector3.Distance(transform.position, GetClosest(hits).point);
        
    }

    int GetLayerInt(LayerMask layer)
    {
        for (int i = 0; i <System.Enum.GetNames(typeof(GeneralWeaponSettings.HitObjects)).Length; i++)
        {
            if(System.Enum.GetNames(typeof(GeneralWeaponSettings.HitObjects))[i] == LayerMask.LayerToName(layer)) return i;   
        }
        return 0;
    }

    public void SetSensitivity(float sens)
    {
        aimSens = sens;
    }

    public void ChangeWepon()
    {
        GetComponentInParent<GunSwhits>().SetPlus();
        GetComponentInChildren<WeaponSway>().Hide();
    }

    RaycastHit GetClosest(RaycastHit[] hits)
    {
        if(hits.Length <= 0) return new RaycastHit();
        int strongest = 0;
        float maxsDis = 0;
        for (int i = 0; i < hits.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, hits[i].point);
            if(distance > maxsDis) {maxsDis = distance; strongest = i;}
        }
    
        return hits[strongest];
    }
}
[System.Serializable] 
public class AudioSetings
{
    public AudioClip audioClip;
    [Range(0,1)]public float volume = 1;
    [Range(0,1)]public float pitch = 1;

}
