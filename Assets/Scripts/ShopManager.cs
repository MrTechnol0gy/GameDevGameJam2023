using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;
    // list of all shop positions
    public List<Vector3> shopPositions = new List<Vector3>();
    
    // Start is called before the first frame update
    void Start()
    {
       // if the current scene is the main menu, don't do anything
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "MainMenu")
        {
            return;
        }
        else
        {
            // Collect all shop positions
            CollectshopPositionsRecursive(transform);            
        } 
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
