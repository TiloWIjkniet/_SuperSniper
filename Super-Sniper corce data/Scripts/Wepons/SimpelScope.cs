using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpelScope : Attachments1
{
    public float aimFieldOfView;
    public float sensitivity;
    float setAimFieldOfView;
    public Gun gun;
    public Transform crosAir;
    public override void Slect()
    {
        setAimFieldOfView = gun.aimFieldOfView;
        gun.aimFieldOfView = aimFieldOfView;
        gun.crosAirPos = crosAir;
        gun.SetSensitivity(sensitivity);
        slect();
    }

    public override void DeeSlect()
    {
        gun.crosAirPos = null;
        gun.aimFieldOfView = setAimFieldOfView;
        gun.SetSensitivity(20);
        deeSlect();
    }
}
