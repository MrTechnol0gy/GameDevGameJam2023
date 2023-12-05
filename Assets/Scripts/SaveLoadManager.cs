using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    // Singleton pattern
    public static SaveLoadManager instance;

    [Header("References")]
    public UpgradeManager upgradeManager;

    [System.Serializable]
    public class SaveData
    {
        public int cash;
        public int progress;
        public List<UpgradeManager.Upgrade> upgrades;
        public List<LevelManager.Level> levels;
    }


    private string savePath;
    private string defaultSavePath = Application.dataPath + "/DefaultSaveData/defaultSave.json";

    private void Awake()
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

        // Subscribe to events
        UIManager.OnSaveGameEvent += SaveGame;
        UIManager.OnLoadGameEvent += LoadGame;
    }
    private void Start()
    {
        savePath = Application.persistentDataPath + "/save.json";        
        Debug.Log(savePath);
        // Load the game
        LoadGame();
    }

    public void SaveGame()
    {
        SaveData data = new SaveData
        {
            cash = UpgradeManager.instance.GetCash(),
            progress = ResultsManager.instance.GetProgress(),
            upgrades = new List<UpgradeManager.Upgrade>(UpgradeManager.instance.upgrades),
            levels = new List<LevelManager.Level>(LevelManager.instance.levels)
        };

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(savePath, json);
    }

    public void LoadGame()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            UpgradeManager.instance.cash = data.cash;
            ResultsManager.instance.SetProgress(data.progress);
            UpgradeManager.instance.upgrades = data.upgrades.ToArray();
            LevelManager.instance.levels = data.levels.ToArray();
        }
        else
        {
            Debug.LogWarning("No save data found.");
            RestoreDefaults();
        }
    }

    public void DeleteSave()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            PlayerPrefs.DeleteAll();
            Debug.Log("Save file deleted.");
            RestoreDefaults();
        }
        else
        {
            PlayerPrefs.DeleteAll();
            RestoreDefaults();
            Debug.Log("No save file found to delete.");
        }
    }

    private void RestoreDefaults()
    {
        if (File.Exists(defaultSavePath))
        {
            string json = File.ReadAllText(defaultSavePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            UpgradeManager.instance.cash = data.cash;
            ResultsManager.instance.SetProgress(data.progress);
            UpgradeManager.instance.upgrades = data.upgrades.ToArray();
            LevelManager.instance.levels = data.levels.ToArray();
        }
        else
        {
            Debug.LogWarning("No default save data found.");
        }
    }
}