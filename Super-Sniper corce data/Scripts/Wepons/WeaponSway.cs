using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class WeaponSway : MonoBehaviour
{
    public Quaternion rotationOfset = Quaternion.identity;
    public Vector3    positionOfset = Vector3.zero;


    public float Setp = 0.01f;             
    public float MaxStepDistances  = 0.06f;
    public float SetpRot  = 4f;  
    public float MaxStepRot  =6f;      
    public float Smoot      = 10f;        
    public float Smootrot   = 12f;     

    public float setp               {get{return   gun.manageable * Setp;} set{setp = value;}}
    public float maxStepDistances   {get{return   gun.manageable * MaxStepDistances;} set{maxStepDistances = value;}}

    public float setpRot            {get{return   gun.manageable * SetpRot  ;}   set{setpRot = value;}}
    public float maxStepRot         {get{return   gun.manageable * MaxStepRot;}   set{maxStepRot = value;}}


    public float smoot              {get{return   Smoot - gun.manageable * (Smoot*0.5f) ;} set{smoot = value;}} 
    public float smootrot           {get{return   Smootrot - gun.manageable * (Smootrot * 0.5f) ;}  set{smootrot = value;}}
    Vector3 startPos;



    public Brains_Player brains_Player {get;set;}
    public Gun gun {get;set;}
    
    float manageable;

    void Awake()
    {
        brains_Player = GetComponentInParent<Brains_Player>();
        gun = GetComponentInParent<Gun>();
        startPos= transform.localPosition;

    }
    
    void Update()
    {
       
        NoramlSway();
    }


    void NoramlSway()
    {
        //transform.localPosition = positionOfset;
        Vector3 pos     = positionOfset;
        Quaternion rot  =  rotationOfset;
        


        if(!brains_Player.useCamera){transform.localRotation = rot; return;}
        Vector3 inverLook = brains_Player.mouseDerection *-setp;
        inverLook *= brains_Player.sensitivityX/ 20f;
        inverLook.x = Mathf.Clamp(inverLook.x,-maxStepDistances, maxStepDistances );
        inverLook.y = Mathf.Clamp(inverLook.y,-maxStepDistances, maxStepDistances );

        transform.localPosition = Vector3.Lerp(transform.localPosition - pos , inverLook + startPos, Time.deltaTime * smoot) + pos;

        Vector3 inverLookRot = brains_Player.mouseDerection *-setpRot;
        inverLookRot *=  brains_Player.sensitivityX / 20f;
        inverLookRot.x = Mathf.Clamp(inverLookRot.x,-maxStepRot, maxStepRot );
        inverLookRot.y = Mathf.Clamp(inverLookRot.y,-maxStepRot, maxStepRot );
        inverLookRot = new Vector3( inverLookRot.y ,  inverLookRot.x, inverLookRot.x); 
        
        transform.localRotation = Quaternion.Slerp(transform.localRotation * Quaternion.Inverse(rot) , Quaternion.Euler(inverLookRot),Time.deltaTime * smootrot) * rot;
        positionOfset= Vector3.zero;
        rotationOfset = Quaternion.identity;
    }

    
    public void Hide()
    {
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.localPosition = Vector3.zero;
    }
}
