using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    // Game Victory Conditions
    [Header("Game Victory Conditions")]
    public int totalShopsToVisitForVictory = 100;
    
    private void Awake()
    {
        // Check if there is an instance of the GameManager
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
    
    // Quits the application
    public void QuitGame()
    {
        Application.Quit();
    }
}
