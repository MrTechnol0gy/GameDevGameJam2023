using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
                {
                int target;
                target = FindTarget();
                    if (target >= 0)
                    {
                        // Find the target and let them know they've been shot
                        spottedEnemy = AIManager.instance.GetEnemies()[target];
                        // Tells the enemy they've been shot by a sniper
                        spottedEnemy.GetComponent<AIVillainBase>().ShotBySniper();
                        // Play the sniper shot sound
                        AudioManager.instance.SniperShot();
                        // Change the state to stopped
                        currentState = States.stopped;
                    }
                    // If there are no enemies on the map to be shot, reset the clock
                    else
                    {
                        currentState = States.stopped;
                    }
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
                break;
            case States.firing:                
                break;
        }
    }
    
    void Start()
    {
        sniperGO = gameObject;
        OnStartedState(currentState);
        AdjustShotDelay();
    }
    void Update()
    {
        OnUpdatedState(currentState);
    }

    private int FindTarget()
    {
        // Get the list of enemies from the AIManager script
        List<GameObject> enemies = AIManager.instance.GetEnemies();
        if (enemies == null)
        {
            Debug.Log("Enemies is null.");
            return - 1;
        }
        Debug.Log("Enemy list length: " + enemies.Count);
        if (enemies.Count > 0)
        {
            // Pick a random enemy from the list
            int randomEnemy = Random.Range(0, enemies.Count);
            // Debug.Log("Enemy is " + randomEnemy);
            return randomEnemy;
        }
        return -1;
    }

    private void AdjustShotDelay()
    {
        // Get the reduction based on the amount of the purchased upgrade
        float reduction = Mathf.Clamp01(UpgradeManager.instance.GetUpgrade("RooftopSniper").amount * 0.1f);
        // Calculate the adjusted shot delay
        float adjustedShotDelay = shotDelay * (1.0f - reduction);
        shotDelay = adjustedShotDelay;
        Debug.Log("Adjusted shot delay: " + shotDelay);
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