using System.Collections;
using UnityEngine;

public class CubeGenerator : MonoBehaviour
{
    public GameObject cubePrefab;
    public int numberOfCubes = 200;

    void Start()
    {
        StartCoroutine(GenerateCubes());
    }

    // Generates a few cubes every second
    IEnumerator GenerateCubes()
    {
        for (int i = 0; i < numberOfCubes; i++)
        {
            // Instantiate a cube
            GameObject cube = Instantiate(cubePrefab);

            // Set the cube's position to a random position
            cube.transform.position = GetRandomPosition();

            // Wait for 0.1 seconds
            yield return new WaitForSeconds(0.1f);
        }
    }

    // Get a random position around the position of the gameobject this script is attached to
    Vector3 GetRandomPosition()
    {
        Vector3 position = transform.position;
        float x = Random.Range(position.x - 5, position.x + 5);
        float y = Random.Range(position.y - 5, position.y + 5);
        float z = Random.Range(position.z - 5, position.z + 5);
        return new Vector3(x, y, z);
    }
}
