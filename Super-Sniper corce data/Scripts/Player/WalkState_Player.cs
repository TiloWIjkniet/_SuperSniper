using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WalkState_Player : BaseState_Player
{

    public override void EnterState(Brains_Player brains)
    {
        brains.StartCoroutine(col(brains));
    }

    public IEnumerator col(Brains_Player brains)
    {
        for (int i = 0; i < 5; i++)
        {

            brains.image[1].color = Color.Lerp(brains.image[1].color, new Color (255,255,255,0), (float)i/5);
            brains.image[2].color = Color.Lerp(brains.image[2].color, new Color (255,255,255,0), (float)i/5);
            yield return new WaitForSeconds(0.05f);
        }      

        brains.image[1].color =  new Color (255,255,255,0);
        brains.image[2].color =  new Color (255,255,255,0);

        for (int i = 0; i < 5; i++)
        {
            brains.image[0].color = Color.Lerp(brains.image[0].color, new Color (255,255,255,0.355f), (float)i/5);
            yield return new WaitForSeconds(0.05f);
        }  
        brains.image[0].color =  new Color (255,255,255,0.355f);               
    }


    public override void UpdateState(Brains_Player brains)
    {


        brains.currentStamina +=  Time.deltaTime * 0.9f;
        if(brains.useCamera)HandelCamera(brains);


    }

    public override void FixedUpdateState(Brains_Player brains)
    {
        ControlsMovement(brains);
        HandelLocalScale(brains);
        
    }

    public override void HandelJump(Brains_Player brains)
    {
        brains.currentSpeedUp += brains.JumpForce;
        brains.currentJumps ++;
    }

    void HandelLocalScale(Brains_Player brains)
    {
        brains.transform.localScale = brains.generalUsage_Player.GetScaleSize(this);
    }

    void ControlsMovement(Brains_Player brains)
    {
        Vector3 moveDerection = brains.generalUsage_Player.GetMoveDerection(this);
        brains.characterController.Move(moveDerection * Time.fixedDeltaTime);
    }

    
    void HandelCamera(Brains_Player brains)
    {
        brains.mouseX -= brains.mouseDerection.y * brains.sensitivityX * Time.deltaTime;
        brains.mouseX = Mathf.Clamp(brains.mouseX,  -90, 90);

        float mouseY = brains.mouseDerection.x * brains.sensitivityY * Time.deltaTime;
        
        brains.Camera.transform.localRotation   = Quaternion.Euler(brains.mouseX,0,0);
        brains.transform.localRotation         *= Quaternion.Euler(0,mouseY,0);
    }
}
