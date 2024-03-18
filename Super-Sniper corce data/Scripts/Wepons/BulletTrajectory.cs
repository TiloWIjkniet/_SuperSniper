using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletTrajectory
{
    float grafity;
    float surfaces;
    float dragCoefficient;
    public float bulletSpeed {get{return bulletDer.magnitude/ Time.fixedDeltaTime;}}
    public float livetime       {get; private set;}
    public Vector3 bulletPos    {get; private set;}
    public Vector3 bulletDer    {get; set;}
    Vector3 whint;
    LayerMask layerMask;
    Transform GunCamera;

    float DENSITIE_AIR = 1.2f;
    
    public BulletTrajectory(Vector3 BulletDer, Vector3 BulletPos, Vector3 Whint,float Grafity ,float Surfaces, float DragCoefficient , LayerMask LayerMask, Transform gunCamera)
    {
        bulletDer = BulletDer;
        bulletPos = BulletPos;
        whint = Whint;
        grafity = Grafity;
        surfaces  = Surfaces;
        dragCoefficient = DragCoefficient;
        layerMask = LayerMask;
        GunCamera = gunCamera;

    }

    public RaycastHit HitObject(float time)
    {
       
        livetime += time;
        RaycastHit hit;
     

        bulletDer += Vector3.up * - grafity * time;
        bulletDer += whint  * time; 
        bulletDer -= bulletDer.normalized *( 0.5f * bulletDer.magnitude * bulletDer.magnitude * surfaces * DENSITIE_AIR * dragCoefficient) * time;
        
        Physics.Raycast(bulletPos, bulletDer.normalized, out hit, bulletDer.magnitude * time , layerMask);
  

        bulletPos += bulletDer.normalized * bulletDer.magnitude * time;

        return hit;
    }
}
