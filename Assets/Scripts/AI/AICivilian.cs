using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AICivilian : MonoBehaviour
{
    private float TimeStartedState;             // timer to know when we started a state
    public LayerMask wallLayer;

    [Header("Agent")]
    public NavMeshAgent Civilian;
    [SerializeField] float stoppedTime = 3f;    // how long the agent will remain in a shop
    [SerializeField] float shopTime = 12f;      // how long the agent will shop
    private Vector3 destination;                // placeholder for any destination the Civilian needs
    private float timer;                        // placeholder for timer
    private List<Vector3> shops;
    
    public enum States
    {
        stopped,        // stopped = 0
        shopping,       // shopping = 1
        chasing,        // chasing = 2
        goinghome,      // goinghome = 3
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
                Civilian.isStopped = true;
                break;
            case States.shopping:
                //Debug.Log("I am " + currentState);
                destination = ShopDestination();                    // gets a destination to go shopping
                break;
            case States.chasing:
                //Debug.Log("I am " + currentState);
                break;
            case States.goinghome:
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
                    currentState = States.shopping;
                }
                break;
            case States.shopping:
                if (TimeElapsedSince(TimeStartedState, Random.Range(shopTime - 10, shopTime + 10)))
                {
                    currentState = States.stopped;
                }
                else
                {
                    Civilian.SetDestination(destination);
                }
                break;
            case States.chasing:
                break; 
            case States.goinghome:
                break;
        }
    }
    // OnEndedState is for things that should end or change when a state ends; for cleanup
    public void OnEndedState(States state)
    {
        switch (state) 
        {
            case States.stopped:
            //AudioManager.get.CivilianClips();
            Civilian.isStopped = false;
                break;
            case States.shopping:
                break;
            case States.chasing:
                break;
            case States.goinghome:
                break;
        }
    }
    
    void Awake()
    {
       
    }
    void Start()
    {        
        OnStartedState(currentState);
    }

    void Update()
    {
        OnUpdatedState(currentState);
    }
    
    private Vector3 ShopDestination()
    {
        if (shops == null || shops.Count == 0)
        {
            if (shops == null)
            {
                Debug.Log("Shops is null, getting shops");
                shops = ShopManager.instance.GetShopPositions();
                int randomIndex = Random.Range(0, shops.Count);
                return shops[randomIndex];
            }
            else if (shops.Count == 0)
            {
                Debug.Log("Shops is empty");
            }
            return transform.position;
        }
        else
        {
            int randomIndex = Random.Range(0, shops.Count);
            return shops[randomIndex];
        }
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