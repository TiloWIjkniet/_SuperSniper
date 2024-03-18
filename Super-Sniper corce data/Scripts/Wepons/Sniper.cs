using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;


// Bugg snipper : 
// Halfe reload handt niet vast


public class Sniper : Gun
{
    
    public TwoBoneIKConstraint RichtHandConstraint;
    public Camera ScopeCamera;
    public bool  RichtConstraintOn  = true;

    bool   LeftConstraintOn = true ;
    bool   magazineConstraintOn = true;
    float  curentAni;
    public Image Image;
    public bool regen {get;set;}
    public AudioSource breathAudio;
    public AudioSource hardBeetAudio;

    public bool isShiftAiming{get{return(!regen && isAim && Input.GetKey(KeyCode.LeftShift) && !reloadAudio.isPlaying);}set{ isShiftAiming = value;}}
    public RectTransform crosIndicator;
    public LineRenderer lineRenderer;
    public void RichtHandConstraintOf()
    {
        RichtConstraintOn = false;

    }

    public void RichtHandConstraintOn()
    {
        RichtConstraintOn = true;

    }

    public void LeftHandConstraintOf()
    {
        LeftConstraintOn = false;

    }

    public void LeftHandConstraintOn()
    {
        LeftConstraintOn = true;

    }

    public void MagazineConstraintOf()
    {
        magazineConstraintOn = false;
    }

    public void MagazineConstraintOn()
    {
        magazineConstraintOn = true;
    }



    public override void Constraints()
    {

        animator.SetBool("IsShift", isShiftAiming);
        if((brains_Player.currentStamina >= 5f ||( brains_Player.currentStamina >= 0 && Input.GetKeyDown(KeyCode.LeftShift)))){regen = false; }
        if(brains_Player.currentStamina <= 0 ){regen = true;}

    
        float LCW = LeftConstraintOn ? 1 : 0; 
        float RCW = RichtConstraintOn ? 1 : 0; 
        float MCW = magazineConstraintOn ? 1 : 0; 

        LeftHandConstraint.weight = Mathf.Lerp(LeftHandConstraint.weight, LCW, Time.deltaTime * 10);
        RichtHandConstraint.weight = Mathf.Lerp(RichtHandConstraint.weight, RCW, Time.deltaTime * 10);
        MagazineConstraint.weight = Mathf.Lerp(LeftHandConstraint.weight, MCW, Time.deltaTime * 10);

        if(isShiftAiming && Input.GetKeyDown(KeyCode.LeftShift)) breathAudio.Play();
        if(isAim)
        {
            float wantetFieldOfView = (isShiftAiming) ? aimFieldOfView - 20 : aimFieldOfView;
            gunCamera.fieldOfView = Mathf.Lerp(gunCamera.fieldOfView, wantetFieldOfView , Time.deltaTime * aimFieldOfViewSpeed* 0.25f);
        }
        if(isShiftAiming)
        {
            if(!hardBeetAudio.isPlaying) hardBeetAudio.Play();
            
            GeneralSettings.Instance.GameVolumeOvset = Mathf.Lerp(GeneralSettings.Instance.GameVolumeOvset, -GeneralSettings.Instance.GameVolume * 0.5f, Time.deltaTime * 10);
            GeneralSettings.Instance.SetAudio();
            hardBeetAudio.volume = 1.1f - brains_Player.currentStamina /brains_Player.maxsStamin;
        }
        else 
        {
            GeneralSettings.Instance.GameVolumeOvset = 0;
        }
        animator.SetFloat("FireRate",  1 + fireRate * 0.25f * (isShiftAiming ? 0.5f : 1));

        if(isAim && scopeAttachment[curentScopeAttachment].GetComponent<AtfanstScope>() != null)
        {
            VisBullitPat();
       

            if(SetCurentScopeAttachment != curentScopeAttachment)
            {
                for (int i = 0; i < lineGameObject.Count; i++)
                {
                    Destroy(lineGameObject[i]);
                }
                lineGameObject = new List<GameObject>();

                StartCoroutine(SwitchScope());
                SetCurentScopeAttachment = curentScopeAttachment;
            }
            else if( setScopeViltOffiuwe != curentAtfanstScope.scopeFiltOvviuw )
            {
                StartCoroutine(SwitchScope());

            }

            // if(Input.GetKeyDown(KeyCode.E)) {k ++; k = Mathf.Clamp(k,0,distanceLines.Length-1); StartCoroutine(SwitchScope()); }
            // else if(Input.GetKeyDown(KeyCode.Q)) {k --; k = Mathf.Clamp(k,0,distanceLines.Length-1); StartCoroutine(SwitchScope()); }
            

          
        }
        AimImage();
        Time.timeScale = isShiftAiming ? 0.8f: 1;


        if(curentAtfanstScope == null)return;

        if(Input.mouseScrollDelta != Vector2.zero)
      {         //  Debug.Log(Input.mouseScrollDelta);

            float value = 50;
            
            if(Input.mouseScrollDelta.y <= -1 &&        GetPOV( curentAtfanstScope.scopeFiltOvviuw, value)<= curentAtfanstScope.minScopeFiltOvviuw)  
                curentAtfanstScope.scopeFiltOvviuw =    GetPOV( curentAtfanstScope.scopeFiltOvviuw, value);
            else if(Input.mouseScrollDelta.y >= 1 &&    GetPOV( curentAtfanstScope.scopeFiltOvviuw, -value)>= curentAtfanstScope.maxsScopeFiltOvviuw) 
                curentAtfanstScope.scopeFiltOvviuw =    GetPOV( curentAtfanstScope.scopeFiltOvviuw, -value);

      //    Debug.Log(curentAtfanstScope.scopeFiltOvviuw);
        }
        
         ScopeCamera.fieldOfView = Mathf.Lerp(ScopeCamera.fieldOfView, curentAtfanstScope.scopeFiltOvviuw, Time.deltaTime * 10);

        

    } 

    float GetPOV(float curetnPOV, float value)
    {
       
        if(value < 0)
        {
             value = 100 + value;
            return (curetnPOV/ 100 * value);
        }
        return (curetnPOV/ value * 100) ;
        
      
    }
    
    
    public override void AimImage()
    {
        Color wantetColor = isShiftAiming? new Color(255,255,255,1f) : new Color(255,255,255, (isAim ? 0.5f: 0));
        Image.color = Color.Lerp(Image.color, wantetColor, Time.deltaTime * aimFieldOfViewSpeed * 0.25f);
    }

    void VisBullitPat()
    {
        if(Input.GetKeyDown(KeyCode.Tab)) crosIndicator.gameObject.SetActive(!crosIndicator.gameObject.active);//EditorApplication.isPaused = true;
        float grafity = GeneralWeaponSettings.Instance.grafity;
        Vector3 whind = GeneralWeaponSettings.Instance.whint;
        BulletTrajectory bulletTrajectory = new BulletTrajectory((gunCamera.transform.forward) * bulletSpeed , gunCamera.transform.position, whind, grafity, 0.007f, 0.2f, layerMask, gunCamera.transform);
        RaycastHit hit;
        do
        {   
            hit = bulletTrajectory.HitObject(Time.fixedDeltaTime);
   
            if(hit.collider != null) break;
        }
        while(Vector3.Distance(bulletTrajectory.bulletPos, transform.position) <= 6000 && bulletTrajectory.livetime <= 20);

        if(hit.collider != null)
        {
            
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(1,( GetScopePos(hit.point, bulletTrajectory).Item2) * 100);
     
            crosIndicator.localPosition =  GetScopePos(hit.point,bulletTrajectory).Item2;
               
        } 
       
    }


    public float[] distanceLines;
    int SetCurentScopeAttachment = 0;
    float setScopeViltOffiuwe;
    List<GameObject> lineGameObject = new List<GameObject>();
    AtfanstScope curentAtfanstScope;
    public  GameObject LineInsatnce;
    int k;
    public IEnumerator SwitchScope()
    {
        
       

        curentAtfanstScope = scopeAttachment[curentScopeAttachment].GetComponent<AtfanstScope>();
        setScopeViltOffiuwe = curentAtfanstScope.scopeFiltOvviuw;
        yield return new WaitForSeconds(0.5f);

        float grafity = GeneralWeaponSettings.Instance.grafity;
        Vector3 whind = GeneralWeaponSettings.Instance.whint;

        BulletTrajectory bulletTrajectory = new BulletTrajectory((gunCamera.transform.forward) * bulletSpeed , gunCamera.transform.position, whind, grafity, 0.007f, 0.2f, layerMask, gunCamera.transform);
        RaycastHit hit;


        int y = 0;
        while(y < distanceLines.Length)
        {
            
            //if(y == k ) startPos  = GetScopePos(bulletTrajectory.bulletPos, bulletTrajectory).Item4;
            hit = bulletTrajectory.HitObject(Time.fixedDeltaTime);
            float angels  = Vector3.Angle(gunCamera.transform.forward, ( bulletTrajectory.bulletPos- gunCamera.transform.position).normalized);       
            if(GetScopePos(bulletTrajectory.bulletPos, bulletTrajectory).Item3 >=  distanceLines[y])
            {
                Debug.Log(GetScopePos(bulletTrajectory.bulletPos, bulletTrajectory).Item3);
               
                if(lineGameObject.Count != distanceLines.Length )
                {
                    var line =Instantiate(LineInsatnce, crosIndicator.parent);
                    line.transform.localPosition = new Vector3(0,0,0);
                    line.transform.localPosition = GetScopePos(bulletTrajectory.bulletPos, bulletTrajectory).Item1;
                    line.transform.localScale = new Vector3(line.transform.localScale.x , line.transform.localScale.y ,distanceLines[y]);
                    line.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text =( distanceLines[y]).ToString() +" M";
                    lineGameObject.Add(line);
                     
                }
                else 
                {
                    StartCoroutine(dis(lineGameObject[y].transform, GetScopePos(bulletTrajectory.bulletPos, bulletTrajectory).Item1));
                }
                
                y ++;
                
    
            }

        }
        
        

        
    }
    IEnumerator dis(Transform trans, Vector3 vec)
    {
        float time = 0;
        Vector3 StartPos = trans.localPosition;
        while(time < 0.5f)
        {
            time += Time.deltaTime;
            trans.localPosition = Vector3.Slerp(StartPos, vec, 0.5f/ time);
            yield return new WaitForEndOfFrame();
        }
    }
    float startPos = 0;
    (Vector3 ,Vector3, float, float) GetScopePos(Vector3 hit, BulletTrajectory bulletTrajectory)
    {

            Vector3 vor = hit - gunCamera.transform.right * (Vector3.Dot(hit- gunCamera.transform.position ,gunCamera.transform.right));
            float DH = Vector3.Distance(gunCamera.transform.position, vor- (GeneralWeaponSettings.Instance.whint * bulletTrajectory.livetime * bulletTrajectory.livetime * 0.5f));
            float F = ScopeCamera.fieldOfView/ 2;
            float angels  = Vector3.Angle(gunCamera.transform.forward, (vor - gunCamera.transform.position).normalized);
            float DR = Mathf.Cos(angels * Mathf.Deg2Rad) * DH;
            
       
            float DNH = Mathf.Sqrt(DH * DH - DR * DR);
            float DN = Mathf.Tan(F * Mathf.Deg2Rad) * DR ;
          
            // Debug.DrawLine(gunCamera.transform.position, vor, Color.yellow);
            // Debug.DrawLine(gunCamera.transform.position, vor, Color.black);
            // Debug.DrawRay(gunCamera.transform.position, gunCamera.transform.forward * DR, Color.black);
            // Debug.DrawLine(gunCamera.transform.forward * DR + gunCamera.transform.position, vor, Color.black);
            // Debug.DrawLine(vor, vor, Color.black);
            // Debug.DrawLine(gunCamera.transform.forward * DR + gunCamera.transform.position, vor, Color.black);

            // Vector3 der =(gunCamera.transform.forward * DR + gunCamera.transform.position  -  vor).normalized;
            // Debug.DrawRay(gunCamera.transform.forward * DR + gunCamera.transform.position - der * DN ,                         der * DN  * 2, Color.magenta);
            // Debug.DrawLine(gunCamera.transform.position,   gunCamera.transform.forward * DR + gunCamera.transform.position +   der * DN , Color.cyan );


            float N = 50;
            float douwn = (N/ DN) * DNH;
            float richt = (N / DN) * (Vector3.Dot(hit- gunCamera.transform.position ,gunCamera.transform.right));



            return (-Vector3.up * (douwn- startPos) , Vector3.right * richt + -Vector3.up * (douwn- startPos), DR, douwn);

    }

}
    

