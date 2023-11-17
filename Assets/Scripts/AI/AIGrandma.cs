using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIGrandma : MonoBehaviour
{
    private float TimeStartedState;             // timer to know when we started a state
    public LayerMask wallLayer;
    public GameObject escapePoint;

    [Header("Agent")]
    public NavMeshAgent grandma;
    public GameObject purse;
    [SerializeField] float stoppedTime = 3f;    // how long the agent will remain in a shop
    [SerializeField] float shopTime = 12f;      // how long the agent will shop
    private int shopList = 0;                   // how many shops grandma needs to visit before she's done shopping
    public bool isMugged = false;
    private Vector3 destination;                // placeholder for any destination the grandma needs
    private float timer;                        // placeholder for timer
    private List<Vector3> shops;
    private int shopsVisited = 0;               // counter for amount of shops grandma has visited
    private GameObject grandmaGO;
    private int preachedToCount = 0;            // counter for amount of times grandma has been preached to
    public Outline outlineScript;
    // Event for when Grandma finishes at each shop
    public delegate void GrandmaFinishedShopping();
    public static event GrandmaFinishedShopping GrandmaFinishedShoppingEvent;
    
    public enum States
    {
        stopped,        // stopped = 0
        shopping,       // shopping = 1
        goinghome,      // goinghome = 2
        mugged          // mugged = 3
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
                grandma.isStopped = true;
                break;
            case States.shopping:
                //Debug.Log("I am " + currentState);
                destination = ShopDestination();                    // gets a destination to go shopping
                break;
            case States.goinghome:
                //Debug.Log("I am " + currentState);
                break;
            case States.mugged:
                //Debug.Log("I am " + currentState);
                grandma.isStopped = true;
                purse.SetActive(false);
                AudioManager.instance.GrandmaMugged();
                break;
        }
    }
    // OnUpdatedState is for things that occur during the state (main actions)
    public void OnUpdatedState(States state) 
    {
        switch (state) 
        {
            case States.stopped:
                if (isMugged)
                {
                    currentState = States.mugged;
                }
                else if (TimeElapsedSince(TimeStartedState, stoppedTime))
                {
                    currentState = States.shopping;
                }
                break;
            case States.shopping:
                if (isMugged)
                {
                    currentState = States.mugged;
                }
                else if (TimeElapsedSince(TimeStartedState, shopTime))
                {
                    shopsVisited++;
                    // Announce that a shop has been visited
                    GrandmaFinishedShoppingEvent?.Invoke();
                    // Debug.Log("Shops visited is " + shopsVisited);
                    AudioManager.instance.GrandmaShops();
                    currentState = States.stopped;
                }
                else if (shopsVisited == shopList)
                {
                    currentState = States.goinghome;
                }
                else
                {
                    grandma.SetDestination(destination);
                }
                break;
            case States.goinghome:
                if (isMugged)
                {
                    currentState = States.mugged;
                }
                else
                {
                    grandma.SetDestination(escapePoint.transform.position);
                    if (DistanceCheck(grandmaGO, escapePoint) < 3)
                    {
                        UIManager.instance.Results();
                    }
                }
                break;
            case States.mugged:
                if (!isMugged)
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
                grandma.isStopped = false;
                break;
            case States.shopping:
                break;
            case States.goinghome:
                break;
            case States.mugged:
                grandma.isStopped = false;
                purse.SetActive(true);
                break;
        }
    }
    
    void Start()
    {
        shopList = LevelManager.instance.GetLevel().requiredVisits;
        
        escapePoint = GameObject.FindWithTag("EscapePoint");
        grandmaGO = gameObject;
        if (UpgradeManager.instance.GetUpgrade("GrandmaSpritzer").isUnlocked)
        {
            outlineScript.enabled = true;
        }
        if (UpgradeManager.instance.GetUpgrade("RocketPoweredScooter").isUnlocked)
        {
            AdjustSpeedBasedOnUpgrades();
        }
        OnStartedState(currentState);
    }

    void Update()
    {
        if (shops == null)
        {
            GetShopsListFromShopManager();
        }
        OnUpdatedState(currentState);
    }

    private void AdjustSpeedBasedOnUpgrades()
    {
        float speedIncrease = Mathf.Clamp01(UpgradeManager.instance.GetUpgrade("RocketPoweredScooter").amount * 0.1f);
        float adjustedSpeed = grandma.speed * (1.0f + speedIncrease);
        grandma.speed = adjustedSpeed;
    }

    private void GetShopsListFromShopManager()
    {
        shops = ShopManager.instance.GetShopPositions();
    }
    private Vector3 ShopDestination()
    {
        if (shops == null || shops.Count == 0)
        {
            if (shops == null)
            {
                Debug.Log("Shops is null.");
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

    public bool GotMugged()
    {
        if (!isMugged)
        {
            isMugged = true;
            return false;
        }
        else if (isMugged)
        {
            return true;
        }
        return false;
    }

    public void PreachToMe()
    {
        Debug.Log("Grandma preached to");
        preachedToCount++;
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