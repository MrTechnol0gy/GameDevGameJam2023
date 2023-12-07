using UnityEngine;
using UnityEngine.UI;

public class PipGenerator : MonoBehaviour
{
    public UpgradeManager upgradeManager; // Reference to the UpgradeManager
    public Transform pipParent; // The parent transform where pips will be created
    public GameObject pipPrefab; // The pip Prefab

    private void Start()
    {
        UpdatePips();
        // Subscribe to the UpgradeManager's upgrade Button Clicked event
        UpgradeManager.OnUpgradeButtonClickedEvent += UpdatePips;
       
    }
    void UpdatePips()
    {
        Debug.Log("UpdatePips() called.");
        // Clear the pips
        foreach (Transform child in pipParent)
        {
            Destroy(child.gameObject);
        }

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
                int maxUpgrade = upgrade.maxUpgrade;
                int pipsToGenerate = amount > maxUpgrade ? maxUpgrade : amount;

                // Generate the pips that have been upgraded
                for (int i = 0; i < pipsToGenerate; i++)
                {
                    GameObject pip = Instantiate(pipPrefab, pipParent);
                    pip.GetComponent<Image>().color = Color.green;
                }
                // Generate the remaining pips
                for (int i = 0; i < maxUpgrade - amount; i++)
                {
                    GameObject pip = Instantiate(pipPrefab, pipParent);
                    pip.GetComponent<Image>().color = Color.red;
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
