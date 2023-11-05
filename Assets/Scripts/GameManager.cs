using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    // Declare the delegate type for the event
    public delegate void GameStartedEventHandler();
    // Declare the event using the delegate type
    public static event GameStartedEventHandler GameStarted;
    public delegate void LevelLoadedEventHandler();
    public static event LevelLoadedEventHandler LevelLoaded;
    
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
    private void Start()
    {
        // Load the game data
        SaveLoadManager.instance.LoadGame();
        Debug.Log("Game loaded");
    }
    public void StartGame()
    {
        Debug.Log("Starting the game");
        // Loads the gameplay scene
        SceneManager.LoadScene("Large Mall");
        
        // Save the game
        SaveLoadManager.instance.SaveGame();
        Debug.Log("Game saved");
        
        // Invoke the event
        if (GameStarted != null)
        {
            GameStarted();
        }
    }

    public void MainMenu()
    {
        // Loads the main menu scene
        SceneManager.LoadScene("MainMenu");
        // Invoke the event
        if (LevelLoaded != null)
        {
            LevelLoaded();
        }
    }
    
    // Quits the application
    public void QuitGame()
    {
        Application.Quit();
    }
}
