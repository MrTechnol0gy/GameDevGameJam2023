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
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Tell the UI Manager to change UIs
        UIManager.instance.ContinueFromResults();

        // Loads the main menu scene
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadMainMenuFromVictory()
    {
        UIManager.instance.MainMenu();
        // Loads the main menu scene
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadMainMenuFromIntro()
    {
        UIManager.instance.MainMenu();
        // Loads the main menu scene
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadLevel()
    {        
        string selectedLevel = UIManager.instance.GetSelectedLevel();
        // Tells the UI Manager to change UIs
        UIManager.instance.ContinueFromLevelSelect();
        // Loads the gameplay scene
        SceneManager.LoadScene(selectedLevel);
        // Save the game
        SaveLoadManager.instance.SaveGame();
        // Invoke the event
        GameStarted?.Invoke();
    }

    public void LoadVictoryLevel()
    {
        // Reset player progress
        ResultsManager.instance.SetProgress(0);

        // Loads the victory scene
        SceneManager.LoadScene("Victory");

        // Invoke the event
        GameEnded?.Invoke();

    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Unsubscribe from the event to prevent multiple calls
        SceneManager.sceneLoaded -= OnSceneLoaded;
        
        // Invoke the event
        GameEnded?.Invoke();
    }
}
