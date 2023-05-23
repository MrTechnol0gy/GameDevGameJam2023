using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIStateMachine : MonoBehaviour
{
    private float TimeStartedState;             // timer to know when we started a state
    public LayerMask wallLayer;

    [Header("Agent")]
    public NavMeshAgent agent;
    [SerializeField] float stoppedTime = 3f;    // how long the agent will remain stopped
    
    public enum States
    {
        stopped,       // stopped = 0
        searching,     // searching = 1
        chasing,       // chasing = 2
        escaping        // escaping = 3
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
                Debug.Log("I am " + currentState);
                break;
            case States.searching:
                Debug.Log("I am " + currentState);
                break;
            case States.chasing:
                Debug.Log("I am " + currentState);
                break;
            case States.escaping:
                Debug.Log("I am " + currentState);
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
                break;
            case States.chasing:
                break; 
            case States.escaping:
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
            case States.searching:
                break;
            case States.chasing:
                break;
            case States.escaping:
                break;
        }
    }
    
    void Start()
    {
        OnStartedState(currentState);
    }

    void Update()
    {
        OnUpdatedState(currentState);
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