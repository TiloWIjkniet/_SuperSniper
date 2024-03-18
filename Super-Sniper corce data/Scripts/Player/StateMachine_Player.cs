using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StateMachine_Player : MonoBehaviour
{
    
    Brains_Player brains_Player; 

    [HideInInspector]public Brains_Player.State lastState;
    public BaseState_Player currentState;
    public WalkState_Player walkState_Player        = new WalkState_Player();
    public RunState_Player runState_Player          = new RunState_Player();
    public CrouchState_Player crouchState_Player    = new CrouchState_Player();
    public ProningState_Player proningState_Player  = new ProningState_Player();

    


    void Awake()
    {
        brains_Player = GetComponent<Brains_Player>();
    }

    public void ChangeState(Brains_Player.State state)
    {
        lastState = brains_Player.generalUsage_Player.GetEnumState(currentState);
        currentState = brains_Player.generalUsage_Player.GetScriptState(state);
        
        currentState.EnterState(brains_Player);
    }

    public void ChangeState(BaseState_Player state)
    {
        lastState = brains_Player.generalUsage_Player.GetEnumState(currentState);
        currentState = state;
        currentState.EnterState(brains_Player);
    }

    void Start()
    {
        currentState = brains_Player.generalUsage_Player.GetScriptState(Brains_Player.State.walk);
        currentState.EnterState(brains_Player);
    }

    void Update()
    {
        currentState.UpdateState(brains_Player);
    }

    void FixedUpdate()
    {
        currentState.FixedUpdateState(brains_Player);
    }



}

