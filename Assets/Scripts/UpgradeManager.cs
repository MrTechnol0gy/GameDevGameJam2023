using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [Header("Cash")]
    // int to store cash amount
    public static int cash = 0;
    [Header("Upgrades")]
    // string to store the name of the upgrade
    public string upgradeOneName = "GrandmaSpritzer";
    // int to store the cost of the upgrade
    public int upgradeOneCost = 100;
    // int to store the max amount of upgrades
    public int upgradeOneMaxUpgrade = 10;
    // int to store the amount of upgrades
    public int upgradeOneAmount = 0;
    // string to store the name of the upgrade
    public string upgradeTwoName = "SecurityGuard";
    // int to store the cost of the upgrade
    public int upgradeTwoCost = 100;
    // int to store the max amount of upgrades
    public int upgradeTwoMaxUpgrade = 10;
    // int to store the amount of upgrades
    public int upgradeTwoAmount = 0;
    // string to store the name of the upgrade
    public string upgradeThreeName = "Wrestler";
    // int to store the cost of the upgrade
    public int upgradeThreeCost = 100;
    // int to store the max amount of upgrades
    public int upgradeThreeMaxUpgrade = 10;
    // int to store the amount of upgrades
    public int upgradeThreeAmount = 0;

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
    public void OnUpgradeButtonClicked(string buttonName)
    {
        // switch case for each button
        switch (buttonName)
        {
            // if the upgrade is GrandmaSpritzer
            case "GrandmaSpritzer":
                // call the Upgrade function
                Upgrade("GrandmaSpritzer");
                break;
            // if the upgrade is GrandpaSpritzer
            case "SecurityGuard":
                // call the Upgrade function
                Upgrade("SecurityGuard");
                break;
            // if the upgrade is AuntSpritzer
            case "Wrestler":
                // call the Upgrade function
                Upgrade("Wrestler");
                break;
        }
    }

    private void Upgrade(string name)
    {
        // get the states for the passed through upgrade
        int cost = GetUpgradeCost(name);
        int maxUpgrade = GetUpgradeMax(name);
        int amount = GetUpgradeAmount(name);

        // if the amount is less than the max amount
        if (amount < maxUpgrade)
        {
            // if the cash is greater than or equal to the cost
            if (cash >= cost)
            {
                // subtract the cost from the cash
                cash -= cost;
                // add one to the amount
                amount++;
                // set the amount
                SetUpgradeAmount(name, amount);
            }
        }
    }

    private int GetUpgradeCost(string name)
    {
        // switch case for each upgrade
        switch (name)
        {
            // if the upgrade is GrandmaSpritzer
            case "GrandmaSpritzer":
                // return the cost of the upgrade
                return upgradeOneCost;
            // if the upgrade is GrandpaSpritzer
            case "SecurityGuard":
                // return the cost of the upgrade
                return upgradeTwoCost;
            // if the upgrade is AuntSpritzer
            case "Wrestler":
                // return the cost of the upgrade
                return upgradeThreeCost;
            default:
                // return 0
                return 0;
        }
    }

    private int GetUpgradeMax(string name)
    {
        // switch case for each upgrade
        switch (name)
        {
            // if the upgrade is GrandmaSpritzer
            case "GrandmaSpritzer":
                // return the max amount of the upgrade
                return upgradeOneMaxUpgrade;
            // if the upgrade is GrandpaSpritzer
            case "SecurityGuard":
                // return the max amount of the upgrade
                return upgradeTwoMaxUpgrade;
            // if the upgrade is AuntSpritzer
            case "Wrestler":
                // return the max amount of the upgrade
                return upgradeThreeMaxUpgrade;
            default:
                // return 0
                return 0;
        }
    }

    private int GetUpgradeAmount(string name)
    {
        // switch case for each upgrade
        switch (name)
        {
            // if the upgrade is GrandmaSpritzer
            case "GrandmaSpritzer":
                // return the amount of the upgrade
                return upgradeOneAmount;
            // if the upgrade is GrandpaSpritzer
            case "SecurityGuard":
                // return the amount of the upgrade
                return upgradeTwoAmount;
            // if the upgrade is AuntSpritzer
            case "Wrestler":
                // return the amount of the upgrade
                return upgradeThreeAmount;
            default:
                // return 0
                return 0;
        }
    }

    private void SetUpgradeAmount(string name, int amount)
    {
        // switch case for each upgrade
        switch (name)
        {
            // if the upgrade is GrandmaSpritzer
            case "GrandmaSpritzer":
                // set the amount of the upgrade
                upgradeOneAmount = amount;
                break;
            // if the upgrade is GrandpaSpritzer
            case "SecurityGuard":
                // set the amount of the upgrade
                upgradeTwoAmount = amount;
                break;
            // if the upgrade is AuntSpritzer
            case "Wrestler":
                // set the amount of the upgrade
                upgradeThreeAmount = amount;
                break;
            default:
                break;
        }
    }
}
