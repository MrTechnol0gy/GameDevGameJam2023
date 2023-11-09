using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AISniper : MonoBehaviour
{
    private float TimeStartedState;             // timer to know when we started a state
    public LayerMask wallLayer;

    [Header("Agent")]
    public GameObject sniperGO;
    [SerializeField] float shotDelay;           // how long the agent will wait between shots
    private GameObject spottedEnemy;            // the spotted villain
    private float timer;                        // placeholder for timer
    public enum States
    {
        stopped,        // stopped = 0
        firing,         // firing = 1
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
                break;
            case States.firing:
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
                if (TimeElapsedSince(TimeStartedState, shotDelay))
                {
                    currentState = States.firing;
                }
                break;
            case States.firing:
                // TODO
                // 1. Find a target
                FindTarget();
                // 2. Tell the target they've been shot
                // EnemyManager.instance.enemies[0].GetComponent<AIVillainBase>().IAmShot();
                // 3. Go to stopped state
                currentState = States.stopped;
                break;
        }
    }
    // OnEndedState is for things that should end or change when a state ends; for cleanup
    public void OnEndedState(States state)
    {
        switch (state) 
        {
            case States.stopped:
                break;
            case States.firing:
                break;
        }
    }
    
    void Start()
    {
        sniperGO = gameObject;
        OnStartedState(currentState);
    }
    void Update()
    {
        OnUpdatedState(currentState);
    }

    private GameObject FindTarget()
    {
        // Get the list of enemies from the EnemyManager script
        List<GameObject> enemies = EnemyManager.instance.enemies;
        // Debug.Log("Enemy list length: " + enemies.Count);
        if (enemies.Count > 0)
        {
            // Pick a random enemy from the list
            int randomEnemy = Random.Range(0, enemies.Count);
            return enemies[randomEnemy];
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