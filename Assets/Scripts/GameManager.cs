using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public List<Vector3> shopPositions = new List<Vector3>();

    void Awake()
    {
        // check if insance already exists
        if (instance == null)
        {
            // if not, set instance to this
            instance = this;
        }
        // if instance already exists and it's not this
        else if (instance != this)
        {
            // then destroy this, enforcing singleton pattern
            Destroy(gameObject);
        }
        // set this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

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

    // Quits the application
    public void QuitGame()
    {
        Application.Quit();
    }
}
