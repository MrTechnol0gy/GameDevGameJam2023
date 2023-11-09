using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperActivator : MonoBehaviour
{
    // This script checks if the RooftopSniper upgrade is unlocked, and reveals the RooftopSniper game object if it is.
    void Start()
    {
        if (UpgradeManager.instance.GetUpgrade("RooftopSniper").isUnlocked)
            gameObject.SetActive(true);
        else
            gameObject.SetActive(false);
    }
}
