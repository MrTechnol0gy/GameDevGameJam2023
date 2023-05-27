using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    [Header("Mugger Stats")]
    public GameObject muggerPrefab; // Prefab to instantiate
    public float spawnIntervalFloor = 6f; // Interval between spawns
    public float spawnIntervalCeiling = 12f;
    [Header("Civilian Stats")]
    public GameObject civilianPrefab;   // Prefab to instantiate
    public int amountOfCivvies = 30;    // amount of civvies
    [Header("Other Components")]
    public GameObject mallFloor;

    private void Start()
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
     private void SpawnCivvie()
    {
        // Get a random point on the NavMesh
        Vector3 randomPosition = GetRandomNavMeshPosition();

        // Instantiate the Mugger prefab at the random position
        Instantiate(civilianPrefab, randomPosition, Quaternion.identity);
    }
}
