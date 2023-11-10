using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIVillainBase : MonoBehaviour
{
    protected float TimeStartedState;
    protected NavMeshAgent agent;
    protected GameObject thisGameObject;
    protected GameObject target;
    protected GameObject escapePoint;
    protected Outline outlineScript;
    protected bool isSpotted;
    protected float spottedTimer;
    protected float durationOfSpotted = 3f;
    protected float destructDelay = 3f;
    protected bool shotBySniper = false;
    protected Rigidbody rb;

    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        target = GameObject.FindGameObjectWithTag("Player");
        escapePoint = GameObject.FindGameObjectWithTag("EscapePoint");
        thisGameObject = gameObject;
        outlineScript = GetComponent<Outline>();
    }

    protected virtual void Update()
    {
        IsSpotted();
    }

    protected virtual void IsSpotted()
    {
        if (isSpotted)
        {
            if (outlineScript == !outlineScript.enabled)
            {
                outlineScript.enabled = true;
            }
            spottedTimer += Time.deltaTime;
        }
        if (isSpotted && spottedTimer >= durationOfSpotted)
        {
            isSpotted = false;
            spottedTimer = 0f;
            outlineScript.enabled = false;
        }
    }

    protected virtual Vector3 SearchDestination(float searchRadius)
    {
        Vector3 randomPoint = Vector3.zero;

        // Get a random point on the NavMesh
        Vector3 randomDirection = Random.insideUnitSphere * searchRadius;
        
        // Try to find a valid point on the NavMesh using the generated position
        if (NavMesh.SamplePosition(transform.position + randomDirection, out NavMeshHit hit, searchRadius, NavMesh.AllAreas))
        {
            //Debug.Log("Random point found.");
            randomPoint = hit.position;
        }
        else
        {
            //Debug.Log("Random point not found.");
            randomPoint = transform.position;
        }
        
        return randomPoint;
    }
    protected virtual bool CheckForGrandma(float searchRadius)
    {
        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance <= searchRadius/2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected virtual float GetDistanceToTarget(GameObject thisGameObject, GameObject target)
    {
        float distance = Vector3.Distance(thisGameObject.transform.position, target.transform.position);
        return distance;
    }

    protected virtual void DestroyMe()
    {
        Destroy(thisGameObject, destructDelay);
    }

    public void IAmSpotted()
    {
        isSpotted = true;
        spottedTimer = 0f;
    }

    public void ShotBySniper()
    {
        shotBySniper = true; 
    }

    protected virtual bool TimeElapsedSince(float timeEventHappened, float testingTimeElapsed) => !(timeEventHappened + testingTimeElapsed > Time.time);

}
