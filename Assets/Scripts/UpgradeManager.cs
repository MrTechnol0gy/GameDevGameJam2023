using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        public Upgrade(string name, int cost, int maxUpgrade, int amount, bool isUnlocked, string description)
        {
            this.name = name;
            this.cost = cost;
            this.maxUpgrade = maxUpgrade;
            this.amount = amount;
            this.isUnlocked = isUnlocked;
            this.description = description;
        }
    }
    [Header("Cash")]
    // int to store cash amount
    public int cash = 0;
    [Header("Upgrades")]
    // Array of upgrades
    public Upgrade[] upgrades;
    // event handler
    public delegate void UpgradeButtonClicked();
    // event
    public static event UpgradeButtonClicked upgradeButtonClicked;

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

        // subscribe to the MuggerClicked event
        AIMugger.muggerClicked += OnMuggerClicked;        
    }

    // listener for the MuggerClicked event
    private void OnMuggerClicked()
    {
        Debug.Log("Mugger Clicked");
        // add 10 to the cash
        cash+=10;
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
            // announce that the upgrade click was successful
            upgradeButtonClicked();
            upgrade.amount++;
            upgrade.cost *= 2;
            Debug.Log("Upgraded " + upgrade.name + " to " + upgrade.amount);
            if (upgrade.isUnlocked == false)
            {
                upgrade.isUnlocked = true;
                Debug.Log("Unlocked " + upgrade.name);
            }
        }
    }    
    public Upgrade GetUpgrade(string name)
    {
        foreach (Upgrade upgrade in upgrades)
        {
            Debug.Log("Checking upgrade " + upgrade.name);
            if (upgrade.name == name)
            {
                Debug.Log("Found upgrade " + upgrade.name);
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
