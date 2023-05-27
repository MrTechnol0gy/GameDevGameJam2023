using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager get;
    public List<Vector3> shopPositions = new List<Vector3>();

    void Awake()
    {
        get = this;
    }

    void Start()
    {
        CollectshopPositionsRecursive(transform);
    }

    void CollectshopPositionsRecursive(Transform parent)
    {
        int childCount = parent.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = parent.GetChild(i);
            shopPositions.Add(child.position);

            // Recursively collect positions of child's children
            CollectshopPositionsRecursive(child);
        }
    }

    void PrintshopPositions()
    {
        foreach (Vector3 position in shopPositions)
        {
            Debug.Log("Child position: " + position);
        }
    }

    public List<Vector3> GetShopPositions()
    {
        return shopPositions;
    }
}
