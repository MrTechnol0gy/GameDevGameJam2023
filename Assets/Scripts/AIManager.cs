using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class AIManager : MonoBehaviour
{
    public static AIManager instance;
    [Header("Villain Stats")]
    public GameObject muggerPrefab; // Prefab to instantiate
    public float spawnIntervalFloor = 6f; // Interval between spawns
    public float spawnIntervalCeiling = 12f;
    public GameObject cultistPrefab;    // Prefab to instantiate
    public float cultistSpawnIntervalFloor = 6f; // Interval between spawns
    public float cultistSpawnIntervalCeiling = 12f;
    public GameObject clownPrefab;      // Prefab to instantiate
    public float clownSpawnIntervalFloor = 6f; // Interval between spawns
    public float clownSpawnIntervalCeiling = 12f;
    [Header("Grandma Stats")]
    public GameObject grandmaPrefab;    // Prefab to instantiate
    public GameObject rocketPoweredGrandmaPrefab;    // Prefab to instantiate
    private GameObject grandma;         // Reference to the grandma object
    [Header("Upgrade Stats")]
    public GameObject guardPrefab;      // Prefab to instantiate
    private int amountOfGuards;         // amount of guards
    private List<GameObject> guards = new List<GameObject>(); // List of guards
    public GameObject wrestlerPrefab;   // Prefab to instantiate
    public int numOfVillainsBeforeWrestlerSpawn = 5; // Number of villains before wrestler spawns
    private List<GameObject> wrestlers = new List<GameObject>(); // List of wrestlers
    public int timeBetweenWrestlerSpawnChecks = 3; // Time between wrestler spawn checks
    [Header("Civilian Stats")]
    public GameObject civilianPrefab;   // Prefab to instantiate
    private int numOfCivvies;        // amount of civvies
    [Header("Other Components")]
    public GameObject mallFloor;        // Reference to the mall floor object
    private GameObject mallEntrance;    // Reference to the mall entrance object
    private Vector3 entrancePosition;   // Position of the mall entrance
    private float timer = 0f;           // Timer for checks

    // A list of all the enemies in the scene
    private List<GameObject> enemies = new List<GameObject>();

    private void Awake()
    {
        // Check if there is an instance of the AIManager
        if (instance == null)
        {
            // If not, set the instance to this
            instance = this;
        }
        else
        {
            // If there is, destroy this object
            Destroy(gameObject);
        }

        // Make sure this object persists between scenes
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        // Listen for the GameStarted event
        // This expression is used to make sure the scene is loaded before the event is invoked
        // GameManager.GameStarted += () => StartCoroutine(OnGameStarted());
        SceneManager.sceneLoaded += OnSceneLoaded;
        // Start the timer
        timer = Time.time;
    }

    private void Update()
    {
        // Update the timer
        timer += Time.deltaTime;
        if (timer >= timeBetweenWrestlerSpawnChecks)
        {
            //Debug.Log("Checking for wrestlers...");
            // Check the number of wrestlers in the scene
            if (wrestlers.Count == 0)
            {
                // Check the number of villains in the scene
                // Spawn a wrestler if necessary
                GetVillainCount();
            }
            // Reset the timer
            timer = 0f;
        }        
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // If the current scene is not the main menu scene...
        if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("MainMenu"))
        {
            // Find the floor object by tag
            mallFloor = GameObject.FindGameObjectWithTag("Floor");

            if (mallFloor != null)
            {
                Debug.Log("Floor found!");
            }
            else
            {
                Debug.LogError("Floor not found!");
            }

            // Find the mall entrance by tag
            mallEntrance = GameObject.FindGameObjectWithTag("EscapePoint");

            // Set the amount of guards
            amountOfGuards = UpgradeManager.instance.GetUpgrade("SecurityGuard").amount;

            // Set the amount of civvies
            numOfCivvies = LevelManager.instance.GetLevel().numOfCivvies;
            
            // Get the entrance position
            GetEntranceForSpawning();

            // Start spawning AIs
            StartSpawning();            
        }
        else
        {
            // Cancel all invokes when the main menu scene is loaded
            CancelInvoke();
            // Clear the list of enemies between rounds
            enemies.Clear();
            // Clear the list of guards between rounds
            guards.Clear();
            // Clear the list of wrestlers between rounds
            wrestlers.Clear();
            // Clear the amount of Civvies
            numOfCivvies = 0;
            // Reset the amount of guards
            amountOfGuards = UpgradeManager.instance.GetUpgrade("SecurityGuard").amount;
            // Clear all references
            grandma = null;
            mallFloor = null;
            mallEntrance = null;
        }
    }

    // Start spawning all scene AIs
    private void StartSpawning()
    {
        // Wait for the Mugger spawn interval floor number of seconds before beginning to spawn
        InvokeRepeating(nameof(SpawnMugger), spawnIntervalFloor, Random.Range(spawnIntervalFloor, spawnIntervalCeiling));
        SpawnGrandma();
        while (numOfCivvies != 0)
        {
            SpawnCivvie();
            numOfCivvies--;
        }
        while (amountOfGuards != 0)
        {
            SpawnGuard();
            amountOfGuards--;
        }
    }

    private void SpawnGrandma()
    {
        // Check the Upgrade Manager to see if the Rocketpowered Scooter has been purchased
        if (UpgradeManager.instance.GetUpgrade("RocketPoweredScooter").isUnlocked)
        {
            // If it has, spawn the rocketPoweredGrandma at the entrance
            grandma = Instantiate(rocketPoweredGrandmaPrefab, entrancePosition, Quaternion.identity);
        }
        else
        {
            // If it hasn't, spawn the basic grandma at the entrance
            grandma = Instantiate(grandmaPrefab, entrancePosition, Quaternion.identity);
        }
    }
    private void SpawnGuard()
    {
        // Get a random point on the NavMesh
        Vector3 randomPosition = GetRandomNavMeshPosition();

        // Instantiate the Guard prefab at the random position
        GameObject newGuard = Instantiate(guardPrefab, randomPosition, Quaternion.identity);

        // Add the new Guard to the list of guards
        guards.Add(newGuard);
    }

    private void SpawnMugger()
    {
        // Get a random point on the NavMesh
        Vector3 randomPosition = GetRandomNavMeshPosition();

        // Instantiate the Mugger prefab at the random position
        // Remember to create a new variable to store the instantiated object
        GameObject newMugger = Instantiate(muggerPrefab, randomPosition, Quaternion.identity);

        // Add the new Mugger to the list of enemies
        enemies.Add(newMugger);
    }

     private void SpawnCivvie()
    {
        // Get a random point on the NavMesh
        Vector3 randomPosition = GetRandomNavMeshPosition();

        // Instantiate the Civilian prefab at the random position
        Instantiate(civilianPrefab, randomPosition, Quaternion.identity);
    }

    private void SpawnWrestler()
    {
        // Get the amount of Wrestlers to spawn from the UpgradeManager
        int amountOfWrestlers = UpgradeManager.instance.GetUpgrade("LocalWrestler").amount;

        while (amountOfWrestlers != 0)
        {
            // Instantiate the Wrestler prefab at the random position
            GameObject newWrestler = Instantiate(wrestlerPrefab, entrancePosition, Quaternion.identity);

            // Add the new Wrestler to the list of wrestlers
            wrestlers.Add(newWrestler);            

            amountOfWrestlers--;
        }
    }

    private void GetVillainCount()
    {
        // Get the number of villains in the scene
        int villainCount = enemies.Count;

        // If the number of villains is greater than the number of villains before the wrestler spawns...
        if (villainCount > numOfVillainsBeforeWrestlerSpawn)
        {
            //Debug.Log("Enough villains to spawn a wrestler!");
            // Spawn the wrestler
            SpawnWrestler();
        }
    }
    private Vector3 GetRandomNavMeshPosition()
    {
        Vector3 randomPoint = Vector3.zero;

        // Get the bounds of the floor object
        Bounds floorBounds = mallFloor.GetComponent<Renderer>().bounds;

        // Generate a random position within the floor bounds
        Vector3 randomPosition = new Vector3(
            Random.Range(floorBounds.min.x, floorBounds.max.x),
            Random.Range(floorBounds.min.y, floorBounds.max.y),
            Random.Range(floorBounds.min.z, floorBounds.max.z)
        );

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPosition, out hit, 10f, NavMesh.AllAreas))
        {
            randomPoint = hit.position;
        }

        return randomPoint;
    }

    private void GetEntranceForSpawning()
    {
        // Get a location on the navmesh near the entrance
        NavMeshHit hit;
        if (NavMesh.SamplePosition(mallEntrance.transform.position, out hit, 10f, NavMesh.AllAreas))
        {
            entrancePosition = hit.position;
        }
    }

    // Return the list of enemies
    public List<GameObject> GetEnemies()
    {
        return enemies;
    }

    // return the list of wrestlers
    public List<GameObject> GetWrestlers()
    {
        return wrestlers;
    }

    // Remove an enemy from the list of enemies by index
    // Removing by index updates the list dynamically
    // This is important because the list is accessed frequently
    public void RemoveEnemy(int index)
    {
        enemies.RemoveAt(index);
    }

    // Remove a wrestler from the list of wrestlers by index
    public void RemoveWrestler(int index)
    {
        wrestlers.RemoveAt(index);
    }
}
