using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GrandchildSpawner : MonoBehaviour
{
    public GameObject agentPrefab;
    public GameObject mallFloor;
    public int numberOfAgents = 25;

    void Start()
    {
        // Spawn the agents
        SpawnAgents();
    }

    void SpawnAgents()
    {
        for (int i = 0; i < numberOfAgents; i++)
        {
            // Instantiate the agent prefab
            GameObject agentObject = Instantiate(agentPrefab);

            // Set the agent's position to a random point on the NavMesh
            Vector3 randomPosition = GetRandomNavMeshPosition();
            agentObject.transform.position = randomPosition;
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
}