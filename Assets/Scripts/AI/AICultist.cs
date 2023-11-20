using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AICultist : AIVillainBase
{
    public LayerMask wallLayer;

    [Header("Agent")]
    [SerializeField] float searchRadius = 20;   // how far the agent will search
    [SerializeField] float stoppedTime = 3f;    // how long the agent will remain stopped
    [SerializeField] float searchTime = 12f;    // how long the agent will search
    [SerializeField] float searchTimer = 3f;    // how often the agent searches for grandma
    [SerializeField] float launchForce = 10f;   // how fast the agent is launched
    [SerializeField] float spinForce = 100f;    // how fast the agent is spun after being launched
    private bool isGrandmaVisible = false;      // can the agent see grandma
    private bool isLaunched = false;            // has the agent got gotted
    private bool hasPreached = false;           // has the agent preached to grandma
    private GameObject closestCivilian;         // placeholder for the closest civilian 
    private Vector3 destination;                // placeholder for any destination the agent needs
    private float timer;                        // placeholder for timer
    // event for when the mugger is clicked
    public delegate void CultistClicked();
    public static event CultistClicked cultistClicked;
    public enum States
    {
        stopped,       // stopped = 0
        searching,     // searching = 1
        chasing,       // chasing = 2
        launched,      // launched = 3
        preached       // preached = 4
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
            case States.launched:
                //Debug.Log("I am " + currentState);
                DestroyMe();
                break;
            case States.preached:
                //Debug.Log("I am " + currentState);
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
                else if (shotBySniper || suplexedByWrestler)
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
                else if (shotBySniper || suplexedByWrestler)
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
                    // Get the closest civilian
                    closestCivilian = GetClosestCivilian(AIManager.instance.GetCivilians());
                    agent.SetDestination(closestCivilian.transform.position);
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
                else if (shotBySniper || suplexedByWrestler)
                {
                    Defeated();
                    currentState = States.launched;
                }
                else if (isGrandmaVisible)
                {
                    agent.SetDestination(target.transform.position);
                    if (GetDistanceToTarget(thisGameObject, target) < 3)
                    {
                        // let the target know they've been preached to
                        target.GetComponent<AIGrandma>().PreachToMe();
                        currentState = States.preached;
                    }
                }
                else if (!isGrandmaVisible)
                {
                    agent.SetDestination(closestCivilian.transform.position);
                    if (closestCivilian != null && (GetDistanceToTarget(thisGameObject, closestCivilian) < 3))
                    {
                        // let the civilian know they've been preached to
                        closestCivilian.GetComponent<AICivilian>().PreachToMe();
                        currentState = States.preached;
                    }
                }
                else
                {
                    currentState = States.searching;
                }
                break; 
            case States.preached:
                if (TimeElapsedSince(TimeStartedState, stoppedTime))
                {
                    currentState = States.stopped;
                }
                else if (shotBySniper || suplexedByWrestler)
                {
                    Defeated();
                    currentState = States.launched;
                }
                else if (isLaunched)
                {
                    currentState = States.launched;
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
            case States.launched:
                break;
            case States.preached:
                break;
        }
    }

    protected override void Start()
    {
        base.Start();
        AudioManager.instance.CultistSpawn();
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
            // Get the index of this enemy in the list of enemies
            int index = AIManager.instance.GetEnemies().IndexOf(thisGameObject);

            // Remove this mugger from the list of enemies in the AIManager
            AIManager.instance.RemoveEnemy(index);

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
            AudioManager.instance.CultistCaught();

            // Let the UpgradeManager know the Mugger has been clicked
            cultistClicked?.Invoke();
        }        
    }    
}