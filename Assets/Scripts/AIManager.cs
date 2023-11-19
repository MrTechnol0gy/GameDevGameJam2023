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
    public float muggerSpawnIntervalFloor = 6f; // Interval between spawns
    public float muggerSpawnIntervalCeiling = 12f;
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
    [Header("Civilian Stats")]
    public GameObject civilianPrefab;   // Prefab to instantiate
    private int numOfCivvies;        // amount of civvies
    [Header("Other Components")]
    public GameObject mallFloor;        // Reference to the mall floor object
    private GameObject mallEntrance;    // Reference to the mall entrance object
    private Vector3 entrancePosition;   // Position of the mall entrance
    public int timeBetweenConditionalSpawnChecks = 6; // Time between conditional spawn checks
    private float timer = 0f;           // Timer for checks

    // A list of all the enemies in the scene
    private List<GameObject> enemies = new List<GameObject>();

    // A list of all civilians in the scene
    private List<GameObject> civilians = new List<GameObject>();

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
        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
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
            
            // Start the timer
            timer = Time.time;

            // Get the entrance position
            GetEntranceForSpawning();

            // Start spawning AIs
            StartSpawning();        

            // Start spawning AIs based on conditions
            InvokeRepeating(nameof(SpawnBasedOnConditions), timeBetweenConditionalSpawnChecks, timeBetweenConditionalSpawnChecks);    
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
            // Clear the list of civilians between rounds
            civilians.Clear();
            // Clear the amount of Civvies
            numOfCivvies = 0;
            // Reset the amount of guards
            amountOfGuards = UpgradeManager.instance.GetUpgrade("SecurityGuard").amount;
            // Reset the timer
            timer = 0f;
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
        InvokeRepeating(nameof(SpawnMugger), muggerSpawnIntervalFloor, Random.Range(muggerSpawnIntervalFloor, muggerSpawnIntervalCeiling));
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
        // Wait for the Cultist spawn interval floor number of seconds before beginning to spawn
        InvokeRepeating(nameof(SpawnCultistWithRandomCivilian), cultistSpawnIntervalFloor, Random.Range(cultistSpawnIntervalFloor, cultistSpawnIntervalCeiling));
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
        GameObject newCivilian = Instantiate(civilianPrefab, randomPosition, Quaternion.identity);

        // Add the new Civilian to the list of civilians
        civilians.Add(newCivilian);
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

    private void SpawnCultistWithRandomCivilian()
    {        
        // Replaces a civilian with a cultist
        // Get a random civilian
        GameObject randomCivilian = civilians[Random.Range(0, civilians.Count)];
        // Get the position of the civilian
        Vector3 civilianPosition = randomCivilian.transform.position;
        // Remove the civilian from the list of civilians
        civilians.Remove(randomCivilian);
        // Destroy the civilian
        Destroy(randomCivilian);
        // Instantiate a cultist at the civilian's position, with an x rotation of -90
        GameObject newCultist = Instantiate(cultistPrefab, civilianPosition, Quaternion.Euler(-90, 0, 0));
        // Add the new Cultist to the list of enemies
        enemies.Add(newCultist);
        // Spawn a new civilian in the scene
        SpawnCivvie();
    }

    public void ReplaceSpecificCivilianWithCultist(GameObject civilian)
    {
        // Replaces a civilian with a cultist
        // Get the position of the civilian
        Vector3 civilianPosition = civilian.transform.position;
        // Remove the civilian from the list of civilians
        civilians.Remove(civilian);
        // Destroy the civilian
        Destroy(civilian);
        // Instantiate a cultist at the civilian's position with an x rotation of -90
        GameObject newCultist = Instantiate(cultistPrefab, civilianPosition, Quaternion.identity);
        // Rotate it's x axis by 90 degrees (to correct an asset import issue)
        newCultist.transform.Rotate(-90, 0, 0);
        // Add the new Cultist to the list of enemies
        enemies.Add(newCultist);
        // Spawn a new civilian in the scene
        SpawnCivvie();
    }

    private void SpawnBalloonClown()
    {
        Debug.Log("Spawning a balloon clown!");
        // Instantiate the Clown prefab at the entrance
        GameObject newClown = Instantiate(clownPrefab, entrancePosition, Quaternion.identity);
        Debug.Log("Clown spawned at " + entrancePosition);
        Debug.Log("Rotating that clown!");
        // Rotate it's x axis by 90 degrees (to correct an asset import issue)
        newClown.transform.Rotate(-90, 0, 0);
        Debug.Log("Clown rotated!");
        Debug.Log("Adding that clown to the list of enemies!");
        // Add the new Clown to the list of enemies
        enemies.Add(newClown);
    }

    // Spawns AIs that require certain conditions to be met
    // Condition 1: The number of villains in the scene is greater than the "number of villains before the wrestler spawns" variable
    // Condition 2: There are no balloon clowns in the scene
    private void SpawnBasedOnConditions()
    {
        // Get the number of villains in the scene
        int villainCount = enemies.Count;

        // Condition 1...
        // If there are enough villains to spawn a wrestler and there are no wrestlers in the scene
        if (villainCount >= numOfVillainsBeforeWrestlerSpawn && wrestlers.Count == 0)
        {
            //Debug.Log("Enough villains to spawn a wrestler!");
            // Spawn the wrestler
            SpawnWrestler();
            // Reset the timer
            timer = 0f;
        }

        // Condition 2...
        // Spawn balloon clowns if there are no balloon clowns in the scene
        if (GetBalloonClownCount() == 0)
        {
            Debug.Log("No balloon clowns in the scene!");
            // Spawn a balloon clown
            SpawnBalloonClown();
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

    // Return the list of civilians
    public List<GameObject> GetCivilians()
    {
        return civilians;
    }

    // return the list of wrestlers
    public List<GameObject> GetWrestlers()
    {
        return wrestlers;
    }

    // Check the list of enemies for the number of Balloon Clowns
    public int GetBalloonClownCount()
    {
        int balloonClownCount = 0;

        foreach (GameObject enemy in enemies)
        {
            if (enemy.GetComponent<AIBalloonClown>() != null)
            {
                balloonClownCount++;
            }
        }

        return balloonClownCount;
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
