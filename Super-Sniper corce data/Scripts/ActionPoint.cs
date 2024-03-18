using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class ActionPoint : MonoBehaviour
{

    PathCreator pathCreator;
    public AnimationClip animationClip;
    public float time;
    void Awake()
    {
        pathCreator = GetComponentInParent<PathCreator>();
    }

    void Start()
    {
        float time = pathCreator.path.GetClosestTimeOnPath(transform.position);
        pathCreator.ActionTime.Add((time,this.gameObject));
    }


    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.red;
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        Gizmos.matrix = rotationMatrix; 
        Gizmos.DrawWireCube(new Vector3(0,0.5f,0), new Vector3(1,2,1));
        Gizmos.DrawWireCube(new Vector3(0,1f,0.5f), new Vector3(0.8f,0.3f,0.3f));
        
    }

}
