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
        public List<UpgradeManager.Upgrade> upgrades;
    }

    private string savePath;

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
    }
    private void Start()
    {
        savePath = Application.persistentDataPath + "/save.json";
        // Load the game
        LoadGame();
    }

    public void SaveGame()
    {
        SaveData data = new SaveData
        {
            cash = upgradeManager.GetCash(),
            upgrades = new List<UpgradeManager.Upgrade>(upgradeManager.upgrades)
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

            upgradeManager.cash = data.cash;
            upgradeManager.upgrades = data.upgrades.ToArray();
        }
        else
        {
            Debug.LogWarning("No save data found.");
        }
    }
}