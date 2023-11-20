using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class AIGrandchild : MonoBehaviour
{
    private NavMeshAgent agent;
    private GameObject mallFloor;
    private float timer = 0f;
    public float timeToFindNewDestination = 3f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        mallFloor = GameObject.Find("VictoryFloor");
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeToFindNewDestination)
        {
            timer = 0f;
            // Find a random point on the NavMesh
            Vector3 randomDestination = GetRandomNavMeshPosition();

            // Set the agent's destination to the random point
            agent.SetDestination(randomDestination);
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
