using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    [Header("Mugger Stats")]
    public GameObject muggerPrefab; // Prefab to instantiate
    public float spawnIntervalFloor = 6f; // Interval between spawns
    public float spawnIntervalCeiling = 12f;
    [Header("Civilian Stats")]
    public GameObject civilianPrefab;   // Prefab to instantiate
    public int amountOfCivvies = 30;    // amount of civvies
    [Header("Other Components")]
    public GameObject mallFloor;        // Reference to the mall floor object
    private void Awake()
    {
        // Check if there is an instance of the EnemyManager
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

            // Start spawning enemies
            StartSpawning();            
        }
        else
        {
            // Stop spawning enemies
            CancelInvoke();
            // Reset the amount of civvies
            amountOfCivvies = 30;
        }
    }
    private void StartSpawning()
    {
        InvokeRepeating(nameof(SpawnMugger), 0f, Random.Range(spawnIntervalFloor, spawnIntervalCeiling));
        while (amountOfCivvies != 0)
        {
            SpawnCivvie();
            amountOfCivvies--;
        }
    }

    private void SpawnMugger()
    {
        // Get a random point on the NavMesh
        Vector3 randomPosition = GetRandomNavMeshPosition();

        // Instantiate the Mugger prefab at the random position
        Instantiate(muggerPrefab, randomPosition, Quaternion.identity);
    }

     private void SpawnCivvie()
    {
        // Get a random point on the NavMesh
        Vector3 randomPosition = GetRandomNavMeshPosition();

        // Instantiate the Mugger prefab at the random position
        Instantiate(civilianPrefab, randomPosition, Quaternion.identity);
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
}
