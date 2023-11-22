using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIWrestler : MonoBehaviour
{
    private float TimeStartedState;             // timer to know when we started a state
    public LayerMask wallLayer;

    [Header("Agent")]
    public NavMeshAgent wrestler;               // placeholder for the agent
    public GameObject wrestlerGO;
    public float distanceToTargetBeforeSuplexing = 1f; // how close the agent needs to be to the target before suplexing
    [SerializeField] float stoppedTime = 1f;    // how long the agent will remain stopped
    private GameObject targetEnemy;             // the targeted villain
    private Rigidbody rb;                       // placeholder for the agent's rigidbody
    public enum States
    {
        stopped,        // stopped = 0
        pursuing,       // pursuing = 1
        suplexing,      // suplexing = 2
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
                wrestler.isStopped = true;
                break;
            case States.pursuing:
                //Debug.Log("I am " + currentState);
                FindEnemyToPursue();
                break;
            case States.suplexing:
                //Debug.Log("I am " + currentState);
                ChildToVillain(targetEnemy);
                // Let the villain know they've been defeated
                targetEnemy.GetComponent<AIVillainBase>().SuplexedByWrestler();
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
                    currentState = States.pursuing;
                }
                break;
            case States.pursuing:
                if (targetEnemy == null)
                {
                    currentState = States.stopped;
                    return;
                }
                wrestler.SetDestination(targetEnemy.transform.position);
                if (DistanceCheck(wrestlerGO, targetEnemy) < 2f)
                {
                    currentState = States.suplexing;
                }
                break;
            case States.suplexing:                
                break;
        }
    }
    // OnEndedState is for things that should end or change when a state ends; for cleanup
    public void OnEndedState(States state)
    {
        switch (state) 
        {
            case States.stopped:
                wrestler.isStopped = false;
                break;
            case States.pursuing:
                break;
            case States.suplexing:
                break;
        }
    }
    
    void Start()
    {
        wrestlerGO = gameObject;
        wrestler = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        OnStartedState(currentState);
    }
    void Update()
    {
        OnUpdatedState(currentState);
    }

    private void OnDestroy() 
    {
        // Remove this Wrestler from the list of Wrestlers in the AIManager, by Index
        int wrestlerIndex = AIManager.instance.GetWrestlers().IndexOf(gameObject);
        AIManager.instance.RemoveWrestler(wrestlerIndex);
    }

    private void FindEnemyToPursue()
    {
        // Get the list of enemies from the AIManager script
        List<GameObject> enemies = AIManager.instance.GetEnemies();
        
        // Choose a random enemy from the list
        int randomEnemy = Random.Range(0, enemies.Count);

        // if the enemy is a Balloon Clown, find a new enemy
        if (enemies[randomEnemy].GetComponent<AIBalloonClown>() != null)
        {
            FindEnemyToPursue();
            return;
        }
        
        // Set the target enemy to the random enemy
        targetEnemy = enemies[randomEnemy];
    }

    private void ChildToVillain(GameObject villain)
    {
        if (villain == null) return;
        else
        {
            // Disable the NavMeshAgent on the wrestler
            wrestler.enabled = false;

            // Diable rigidbody on the wrestler
            rb.isKinematic = true;
            rb.useGravity = false;

            // Set the wrestler's parent to the villain
            wrestler.transform.parent = villain.transform;

        }
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