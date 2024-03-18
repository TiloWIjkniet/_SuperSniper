using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class HitSettings : MonoBehaviour
{
    
    public GeneralWeaponSettings.HitObjects hitObject;
    public UnityDataEvent hitEvent = new UnityDataEvent();

    public void Hit(RaycastHit hit,float damage)
    {
        
        hitEvent.Invoke(hit, damage);
    }

    public void DestoyScript()
    {
        Destroy(this);
    }


    
}



[System.Serializable]
public class UnityDataEvent: UnityEvent<RaycastHit, float>{}