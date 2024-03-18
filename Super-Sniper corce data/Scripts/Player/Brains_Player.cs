using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(StateMachine_Player))]
public class Brains_Player : Inputss
{
    // In inspector 
        // Variables
    [Header("Variables")]

            //Camera
    [Header("Camera Setins")]
    [Tooltip("Sensitivity on the Y Axsis")]         public float sensitivityY = 200f;
    [Tooltip("Sensitivity on the X Axsis")]         public float sensitivityX = 200f;

    //Movment
    [Header("Movment Setings")]

    [Tooltip("deceleration in M/S")]                public float deceleration = 10;

    // Jump
    [Tooltip("fall acceleration in M/S")]           public float Grafity = 9.81f;
    [Tooltip("controle in air"), Range(0,0.5f)]     public float inAirCotrol= 0.1f;
    [Tooltip("Jump Force")]                         public float JumpForce = 10f;
    [Tooltip("maxs Jumps Before Landing")]          public int maxsJumpsBeforeLanding = 1;

    //stamina;
    public float maxsStamin = 10;
  
    [Tooltip("Specific settings per state")]        public List<StateSettings> stateSettings = new List<StateSettings>();
    

        // References
    [Header("References")]
    [Tooltip("Reference to player camera")]         public Camera Camera;



    // Not in inspector variables
    public CharacterController characterController  {get; set;}
    public bool  isGrounded                         {get; set;}
    public bool  useCamera                          {get; set;}
    public float currentSpeedUp                     {get; set;}
    public float mouseX                             {get; set;}
    public int   currentJumps                       {get; set;}

    float stepDelay;
    public UnityEngine.UI.Image[] image;
   
    //States
  
    [HideInInspector] public StateMachine_Player stateMachine_Player;
    [HideInInspector] public GeneralUsage_Player generalUsage_Player    = new GeneralUsage_Player();
    [HideInInspector] public enum State                                 {walk, run, crouch, proning };

    void Awake()
    {
 
        characterController = GetComponent<CharacterController>();    
        stateMachine_Player = GetComponent<StateMachine_Player>();

        generalUsage_Player.brains = this;
        AwakeInput(this);
        SetInputs();
    }

    void Start()
    {
        currentStamina = maxsStamin;
        Cursor.lockState = CursorLockMode.Locked;
        stateMachine_Player.ChangeState(State.walk);
        useCamera = true;

        Application.targetFrameRate = 120;
    }

    void Update()
    {   
//        Debug.Log(mouseDerection);
        currentStamina = Mathf.Clamp(currentStamina,0,maxsStamin);        
        if(currentStamina <= 0&& generalUsage_Player.GetEnumState(stateMachine_Player.currentState)== State.run){stateMachine_Player.ChangeState(State.walk);}

        CheckInput();
        HandelGrafity();   
        generalUsage_Player.Update();  
        

    }

    public void Stamina(float valu)
    {
        currentStamina -= valu;
    }



    void FixedUpdate() 
    {
        generalUsage_Player.FixedUpdate();
    }


    void HandelGrafity()
    {   
        if(!isGrounded)
        {
            float TERMINAL_SPEED  = 684;
            if(currentSpeedUp < TERMINAL_SPEED) currentSpeedUp -= Grafity * Time.deltaTime;
            if(generalUsage_Player.measuredSpeedUp == 0)  currentSpeedUp = -0.5f ;
        } 
        else if(currentSpeedUp <= 0)
        {
            currentSpeedUp  = -0.5f;
            currentJumps =0;
        }

        characterController.Move(new Vector3(0,currentSpeedUp,0) * Time.deltaTime);
        isGrounded = characterController.isGrounded;        
    }

    public void PlayerManageable(float manageable)
    {
        for(int i =0; i < stateSettings.Count; i ++)
        {
            stateSettings[i].manageable = manageable;
        }
    }

}

[System.Serializable]
public class StateSettings
{

    [Tooltip("Disply state name"),SerializeField] Brains_Player.State state;
    [Tooltip("Speed in M/S"), SerializeField]  float Speed = 5;

    [Tooltip("velocity added in M/S"), Range(3,20) , SerializeField] float Acceleration = 10;
    [Tooltip("speed Scale factor"), SerializeField] float timeVorScale = 1;
    [Tooltip("Scale")] public Vector3 scale = Vector3.zero;


    [HideInInspector]public float manageable = 1;
    [HideInInspector]public float speed     
    {
        get { return Speed / (1 +manageable * (1f/3f) );}
        set { speed = value; }
    }

    [HideInInspector]public float acceleration
    {
        get { return Acceleration / (1 +manageable * (1f/3f) ); }
        set { acceleration = value; }
    }

    [HideInInspector]public float TimeVorScale 
    {
        get { return 1/ timeVorScale; }
        set { timeVorScale = value; }
    }
}

