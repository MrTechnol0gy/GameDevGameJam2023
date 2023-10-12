using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMugger : MonoBehaviour
{
    private float TimeStartedState;             // timer to know when we started a state
    public LayerMask wallLayer;
    public GameObject purse;

    [Header("Agent")]
    public NavMeshAgent mugger;
    public GameObject muggerGO;
    public GameObject grandma;
    public GameObject escapePoint;
    [SerializeField] float stoppedTime = 3f;    // how long the agent will remain stopped
    [SerializeField] float searchTime = 12f;    // how long the agent will search
    [SerializeField] float searchRadius = 30f;  // how large an area the agent will search in
    [SerializeField] float searchTimer = 3f;    // how often the mugger searches for grandma
    [SerializeField] float launchForce = 10f;   // how fast the agent is launched
    [SerializeField] float spinForce = 100f;    // how fast the agent is spun after being launched
    [SerializeField] float destructDelay = 3f;  // how soon after being launched the mugger is destroyed
    private bool isGrandmaVisible = false;      // can the mugger see grandma
    private bool isLaunched = false;            // has the mugger got gotted
    private bool hasMugged = false;             // has the mugger got the purse
    private Vector3 destination;                // placeholder for any destination the mugger needs
    private Rigidbody agentRigidbody;           // placeholder for the mugger's rigidbody
    private float timer;                        // placeholder for timer
    // event for when the mugger is clicked
    public delegate void MuggerClicked();
    public static event MuggerClicked muggerClicked;
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
                mugger.isStopped = true;
                break;
            case States.searching:
                //Debug.Log("I am " + currentState);
                destination = SearchDestination();                    // gets a destination to search towards
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
                else if (isLaunched)
                {
                    currentState = States.launched;
                }
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    timer = searchTimer;
                    isGrandmaVisible = CheckForGrandma();
                }
                if (!isGrandmaVisible)
                {
                    mugger.SetDestination(destination);
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
                else if (isGrandmaVisible)
                {
                    mugger.SetDestination(grandma.transform.position);
                }
                if (DistanceCheck(muggerGO, grandma) < 3)
                {
                    if (!grandma.GetComponent<AIGrandma>().GotMugged())
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
                else
                {
                    mugger.SetDestination(escapePoint.transform.position);
                    if (DistanceCheck(muggerGO, escapePoint) < 3)
                    {
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
                mugger.isStopped = false;
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
    
    void Start()
    {
        grandma = GameObject.FindWithTag("Player");
        muggerGO = gameObject;
        escapePoint = GameObject.FindWithTag("EscapePoint");
        agentRigidbody = GetComponent<Rigidbody>();
        OnStartedState(currentState);
        AudioManager.instance.MuggerSpawn();
    }
    void Update()
    {
        OnUpdatedState(currentState);
    }

    private Vector3 SearchDestination()
    {
        Vector3 randomPoint = Vector3.zero;

        // Get a random point on the NavMesh
        Vector3 randomDirection = Random.insideUnitSphere * searchRadius;
        
        // Try to find a valid point on the NavMesh using the generated position
        if (NavMesh.SamplePosition(transform.position + randomDirection, out NavMeshHit hit, searchRadius, NavMesh.AllAreas))
        {
            //Debug.Log("Random point found.");
            randomPoint = hit.position;
        }
        else
        {
            //Debug.Log("Random point not found.");
            randomPoint = transform.position;
        }
        
        return randomPoint;
    }

    public void Gottem()
    {
        Debug.Log("Got me.");
        isLaunched = true;
        // Disable NavMeshAgent to allow manual control
        mugger.enabled = false;

        // Disable rigidbody gravity to ensure it launches upwards
        agentRigidbody.useGravity = false;

        // Disable the kinematics so that force can be applied directly
        agentRigidbody.isKinematic = false;

        // Apply a vertical force to launch the agent into the air
        agentRigidbody.AddForce(Vector3.up * launchForce, ForceMode.Impulse);

        // Apply a torque to make the agent spin wildly
        agentRigidbody.AddTorque(Random.insideUnitSphere * spinForce, ForceMode.Impulse);

        // Play sfx
        AudioManager.instance.MuggerCaught();
        
        if (hasMugged)
        {
            // Let Grandma know she's got her purse back
            grandma.GetComponent<AIGrandma>().isMugged = false;
            //Debug.Log("Grandma can have her purse back.");
        }
        // Let the UpgradeManager know the Mugger has been clicked
        muggerClicked?.Invoke();            
    }
    private bool CheckForGrandma()
    {
        float distance = Vector3.Distance(transform.position, grandma.transform.position);
        if (distance <= searchRadius/2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public float DistanceCheck(GameObject checker, GameObject target)
    {
        float distance = Vector3.Distance(checker.transform.position, target.transform.position);
        return distance;
    }
    private void DestroyMe()
    {
        Destroy(gameObject, destructDelay);
    }
    
    // This method can be used to test if a certain time has elapsed since we registered an event time. 
    public bool TimeElapsedSince(float timeEventHappened, float testingTimeElapsed) => !(timeEventHappened + testingTimeElapsed > Time.time);
    
    // I use Handles.Label to show a label with the current state above the player. Can use it for more debug info as well.
    // I wrap it around a #if UNITY_EDITOR to make sure it doesn't make its way into the build, unity doesn't like using UnityEditor methods in builds.
    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        //If we're not playing don't draw gizmos.
        if (!Application.isPlaying) return;
        
        //Setting the position for our debug label and the color.
        Vector3 debugPos = transform.position;
        debugPos.y += 2; 
        GUI.color = Color.black;
        UnityEditor.Handles.Label(debugPos,$"{currentState}");
    }
    #endif
}