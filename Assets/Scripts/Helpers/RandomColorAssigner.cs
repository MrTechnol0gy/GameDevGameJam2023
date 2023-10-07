using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomColorAssigner : MonoBehaviour
{
    private void Start()
    {
        // Get the MeshRenderer component attached to the game object
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

        if (meshRenderer != null)
        {
            // Generate a random color
            Color randomColor = new Color(Random.value, Random.value, Random.value);

            // Assign the random color to the material of the MeshRenderer
            meshRenderer.material.color = randomColor;
        }
    }
}

