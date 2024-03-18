using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtfanstScope : SimpelScope
{
    public Camera scopCamera;
    public float scopeFiltOvviuw;
    public float minScopeFiltOvviuw;
    public float maxsScopeFiltOvviuw;
    public float[] DistanceLines;
    [HideInInspector]public int scopeInt;

    void Start()
    {

        scopeFiltOvviuw = minScopeFiltOvviuw;
        scopCamera.fieldOfView = scopeFiltOvviuw;
    }
}
