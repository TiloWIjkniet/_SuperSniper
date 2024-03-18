using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine. AI;

public class TEST_Enmey : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    float toDer;
    public float HP = 80;
    Vector3 startPos;
    Material mat;
    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        mat = gameObject.GetComponent<Renderer>().material;
    }

    void Start()
    {
        moveToward = GetMoveDerection();
        navMeshAgent.Move(moveToward) ;
        navMeshAgent.SetDestination(moveToward);
        
    }
    bool doot = false;
    Vector3 moveToward;
    void Update()
    {
        if(doot) Day();
        toDer += Time.deltaTime;
        if(Vector3.Distance(transform.position, moveToward) <= 30 || toDer > 30)
        {
            moveToward =   GetMoveDerection();
            navMeshAgent.SetDestination(moveToward);
        }
    }

    Vector3 GetMoveDerection()
    {   toDer = 0;
            Vector3 randomDirection = Random.insideUnitSphere * 300;
            randomDirection += transform.position; 
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, 300, 1);
            return hit.position;
         
    }

    public void Hitt(RaycastHit hit, float damige)
    {

        HP -= damige;
        mat.color = Color.Lerp(Color.red, Color.black,1- HP/80 );
        if(HP <= 0) 
        {   
            doot = true;
            Destroy(this.gameObject,1);
        }
    }

        public void Hittt(RaycastHit hit, float damige)
    {

        HP -= damige * 3;
        mat.color = Color.Lerp(Color.red, Color.black,1- HP/80);
        Debug.Log(HP);
        if(HP <= 0) 
        {   
            doot = true;
            Destroy(this.gameObject,1);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(moveToward, 1);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(moveToward,  moveToward + Vector3.up * 100);
    }

    void Day() 
    {
        mat.color = Color.Lerp(mat.color, new Color(0,0,0,0), Time.deltaTime);
        
    }
}
