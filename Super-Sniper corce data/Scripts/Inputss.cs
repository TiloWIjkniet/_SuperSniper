using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inputss : MonoBehaviour
{
    Brains_Player brains_Player; 
    public float currentStamina {get;set;}
    public Vector3 mouseDerection   {get; set;}
    public Vector3 moveDerection    {get; set;}

    public Inputs inputs {get ;private set;}

    public void AwakeInput(Brains_Player _brains_Player)
    {
        brains_Player = _brains_Player;
    }
    public void SetInputs()
    {
        inputs = new Inputs();    
        inputs.Player.Enable();
        inputs.Player.Movment.performed += MovmentPerformed;
        inputs.Player.Mouse.performed += mousePerformed;
        inputs.Player.Run.performed += RunPerformed;
        inputs.Player.Crouch.performed += CrouchPerformed;
        inputs.Player.Proning.performed +=ProningPerformed;
        inputs.Player.Jump.performed += JumpPerformed;

        inputs.Player.Run.canceled += RunPerformedCanceled;
    }
    public void CheckInput()
    {
        mouseDerection = inputs.Player.Mouse.ReadValue<Vector2>();
        moveDerection = inputs.Player.Movment.ReadValue<Vector2>();

        if(inputs.Player.Run.ReadValue<float>() > 0 && currentStamina >= 5 && moveDerection != Vector3.zero )
        {
            if(Input.GetMouseButton(1)) return;
//            Debug.Log(moveDerection);
            brains_Player.stateMachine_Player.ChangeState(Brains_Player.State.run);
        }

        

    }
    public void MovmentPerformed (InputAction.CallbackContext context)
    {
        moveDerection = context.ReadValue<Vector2>();
    }

    public void mousePerformed (InputAction.CallbackContext context)
    {
        mouseDerection = context.ReadValue<Vector2>();
    }

    public void RunPerformed (InputAction.CallbackContext context)
    {

        if(currentStamina <= 0 || UnityEngine.Input.GetMouseButton(1) || moveDerection == Vector3.zero) return;
        brains_Player.stateMachine_Player.ChangeState(Brains_Player.State.run);
    }

    public void CrouchPerformed (InputAction.CallbackContext context)
    {
        if(!brains_Player.isGrounded) return;
        if(brains_Player.generalUsage_Player.GetEnumState(brains_Player.stateMachine_Player.currentState) == Brains_Player.State.crouch)
        {
            brains_Player.stateMachine_Player.ChangeState(Brains_Player.State.walk);
        }
        else
        {
            brains_Player.stateMachine_Player.ChangeState(Brains_Player.State.crouch);
        }
        
    }
    public void JumpPerformed(InputAction.CallbackContext context)
    {
        if(!brains_Player.isGrounded && brains_Player.currentJumps >= brains_Player.maxsJumpsBeforeLanding) return;
        brains_Player.stateMachine_Player.currentState.HandelJump((Brains_Player)brains_Player);
        
    }

    public void ProningPerformed(InputAction.CallbackContext context)
    {
        if(!brains_Player.isGrounded) return;

        if(brains_Player.generalUsage_Player.GetEnumState(brains_Player.stateMachine_Player.currentState) == Brains_Player.State.proning)
        {
            brains_Player.stateMachine_Player.ChangeState(Brains_Player.State.walk);
        }
        else
        {
            brains_Player.stateMachine_Player.ChangeState(Brains_Player.State.proning);
        }
    }

    public void RunPerformedCanceled (InputAction.CallbackContext context)
    {
        if(brains_Player.generalUsage_Player.GetEnumState(brains_Player.stateMachine_Player.currentState) == Brains_Player.State.run)
        {
            brains_Player.stateMachine_Player.ChangeState(Brains_Player.State.walk);
        }
        else
        {
            brains_Player.stateMachine_Player.ChangeState(brains_Player.generalUsage_Player.GetEnumState(brains_Player.stateMachine_Player.currentState));
        }
    }
}
