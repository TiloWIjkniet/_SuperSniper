using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralUsage_Player 
{
    public Brains_Player brains;
    
    public void Update()
    {
        GetCurrentSpeedY();
    }

    public void FixedUpdate()
    {
        GetCurrentSpeed();
    }


    /// <summary>
    ///  converds eneum status in to script
    /// </summary>
    public BaseState_Player GetScriptState(Brains_Player.State state)
    {
        if(state        == Brains_Player.State.walk)        return brains.stateMachine_Player.walkState_Player;
        else if(state   == Brains_Player.State.run)         return brains.stateMachine_Player.runState_Player;
        else if(state   == Brains_Player.State.crouch)      return brains.stateMachine_Player.crouchState_Player;
        else if(state   == Brains_Player.State.proning)     return brains.stateMachine_Player.proningState_Player;
        else 
        {  
            Debug.LogError("Passed enum has no associated script");
            return null;
        }
    }


    /// <summary>
    /// converds script in to eneum status
    /// </summary>
    public Brains_Player.State GetEnumState(BaseState_Player state)
    {
        if(state        == brains.stateMachine_Player.walkState_Player)     return Brains_Player.State.walk;
        else if(state   == brains.stateMachine_Player.runState_Player)      return Brains_Player.State.run;
        else if(state   == brains.stateMachine_Player.crouchState_Player)   return Brains_Player.State.crouch;
        else if(state   == brains.stateMachine_Player.proningState_Player)  return Brains_Player.State.proning;
        else
        {  
            Debug.LogError("Passed script has no associated enum");
            return Brains_Player.State.walk;
        }
    }

    public int GetStateAsInt(Brains_Player.State state)
    {
        return (int)state;
    }

        public int GetStateAsInt(BaseState_Player state)
    {
        if(state        == brains.stateMachine_Player.walkState_Player)     return (int)Brains_Player.State.walk;
        else if(state   == brains.stateMachine_Player.runState_Player)      return (int)Brains_Player.State.run;
        else if(state   == brains.stateMachine_Player.crouchState_Player)   return (int)Brains_Player.State.crouch;
        else if(state   == brains.stateMachine_Player.proningState_Player)  return (int)Brains_Player.State.proning;
        else return 0;
    }


    public Vector3 lastMoveDerecrion;
    /// <summary>
    /// get direction to move in
    /// </summary>
    public Vector3 GetMoveDerection (BaseState_Player state)
    {   
        
        Vector3 moveDerection;
        if(brains.isGrounded)
        {
            moveDerection = GetMoveDerectionGrounded();
        }
        else
        {
            moveDerection = GetMoveDerectionNotGrounded();
        }

        lastMoveDerecrion   = moveDerection;
        moveDerection       = moveDerection * GetSpeed(state);
        return moveDerection;
    }

    float GetSpeed(BaseState_Player state)
    {
        float currentSpeed  =   fixedSpeed;
        float wantedSpeed   =   brains.stateSettings[(int)GetEnumState(state)].speed;
        float acceleration  =   brains.stateSettings[(int)GetEnumState(state)].acceleration;

        wantedSpeed        *= Mathf.Abs(brains.moveDerection.y) + Mathf.Abs(brains.moveDerection.x) >= 1? 1: 0;
        

        if(acceleration < 3){ Debug.LogError("acceleration is too low for result"); return 0;}

        float accelerationFactor = 1 / Mathf.Abs(wantedSpeed - currentSpeed) * acceleration;
        accelerationFactor      *= brains.isGrounded? 1 : brains.inAirCotrol * 5;

        return Mathf.Lerp(currentSpeed , wantedSpeed, accelerationFactor * Time.fixedDeltaTime);
    }

    Vector3 GetMoveDerectionGrounded()
    {
        Vector3 moveDerection   = Vector3.zero;
        moveDerection           = brains.transform.forward    * brains.moveDerection.y;
        moveDerection          += brains.transform.right      * brains.moveDerection.x;
        moveDerection           = moveDerection.normalized;

        if(moveDerection == Vector3.zero && fixedSpeed != 0)
        {
            float decelerationFactor = 1/Mathf.Abs(0 - fixedSpeed) * brains.deceleration;
            return Vector3.Lerp(lastMoveDerecrion, moveDerection , Time.fixedDeltaTime * decelerationFactor);
        }

        return moveDerection;
    }
    Vector3 GetMoveDerectionNotGrounded()
    {
        Vector3 moveDerection    = lastMoveDerecrion;
        moveDerection           += brains.transform.forward    * brains.moveDerection.y  * brains.inAirCotrol;
        moveDerection           += brains.transform.right      * brains.moveDerection.x  * brains.inAirCotrol;
        return moveDerection.normalized;
    }


    /// <summary>
    /// lerps between current scale in set scale of current seane. in a time indicated by the current seane
    /// </summary>
    public Vector3 GetScaleSize(BaseState_Player seane)
    {
        int enumState       = (int)GetEnumState(seane);
        float TimeVorScale  = brains.stateSettings[enumState].TimeVorScale;
        return Vector3.Lerp(brains.transform.localScale, brains.stateSettings[enumState].scale, Time.deltaTime * TimeVorScale);
    }
    
    Vector3 LastPos;
    /// <summary>
    /// the horizontal and vertical speed of the player
    /// </summary>
    public float fixedSpeed;
    /// <summary>
    /// Mumust be called by Update
    /// </summary>
    float GetCurrentSpeed()
    {
        float speed = Vector3.Distance(LastPos, Vector3.Scale(brains.transform.position, new Vector3(1,0,1)));
        LastPos     = Vector3.Scale(brains.transform.position, new Vector3(1,0,1)); 
        fixedSpeed  = speed/Time.fixedDeltaTime;
        return fixedSpeed;
    }

    /// <summary>
    /// the speed Up of the player
    /// </summary>
    public float measuredSpeedUp;
    Vector3 LastPosY;
    /// <summary>
    /// Mumust be called by Update
    /// </summary>
    float GetCurrentSpeedY()
    {
        float speed = Vector3.Distance(LastPosY, Vector3.Scale(brains.transform.position, new Vector3(0,1,0)));
        speed   /= Time.deltaTime;
        speed       *= LastPosY.y > brains.transform.position.y ? -1: 1;
        LastPosY     = Vector3.Scale(brains.transform.position, new Vector3(0,1,0)); 

        measuredSpeedUp = speed;

        return measuredSpeedUp;
    }

}
