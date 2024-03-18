using System.Collections;
using System.Collections.Generic;
using UnityEngine;
# if UNITY_EDITOR
using UnityEditor;
# endif
using UnityEngine.AI;

public class RifleManArmedPatrol : MonoBehaviour
{
    bool Derection;
    public Vector3 destination;
    RifleMan rifleMan;

    void Awake()
    {
        rifleMan = GetComponent<RifleMan>();
    }
    void Start()
    {
        
        destination = GetDestination();
        rifleMan.navMeshAgent.SetDestination(destination);
    }

    void Update()
    {
           
        if(rifleMan.patrolBackAndForth && !rifleMan.isPartner) AtTheBeginningOrEnd();
        if(Vector3.Distance(transform.position , destination) < 1 && !rifleMan.isPartner)
        {
            destination = GetDestination();
            rifleMan.navMeshAgent.SetDestination(destination);
        }
  
    }

    Vector3 GetDestination()
    {

        if(rifleMan.pathCreator == null && !rifleMan.isPartner)
        {
            Vector3 randomDirection = Random.insideUnitSphere * 300;
            randomDirection += transform.position; 
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, 300, 1);
            return hit.position;
        }

        float length =  rifleMan.pathCreator.path.length;
        float time = rifleMan.pathCreator.path.GetClosestTimeOnPath(transform.position);
        
        Vector3 movTo;
        if(Derection)   movTo = rifleMan.pathCreator.path.GetPointAtDistance( length * time + 5 * rifleMan.walkSpeedArmedPatrol);
        else            movTo = rifleMan.pathCreator.path.GetPointAtDistance( length * time - 5 * rifleMan.walkSpeedArmedPatrol);

        if(rifleMan.patrolPartner != null)
        {
            NavMeshHit hit2;
            NavMesh.SamplePosition(movTo + transform.right * 1.5f, out hit2, 20, NavMesh.AllAreas);
            rifleMan.patrolPartner.navMeshAgent.SetDestination(hit2.position);
        }

        NavMeshHit hit1;
        NavMesh.SamplePosition(movTo, out hit1, 20, NavMesh.AllAreas);
        movTo = hit1.position;

      

        return  movTo;
    }



    void AtTheBeginningOrEnd()
    {
        float time = rifleMan.pathCreator.path.GetClosestTimeOnPath(transform.position);
        
        if(time <= 0.1f)
        {
            Derection = true;
        }
        else if(time >= 0.9f)
        {
            Derection = false;
        }
    }
    

    void OnDrawGizmosSelected()
    {

        if(!Application.isPlaying) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(destination, 0.3f);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(destination, Vector3.up * 100);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, destination);


        Gizmos.color = Color.magenta;
        NavMeshPath navMeshPath = new  NavMeshPath();
        if(NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, navMeshPath))
        {
            Gizmos.DrawLine(transform.position, navMeshPath.corners[0]);
            for (int i = 1; i < navMeshPath.corners.Length ; i++)
            {
                Gizmos.DrawLine(navMeshPath.corners[i - 1], navMeshPath.corners[i]);
            }
        }
    }
}

# if UNITY_EDITOR
[CustomEditor(typeof(RifleManArmedPatrol))]
public class RifleManArmedPatrolEditor: Editor
{
    public override void OnInspectorGUI()
    {

    }
}
# endif