using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRecoil : MonoBehaviour
{
    Vector3 currentRotation;
    Vector3 targetRotation;

    float snappiness;
    float returnSpeed;

    void Awake()
    {

    }

    void Update()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, Time.deltaTime * returnSpeed);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, Time.deltaTime * snappiness);
        transform.localRotation = Quaternion.Euler(currentRotation);
    }

    public void Shood(Vector3 Recoil, float AimMultiplier, float Snappiness, float ReturnSpeed , bool isAim)
    {
        snappiness  = Snappiness;
        returnSpeed = ReturnSpeed;
        if(isAim)   targetRotation += new Vector3(Recoil.x, Random.Range(-Recoil.y * AimMultiplier, Recoil.y ), Random.Range(-Recoil.z ,Recoil.z)) * AimMultiplier ;
        else        targetRotation += new Vector3(Recoil.x, Random.Range(-Recoil.y, Recoil.y), Random.Range(-Recoil.z,Recoil.z));
    }
}
