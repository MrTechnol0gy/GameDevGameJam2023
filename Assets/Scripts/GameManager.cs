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

    public void StartGame()
    {
        // Loads the gameplay scene
        SceneManager.LoadScene("Gameplay");
        
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
