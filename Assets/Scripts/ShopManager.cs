using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;
    // list of all shop positions
    public List<Vector3> shopPositions = new List<Vector3>();
    // Declare a public event that will be raised when the shop positions are collected
    public event Action ShopPositionsCollected;

    private void Awake()
    {
        // Check if there is an instance of the ShopManager
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
    private void OnEnable()
    {
        // Subscribe to the SceneManager.sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Unsubscribe from the SceneManager.sceneLoaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if the loaded scene is one where we should collect shop positions
        if (scene.name == "Large Mall" || scene.name == "Medium Mall" || scene.name == "Small Mall")
        {
            // Collect all shop positions
            Debug.Log("Collecting shop positions...");
            CollectshopPositions();
        }
    }
    public void CollectshopPositions()
    {
        // Find all objects with the "ShopStop" tag
        GameObject[] shopObjects = GameObject.FindGameObjectsWithTag("ShopStop");

        // Collect the position of each shop object
        foreach (GameObject shopObject in shopObjects)
        {
            shopPositions.Add(shopObject.transform.position);
        }

        // Raise the ShopPositionsCollected event
        ShopPositionsCollected?.Invoke();
        Debug.Log("Shop positions collected by the manager. Event invoked.");

        //Debug.Log("Shop positions collected: " + shopPositions.Count);
    }

    // helper method to print out the shop positions
    void PrintshopPositions()
    {
        foreach (Vector3 position in shopPositions)
        {
            Debug.Log("Child position: " + position);
        }
    }

    // returns the list of shop positions
    public List<Vector3> GetShopPositions()
    {
        Debug.Log("Returning shop positions...");
        return shopPositions;
    }
}
