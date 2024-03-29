using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{    
    // Singleton pattern
    public static UpgradeManager instance;
    // Nested class definition
    [System.Serializable]
    public class Upgrade
    {
        public string name;
        public int cost;
        public int maxUpgrade;
        public int amount;
        public bool isUnlocked = false;
        public string description;
        public string imageName; // name of the image file, since Instance ID won't maintain between sessions

        public Upgrade(string name, int cost, int maxUpgrade, int amount, bool isUnlocked, string description, string imageName)
        {
            this.name = name;
            this.cost = cost;
            this.maxUpgrade = maxUpgrade;
            this.amount = amount;
            this.isUnlocked = isUnlocked;
            this.description = description;
            this.imageName = imageName;
        }
    }
    [Header("Cash")]
    // int to store cash amount
    public int cash = 0;
    
    [Header("EnemyValues")]
    public int muggerValue = 10;
    public int cultistValue = 20;
    public int clownValue = 30;
    public int balloonValue = 1;

    [Header("Upgrades")]
    // Array of upgrades
    public Upgrade[] upgrades;
    // event handler
    public delegate void UpgradeButtonClicked();
    // event
    public static event UpgradeButtonClicked OnUpgradeButtonClickedEvent;

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

        // subscribe to the VillainClicked events
        AIMugger.muggerClicked += OnMuggerClicked;
        AICultist.cultistClicked += OnCultistClicked;
        AIBalloonClown.clownClicked += OnClownClicked;
        SphereSpawner.sphereClicked += OnSphereClicked;
    }

    // listener for the MuggerClicked event
    private void OnMuggerClicked()
    {
        // Debug.Log("Mugger Clicked");

        // add to the cash
        cash += muggerValue;
    }

    private void OnCultistClicked()
    {
        // Debug.Log("Cultist Clicked");

        // add to the cash
        cash += cultistValue;
    }

    private void OnClownClicked()
    {
        // Debug.Log("Clown Clicked");

        // add to the cash
        cash += clownValue;
    }

    private void OnSphereClicked()
    {
        // Debug.Log("Sphere Clicked");

        // add to the cash
        cash += balloonValue;
    }
    
    // listener for the upgrade button
    // buttonName is the name of the button that was clicked
    // must be manually set when you assign the button in the inspector
    public void OnUpgradeButtonClicked(string buttonName)
    {
        // Find the corresponding upgrade
        Upgrade upgrade = GetUpgrade(buttonName);        

        if (upgrade.amount < upgrade.maxUpgrade && cash >= upgrade.cost)
        {
            cash -= upgrade.cost;
            upgrade.amount++;
            upgrade.cost *= 2;
            // If the upgrade is the bigger mall, unlock the next level via the LevelManager
            if (upgrade.name == "BiggerMall")
            {
                LevelManager.instance.UnlockNextLevel();
            }
            // Debug.Log("Upgraded " + upgrade.name + " to " + upgrade.amount);
            if (upgrade.isUnlocked == false)
            {
                upgrade.isUnlocked = true;
                // Debug.Log("Unlocked " + upgrade.name);
            }
            // announce that the upgrade click was successful
            OnUpgradeButtonClickedEvent();
        }
    }    
    public Upgrade GetUpgrade(string name)
    {
        foreach (Upgrade upgrade in upgrades)
        {
            // Debug.Log("Checking upgrade " + upgrade.name);
            if (upgrade.name == name)
            {
                // Debug.Log("Found upgrade " + upgrade.name);
                // Debug.Log("Upgrade image is " + upgrade.image);
                return upgrade;
            }
        }
        Debug.Log("Upgrade " + name + " not found");
        return null;
    }

    // return the cash amount
    public int GetCash()
    {
        return cash;
    }
}
