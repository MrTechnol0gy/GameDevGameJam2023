using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIGuard : MonoBehaviour
{
    private float TimeStartedState;             // timer to know when we started a state
    public LayerMask wallLayer;

    [Header("Agent")]
    public NavMeshAgent guard;                 // placeholder for the agent
    public GameObject guardGO;
    [SerializeField] float stoppedTime = 3f;    // how long the agent will remain stopped
    [SerializeField] float searchTime = 12f;    // how long the agent will search
    [SerializeField] float searchRadius = 30f;  // how large an area the agent will search in
    [SerializeField] float searchTimer = 3f;    // how often the guard searches
    private GameObject spottedEnemy;            // the spotted villain
    private Vector3 destination;                // placeholder for any destination the agent needs to go to
    private float timer;                        // placeholder for timer
    public enum States
    {
        stopped,       // stopped = 0
        searching,     // searching = 1
        spotted,       // spotted = 2
    }
    private States _currentState = States.stopped;       //sets the starting AI state
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
                guard.isStopped = true;
                break;
            case States.searching:
                //Debug.Log("I am " + currentState);
                destination = SearchDestination();                    // gets a destination to search towards
                break;
            case States.spotted:
                //Debug.Log("I am " + currentState);
                // Call the I Am Spotted method on the spotted villain
                spottedEnemy.GetComponent<AIVillainBase>().IAmSpotted();
                // Audio cue
                AudioManager.instance.VillainSpotted();
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
                break;
            case States.searching:
                if (TimeElapsedSince(TimeStartedState, searchTime))
                {
                    currentState = States.stopped;
                }
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    timer = searchTimer;
                    Debug.Log("Checking for enemies.");
                    spottedEnemy = CheckForEnemies();
                    if (spottedEnemy != null)
                    {
                        Debug.Log("Spotted enemy." + spottedEnemy.name);
                        currentState = States.spotted;
                    }
                } 
                guard.SetDestination(destination);                   // sets the agent's destination to the destination             
                break;
            case States.spotted:
                // wait 3 seconds and then go to Stopped
                if (TimeElapsedSince(TimeStartedState, stoppedTime))
                {
                    currentState = States.stopped;
                }
                break;
        }
    }
    // OnEndedState is for things that should end or change when a state ends; for cleanup
    public void OnEndedState(States state)
    {
        switch (state) 
        {
            case States.stopped:
                guard.isStopped = false;
                break;
            case States.searching:
                break;
            case States.spotted:
                break;
        }
    }
    
    void Start()
    {
        guardGO = gameObject;
        guard = GetComponent<NavMeshAgent>();
        OnStartedState(currentState);
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

    private GameObject CheckForEnemies()
    {
        // Get the list of enemies from the AIManager script
        List<GameObject> enemies = AIManager.instance.enemies;
        Debug.Log("Enemy list length: " + enemies.Count);
        // Loop through the list of enemies
        foreach (GameObject enemy in enemies)
        {
            // Debug distance
            Debug.Log("Distance to enemy: " + DistanceCheck(guardGO, enemy));
            // Check if the enemy is within the search radius
            if (DistanceCheck(guardGO, enemy) <= searchRadius)
            {
                
                Debug.Log("Enemy spotted!");
                return enemy;
            }
        }
        return null;
    }
    public float DistanceCheck(GameObject checker, GameObject target)
    {
        float distance = Vector3.Distance(checker.transform.position, target.transform.position);
        return distance;
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