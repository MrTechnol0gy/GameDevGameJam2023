using UnityEngine;
using UnityEngine.UI;

public class PipGenerator : MonoBehaviour
{
    public UpgradeManager upgradeManager; // Reference to the UpgradeManager
    public Transform pipParent; // The parent transform where pips will be created
    public GameObject pipPrefab; // The pip Prefab

    private void Start()
    {
        // Subscribe to the Uimanager's upgrade ui started event
        UIManager.upgradesState += UpdatePips;
        // Subscribe to the UpgradeManager's upgrade Button Clicked event
        UpgradeManager.upgradeButtonClicked += UpdatePips;
    }
    void UpdatePips()
    {
        // Check that the UpgradeManager and pipPrefab are assigned
        if (upgradeManager != null && pipPrefab != null)
        {
            // Get the name of the parent object
            string parentObjectName = transform.parent.gameObject.name;

            // Find the corresponding upgrade data from the UpgradeManager using the parent object name
            UpgradeManager.Upgrade upgrade = upgradeManager.GetUpgrade(parentObjectName);

            if (upgrade != null)
            {
                // Generate pips based on the upgrade amount and maxUpgrade
                int amount = upgrade.amount;
                int maxAmount = upgrade.maxUpgrade;

                for (int i = 0; i < maxAmount; i++)
                {
                    GameObject pip = Instantiate(pipPrefab, pipParent);
                    // Customize the pip appearance or properties as needed
                    // You may need to arrange pips in a specific layout
                }
            }
            else
            {
                Debug.LogError("Upgrade not found for: " + parentObjectName);
            }
        }
        else
        {
            Debug.LogError("Please assign the UpgradeManager, pipPrefab, and pipParent in the Inspector.");
        }

    }
}
