using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    // Nested class definition
    [System.Serializable]
    public class Upgrade
    {
        public string name;
        public int cost;
        public int maxUpgrade;
        public int amount;
        public bool isUnlocked = false;

        public Upgrade(string name, int cost, int maxUpgrade, int amount, bool isUnlocked)
        {
            this.name = name;
            this.cost = cost;
            this.maxUpgrade = maxUpgrade;
            this.amount = amount;
            this.isUnlocked = isUnlocked;
        }
    }
    [Header("Cash")]
    // int to store cash amount
    public static int cash = 0;
    [Header("Upgrades")]
    // Array of upgrades
    public Upgrade[] upgrades;

    void Awake()
    {
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
    public void OnUpgradeButtonClicked(string buttonName)
    {
        // Find the corresponding upgrade
        Upgrade upgrade = GetUpgrade(buttonName);

        if (upgrade.amount < upgrade.maxUpgrade && cash >= upgrade.cost)
        {
            cash -= upgrade.cost;
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
    private Upgrade GetUpgrade(string name)
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
}
