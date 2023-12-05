using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Singleton pattern
    public static LevelManager instance;

    public enum LevelSize { Small, Medium, Large };
    private string currentLevelName;

    // Nested class definition
    [System.Serializable]
    public class Level
    {
        public string name;
        public LevelSize size;
        public int shopsAmount;     // What is this for?
        public bool isUnlocked = false; // Whether the level has been unlocked.
        public List<string> villainTypes;   // The kind of villains that can appear in this level.
        public int requiredVisits;  // How many shops need to be visited before the level is complete.
        public int numOfCivvies;    // How many civilians are in the level.
        public Level(string name, LevelSize size, int shopsAmount, bool isUnlocked, List<string> villainType, int requiredVisits, int numOfCivvies)
        {
            this.name = name;
            this.size = size;
            this.shopsAmount = shopsAmount;
            this.isUnlocked = isUnlocked;
            this.villainTypes = villainType;
            this.requiredVisits = requiredVisits;
            this.numOfCivvies = numOfCivvies;

        }
    }
    // Array of levels
    public Level[] levels;

    void Awake()
    {
        // Check if there is an instance of the UpgradeManager
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
        DontDestroyOnLoad(gameObject);

        // Listen for the scene changing
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public Level GetLevel()
    {
        string name = currentLevelName;
        // Debug.Log("Current level name: " + name);
        // Loop through the upgrades array
        foreach (Level level in levels)
        {
            // If the name of the upgrade matches the name passed in
            if (level.name == name)
            {
                // Return the upgrade
                return level;
            }
        }
        // If no upgrade is found, return null
        Debug.Log("Level " + name + "not found");
        return null;
    }

    // Returns the amount of cameras needed for the current level
    // TODO: This should be moved to the MainCamera script
    public int DetermineCameraAmount()
    {
        Level level = GetLevel();
        switch (level.size)
        {
            case LevelSize.Small:
                return 2;
            case LevelSize.Medium:
                return 2;
            case LevelSize.Large:
                return 6;
            default:
                return 1;
        }
    }

    public void UnlockNextLevel()
    {
        // Loop through the levels array
        foreach (Level level in levels)
        {
            // If the level is not unlocked
            if (!level.isUnlocked)
            {
                // Unlock the level
                level.isUnlocked = true;
                // Break out of the loop
                break;
            }
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Get the current scenes name
        currentLevelName = SceneManager.GetActiveScene().name;
    }
}
