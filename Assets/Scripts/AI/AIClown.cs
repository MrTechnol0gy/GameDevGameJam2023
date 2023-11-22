using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIBalloonClown : AIVillainBase
{
    public LayerMask wallLayer;

    [Header("Agent")]
    [SerializeField] float stoppedTime = 3f;    // how long the agent will remain stopped
    [SerializeField] float launchForce = 10f;   // how fast the agent is launched
    [SerializeField] float spinForce = 100f;    // how fast the agent is spun after being launched
    private bool isLaunched = false;            // has the agent got gotted
    private Vector3 destination;                // placeholder for any destination the agent needs
    private float timer;                        // placeholder for timer
    // Placeholder for the list of clown stops in the scene
    private List<GameObject> clownStops = new List<GameObject>();
    private SphereSpawner sphereSpawner;
    private bool spheresRemaining = true;
    // event for when the agent is clicked
    public delegate void ClownClicked();
    public static event ClownClicked clownClicked;
    public enum States
    {
        stopped,       // stopped = 0
        goingToCamera, // going to a camera = 1
        launched,      // launched = 2
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
            case States.goingToCamera:
                //Debug.Log("I am " + currentState);
                // Get a random camera from the list of cameras and set it as the destination
                GetRandomStopPoint();
                break;
            case States.launched:
                //Debug.Log("I am " + currentState);
                DestroyMe();
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
                    currentState = States.goingToCamera;
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
            case States.goingToCamera:
                if (shotBySniper || suplexedByWrestler)
                {
                    Defeated();
                    currentState = States.launched;
                }
                else if (isLaunched)
                {
                    currentState = States.launched;
                }  
                else
                {
                    // head towards the camera
                    agent.SetDestination(destination);
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
            case States.goingToCamera:
                break;
            case States.launched:
                break;
        }
    }

    protected override void Start()
    {
        base.Start();
        AudioManager.instance.BalloonClownSpawned();
        OnStartedState(currentState);
        // Get all the clown stops in the scene
        clownStops.AddRange(GameObject.FindGameObjectsWithTag("ClownStop"));
        // Get the SphereSpawner component from a child
        sphereSpawner = GetComponentInChildren<SphereSpawner>();
    }

    protected override void Update()
    {
        base.Update();
        OnUpdatedState(currentState);
    }

    private void GetRandomStopPoint()
    {
        // Get a random clown stop from the list of clown stops
        int randomStop = Random.Range(0, clownStops.Count);
        // Set the destination to the random clown stop
        destination = clownStops[randomStop].transform.position;
    }

    public void Defeated()
    {
        if (!isLaunched)
        {
            // This if statement checks for remaining balloons and prevents the clown from being launched
            if (spheresRemaining)
            {   
                // If there are spheres remaining
                if (sphereSpawner.GetNumberOfSpheres() > 0)
                {
                    sphereSpawner.DestroyRandomSphere();
                    if (sphereSpawner.GetNumberOfSpheres() == 0)
                    {
                        spheresRemaining = false;
                    }
                    return;
                }                
            }
            // Get the index of this Clown in the list of enemies
            int index = AIManager.instance.GetEnemies().IndexOf(thisGameObject);
            
            // Remove this agent from the list of enemies in the AIManager
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
            AudioManager.instance.ClownCaught();

            // Let the UpgradeManager know the agent has been clicked
            clownClicked?.Invoke();
        }        
    }    
}