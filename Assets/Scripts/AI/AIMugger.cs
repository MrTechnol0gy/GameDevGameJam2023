using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMugger : AIVillainBase
{
    public LayerMask wallLayer;
    public GameObject purse;

    [Header("Agent")]
    [SerializeField] float searchRadius = 20;   // how far the agent will search
    [SerializeField] float stoppedTime = 3f;    // how long the agent will remain stopped
    [SerializeField] float searchTime = 12f;    // how long the agent will search
    [SerializeField] float searchTimer = 3f;    // how often the mugger searches for grandma
    [SerializeField] float launchForce = 10f;   // how fast the agent is launched
    [SerializeField] float spinForce = 100f;    // how fast the agent is spun after being launched
    private bool isGrandmaVisible = false;      // can the mugger see grandma
    private bool isLaunched = false;            // has the mugger got gotted
    private bool hasMugged = false;             // has the mugger got the purse
    private Vector3 destination;                // placeholder for any destination the mugger needs
    private float timer;                        // placeholder for timer
    // event for when the mugger is clicked
    public delegate void MuggerClicked();
    public static event MuggerClicked muggerClicked;
    // event for when the mugger escapes
    public delegate void MuggerEscaped();
    public static event MuggerEscaped muggerEscaped;

    public enum States
    {
        stopped,       // stopped = 0
        searching,     // searching = 1
        chasing,       // chasing = 2
        escaping,      // escaping = 3
        launched,      // launched = 4
        mugged         // mugged = 5
    }

    private States _currentState = States.stopped;       //sets the starting enemy state
    public States currentState 
    {
        get => _currentState;
        set {
            if (_currentState != value) 
            {
                // Calling ended state for the previous state registered.
                OnEndedState(_currentState);
                
                // Setting the new current state
                _currentState = value;
                
                // Registering here the time we're starting the state
                TimeStartedState = Time.time;
                
                // Call the started state method for the new state.
                OnStartedState(_currentState);
            }
        }
    }

    // OnStartedState is for things that should happen when a state first begins
    public void OnStartedState(States state)
    {
        switch (state) 
        {
            case States.stopped:
                //Debug.Log("I am " + currentState);
                agent.isStopped = true;
                break;
            case States.searching:
                //Debug.Log("I am " + currentState);
                destination = SearchDestination(searchRadius);                    // gets a destination to search towards
                break;
            case States.chasing:
                //Debug.Log("I am " + currentState);
                break;
            case States.escaping:
                //Debug.Log("I am " + currentState);
                break;
            case States.launched:
                //Debug.Log("I am " + currentState);                
                purse.SetActive(false);
                DestroyMe();
                break;
            case States.mugged:
                //Debug.Log("I am " + currentState);
                purse.SetActive(true);
                break;
        }
    }

    // OnUpdatedState is for things that occur during the state (main actions)
    public void OnUpdatedState(States state) 
    {
        switch (state) 
        {
            case States.stopped:
                if (TimeElapsedSince(TimeStartedState, stoppedTime))
                {
                    currentState = States.searching;
                }
                else if (shotBySniper)
                {
                    Defeated();
                    currentState = States.launched;
                }
                else if (isLaunched)
                {
                    currentState = States.launched;
                }
                break;
            case States.searching:
                if (TimeElapsedSince(TimeStartedState, searchTime))
                {
                    currentState = States.stopped;
                }
                else if (shotBySniper)
                {
                    Defeated();
                    currentState = States.launched;
                }
                else if (isLaunched)
                {
                    currentState = States.launched;
                }
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    timer = searchTimer;
                    isGrandmaVisible = CheckForGrandma(searchRadius);
                }
                if (!isGrandmaVisible)
                {
                    agent.SetDestination(destination);
                }
                else if (isGrandmaVisible)
                {
                    currentState = States.chasing;
                }
                break;
            case States.chasing:
                if (isLaunched)
                {
                    currentState = States.launched;
                }
                else if (shotBySniper)
                {
                    Defeated();
                    currentState = States.launched;
                }
                else if (isGrandmaVisible)
                {
                    agent.SetDestination(target.transform.position);
                }
                if (GetDistanceToTarget(thisGameObject, target) < 3)
                {
                    if (!target.GetComponent<AIGrandma>().GotMugged())
                    {
                        hasMugged = true;
                        Debug.Log("grandma is mugged");
                        currentState = States.mugged;
                    }
                    else
                    {
                        currentState = States.searching;
                    }
                }
                break; 
            case States.escaping:
                if (isLaunched)
                {
                    currentState = States.launched;
                }
                else if (shotBySniper)
                {
                    Defeated();
                    currentState = States.launched;
                }
                else
                {
                    agent.SetDestination(escapePoint.transform.position);
                    if (GetDistanceToTarget(thisGameObject, escapePoint) < 3)
                    {
                        muggerEscaped?.Invoke();
                        UIManager.instance.Results();
                    }
                }
                break;
            case States.mugged:
                if (hasMugged)
                {
                    currentState = States.escaping;
                }
                break;
            case States.launched:
                break;
        }
    }

    // OnEndedState is for things that should end or change when a state ends; for cleanup
    public void OnEndedState(States state)
    {
        switch (state) 
        {
            case States.stopped:
                agent.isStopped = false;
                break;
            case States.searching:
                break;
            case States.chasing:
                break;
            case States.escaping:
                break;
            case States.launched:
                break;
            case States.mugged:
                break;
        }
    }

    protected override void Start()
    {
        base.Start();
        AudioManager.instance.MuggerSpawn();
        OnStartedState(currentState);
    }

    protected override void Update()
    {
        base.Update();
        OnUpdatedState(currentState);
    }

    public void Defeated()
    {
        if (!isLaunched)
        {
            // Get the index of this mugger in the AIManager enemies list
            int muggerIndex = AIManager.instance.GetEnemies().IndexOf(gameObject);
            // Remove this mugger from the list of enemies in the AIManager using the index
            AIManager.instance.RemoveEnemy(muggerIndex);

            // Set the launched flag to true
            isLaunched = true;

            // Disable NavMeshAgent to allow manual control
            agent.enabled = false;

            // Disable rigidbody gravity to ensure it launches upwards
            rb.useGravity = false;

            // Disable the kinematics so that force can be applied directly
            rb.isKinematic = false;

            // Apply a vertical force to launch the agent into the air
            rb.AddForce(Vector3.up * launchForce, ForceMode.Impulse);

            // Apply a torque to make the agent spin wildly
            rb.AddTorque(Random.insideUnitSphere * spinForce, ForceMode.Impulse);

            // Play sfx
            AudioManager.instance.MuggerCaught();

            // Let the UpgradeManager know the Mugger has been clicked
            muggerClicked?.Invoke();            
            if (hasMugged)
            {
                // Let Grandma know she's got her purse back
                target.GetComponent<AIGrandma>().isMugged = false;
            }
        }        
    }    
}