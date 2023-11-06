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
        public int shopsAmount;
        public bool isUnlocked = false;
        public List<string> villainTypes;
        public int requiredVisits;

        public Level(string name, LevelSize size, int shopsAmount, bool isUnlocked, List<string> villainType, int requiredVisits)
        {
            this.name = name;
            this.size = size;
            this.shopsAmount = shopsAmount;
            this.isUnlocked = isUnlocked;
            this.villainTypes = villainType;
            this.requiredVisits = requiredVisits;
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
        Debug.Log("Current level name: " + name);
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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Get the current scenes name
        currentLevelName = SceneManager.GetActiveScene().name;
    }
}