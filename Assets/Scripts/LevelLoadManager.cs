using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoadManager : MonoBehaviour
{
    // Singleton
    public static LevelLoadManager instance;

    public delegate void GameStartedEventHandler();
    // Declare the event using the delegate type
    public static event GameStartedEventHandler GameStarted;

    void Awake()
    {
        // Check if there is an instance of the LevelLoadManager
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

    public void StartLargeMall()
    {
        // Tells the UI Manager to change UIs
        UIManager.instance.ContinueFromUpgrades();

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

    public void StartMediumMall()
    {
        // Tells the UI Manager to change UIs
        UIManager.instance.ContinueFromUpgrades();

        // Loads the gameplay scene
        SceneManager.LoadScene("Medium Mall");
        
        // Save the game
        SaveLoadManager.instance.SaveGame();
        Debug.Log("Game saved");
        
        // Invoke the event
        if (GameStarted != null)
        {
            GameStarted();
        }
    }

    public void StartConvenienceStore()
    {
        // Tells the UI Manager to change UIs
        UIManager.instance.ContinueFromUpgrades();
        
        // Loads the gameplay scene
        SceneManager.LoadScene("Convenience Store");
        
        // Save the game
        SaveLoadManager.instance.SaveGame();
        Debug.Log("Game saved");
        
        // Invoke the event
        if (GameStarted != null)
        {
            GameStarted();
        }
    }
}
