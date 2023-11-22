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

    public delegate void GameEndedEventHandler();
    // Declare the event using the delegate type
    public static event GameEndedEventHandler GameEnded;

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

    public void LoadMainMenu()
    {
        // Tell the UI Manager to change UIs
        UIManager.instance.ContinueFromResults();

        // Loads the main menu scene
        SceneManager.LoadScene("MainMenu");

        // Invoke the event
        GameEnded?.Invoke();
    }

    public void LoadMainMenuFromIntro()
    {
        UIManager.instance.MainMenu();
        // Loads the main menu scene
        SceneManager.LoadScene("MainMenu");
    }

    public void StartLargeMall()
    {
        // Tells the UI Manager to change UIs
        UIManager.instance.ContinueFromUpgrades();

        // Loads the gameplay scene
        SceneManager.LoadScene("Large Mall");
        
        // Save the game
        SaveLoadManager.instance.SaveGame();

        // Invoke the event
        GameStarted?.Invoke();
    }

    public void StartMediumMall()
    {
        // Tells the UI Manager to change UIs
        UIManager.instance.ContinueFromUpgrades();

        // Loads the gameplay scene
        SceneManager.LoadScene("Medium Mall");
        
        // Save the game
        SaveLoadManager.instance.SaveGame();

        // Invoke the event
        GameStarted?.Invoke();
    }

    public void StartConvenienceStore()
    {
        // Tells the UI Manager to change UIs
        UIManager.instance.ContinueFromUpgrades();
        
        // Loads the gameplay scene
        SceneManager.LoadScene("Convenience Store");
        
        // Save the game
        SaveLoadManager.instance.SaveGame();

        // Invoke the event
        GameStarted?.Invoke();
    }

    public void LoadVictoryLevel()
    {
        // Loads the victory scene
        SceneManager.LoadScene("Victory");

        // Invoke the event
        GameEnded?.Invoke();
    }
}
