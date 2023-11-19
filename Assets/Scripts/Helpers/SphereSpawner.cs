using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereSpawner : MonoBehaviour
{
    public int numberOfSpheres = 5;     // 30 is a good number.
    public float sphereRadius = 0.5f;   // 0.5 is a good number.
    public float sphereScale = 1f;      // 1 is a good number.
    // List of the spheres
    private List<GameObject> spheres = new List<GameObject>();

    void Start()
    {
        SpawnSpheres();
    }

    void SpawnSpheres()
    {
        for (int i = 0; i < numberOfSpheres; i++)
        {
            Vector3 randomOffset = Random.onUnitSphere * sphereRadius;
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = transform.position + randomOffset;
            sphere.transform.localScale = Vector3.one * sphereRadius * sphereScale; // Set the scale to match the diameter

            // Set a random color
            Renderer renderer = sphere.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = Random.ColorHSV();
            }

            // Make the sphere a child of the spawner
            sphere.transform.parent = transform;

            // Add the sphere to the list
            spheres.Add(sphere);
        
        
        }
    }

    // Return the list of spheres
    public List<GameObject> GetSpheres()
    {
        return spheres;
    }

    // Pick a random sphere from the list and destroy it
    public void DestroyRandomSphere()
    {
        if (spheres.Count > 0)
        {
            int randomIndex = Random.Range(0, spheres.Count);
            GameObject sphere = spheres[randomIndex];
            spheres.RemoveAt(randomIndex);
            Destroy(sphere);
        }
    }
}
