using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using PathCreation;
using UnityEngine.Animations.Rigging;
using Tilos_NS;

public class Enemy : MonoBehaviour
{
    public enum MoveState {idle, walk, run}

    public AudioClip [] audioClips;
    AudioSource audioSource;
    public float patrolDistance;
    public float maxsSpecialTime = 220;
    public float MinSpecialTime = 60;
    public float walkSpeed;
    public float crouchSpeed;
    public float proneSpeed;
    public float runSpeed;
    public GameObject Gun;
    public PathCreator pathCreator;
    public TwoBoneIKConstraint LeftConstant;


    Animator animator;
    NavMeshObstacle navMeshObstacle;
    NavMeshAgent navMeshAgent;
    MoveState moveState = MoveState.walk;

    public bool dood {get;set;}
    float specialTime = 0;
    float time=0;
    Vector3 movTo = Vector3.zero;

    void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshObstacle = GetComponent<NavMeshObstacle>();

    }
    void Start()
    {
        dood = false;
        navMeshObstacle.enabled = false;
        navMeshAgent.enabled = true;
       // time = Random.Range(0,100);
       transform.position = GetNextWalkPos();
        
        
        
    }




    void Update()
    {
        animator.SetFloat("Speed", GetSpeed());

        if(Vector3.Distance(transform.position, movTo) <= patrolDistance)
        {
            if(moveState == MoveState.run) moveState = MoveState.walk;
            if(navMeshAgent.enabled)
            {
                navMeshAgent.SetDestination(transform.position);
                navMeshAgent.SetDestination(GetNextWalkPos());
                Debug.DrawRay(transform.position, Vector3.up * 100 , Color.blue, 10);
            }
        }
        else if(Vector3.Distance(transform.position, movTo) > patrolDistance * 2) 
        {
            moveState = MoveState.run;
        }
        
        if(specialTime < time) Special();

        if(moveState == MoveState.idle)       Idle();
        else if (moveState == MoveState.walk) Walk();
        else if (moveState == MoveState.run)  Run();


        

//        animator.SetBool("Jump", navMeshAgent.isOnOffMeshLink);
   

    }



    void Special()
    {
    
        specialTime = time + Random.Range(MinSpecialTime, maxsSpecialTime);
//        animator.SetFloat("Special",Random.Range(1,3));
        moveState = MoveState.idle;
        StartCoroutine(SetWalkState());
        StartCoroutine(SetNavMeshAgent(false));
    }
    
    public void stap()
    {
        audioSource.clip = audioClips[Random.Range(0,audioClips.Length - 1)];
        audioSource.Play();
    }




    void Idle()
    {

        time += Time.deltaTime;
        navMeshAgent.speed = 0;
    }

    void Walk()
    {
        time += Time.deltaTime;
        navMeshAgent.speed = walkSpeed;
    }

    void Run()
    {
        time += Time.deltaTime;
        navMeshAgent.speed =runSpeed;
    }

    Vector3 GetNextWalkPos()
    {
        NavMeshPath navMeshPath = new NavMeshPath();
        
        
        Vector3 randomDirection = Random.insideUnitSphere * 2000;
        randomDirection += transform.position; 
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, 2000, 1);
        
        navMeshAgent.CalculatePath(movTo, navMeshPath);
       
        if(navMeshPath.status != NavMeshPathStatus.PathComplete) movTo =  hit.position;
        
        
        return movTo;
    

    }




    IEnumerator SetWalkState()
    {
        yield return new WaitForSeconds (0.5f);
        yield return new WaitForSeconds (animator.GetCurrentAnimatorStateInfo(0).length - 0.5f);

        
//        animator.SetFloat("Special",0);
        StartCoroutine(SetNavMeshAgent(true));
        moveState = MoveState.walk;
        


    }

    IEnumerator SetNavMeshAgent(bool value)
    {
        if(value)
        {
            navMeshObstacle.enabled = false;
            yield return null;
            navMeshAgent.enabled = true;    
                            navMeshAgent.SetDestination(transform.position);
                navMeshAgent.SetDestination(GetNextWalkPos());
                Debug.DrawRay(transform.position, Vector3.up * 100 , Color.blue, 10);
            
        }
        else
        {
            navMeshAgent.enabled = false;
            yield return null;
            navMeshObstacle.enabled = true;
        }
    }

    Vector3 lastPos;
    float speed = 1 ;

    float GetSpeed()
    {
        float CuretnSpeed =(transform.position - lastPos).magnitude;
        if( Time.deltaTime == 0) return 0;
        lastPos = transform.position;
        speed =  Mathf.Lerp(speed, CuretnSpeed / Time.deltaTime , 2 * Time.deltaTime);
//                Debug.Log(speed);
        return speed ;
    }

    void OnDrawGizmos()
    {

        if(!Application.isPlaying) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(movTo, 0.3f);

        Gizmos.color = Color.red;

        Gizmos.DrawRay(movTo, Vector3.up * 100);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, movTo);


        Gizmos.color = Color.magenta;
        NavMeshPath navMeshPath = new NavMeshPath();
        if(NavMesh.CalculatePath(transform.position, movTo, NavMesh.AllAreas, navMeshPath))
        {
            Gizmos.DrawLine(transform.position, navMeshPath.corners[0]);
            for (int i = 1; i < navMeshPath.corners.Length ; i++)
            {

                Gizmos.DrawLine(navMeshPath.corners[i - 1], navMeshPath.corners[i]);
            }
        }
    }



    public void GetDamige(RaycastHit hit, float damage)
    {
        dood = true;
        moveState = MoveState.idle;
        StartCoroutine(SetNavMeshAgent(false));
        DropGun();
        animator.enabled = false;
        Rigidbody[] rigidBodys = GetComponentsInChildren<Rigidbody>();
        for (int i = 0; i < rigidBodys.Length; i++)
        {
            rigidBodys[i].isKinematic = false;
        }
        Destroy(this,0.1f);
    }


    public void DropGun()
    {
        if(Gun == null) return;
        Gun.AddComponent<Rigidbody>().mass = 20;
        Gun.transform.SetParent(null);
        Gun = null;
        
    }

    public void WeaponlessCalm()
    {
        //animator.runtimeAnimatorController = weaponlessCalm;
        LeftConstant.weight = 0;
    }
}
