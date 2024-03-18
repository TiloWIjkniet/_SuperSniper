using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attachments1 : MonoBehaviour
{

    public float damage; 
    public float range;       
    public float acuration;     
    public float manageable;
    public float fireRate;     
    public int   ammunitionCapacity;
    public float noice;
    public Weapon weapon; 

    public virtual  void Slect()
    {
        slect();
    }
    
    public virtual  void DeeSlect()
    {
        deeSlect();
    }

    public void slect()
    {
        weapon.damageMin             +=damage;
        weapon.rangeMin              +=range;
        weapon.acurationMin          +=acuration;
        weapon.manageableMin         +=manageable;
        weapon.fireRateMin           +=fireRate;
        weapon.ammunitionCapacityMin +=ammunitionCapacity;
         weapon.noiceMin             +=noice;
    }

    public void deeSlect()
    {
        weapon.damageMin             -=damage;
        weapon.rangeMin              -=range; 
        weapon.acurationMin          -=acuration;
        weapon.manageableMin         -=manageable;
        weapon.fireRateMin           -=fireRate;
        weapon.ammunitionCapacityMin -=ammunitionCapacity;
        weapon.noiceMin              -=noice;
    
    }
}