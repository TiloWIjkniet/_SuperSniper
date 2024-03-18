using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breathing : MonoBehaviour
{
    public Brains_Player brains_Player;
    public WeaponSway weaponSway;
    public Sniper sniper;

    float time = 0;
    Vector3 noce;
    void Update()
    {
       
        if(sniper.isShiftAiming)
        {
            Brains_Player.State state = brains_Player.generalUsage_Player.GetEnumState(brains_Player.stateMachine_Player.currentState);
            if(state == Brains_Player.State.walk) brains_Player.Stamina(2 * Time.deltaTime);
            else if(state == Brains_Player.State.crouch) brains_Player.Stamina(1.8f * Time.deltaTime);
            else if(state == Brains_Player.State.proning) brains_Player.Stamina(1.5f * Time.deltaTime);
        }
        else if(!sniper.isChangeLodaut)
        {
            time += Time.deltaTime;
            noce += AddNoise(-0.0001f,0.0001f);
            noce = Vector3.ClampMagnitude(noce,0.01f);
            weaponSway.positionOfset += new Vector3( 0,Mathf.Sin(time) * 0.001f,0) + noce;
            weaponSway.rotationOfset *= Quaternion.Euler( Mathf.Sin(1.2f * time)/ 4, 0 ,0) * Quaternion.Euler(noce);
        }
            
        

    }

     Vector3 AddNoise ( float min, float max)  
     {

        float xNoise = Random.Range (min, max);
        float yNoise = Random.Range (min, max);
        float zNoise = Random.Range (min, max);

        return new Vector3(xNoise,yNoise,zNoise);
     }
}
