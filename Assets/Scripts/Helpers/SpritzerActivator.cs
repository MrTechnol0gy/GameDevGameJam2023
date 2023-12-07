using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritzerActivator : MonoBehaviour
{
    // This script checks if the GrandmaSpritzer upgrade is unlocked, and reveals the GrandmaSpritzer game object if it is.
    void Start()
    {
        if (UpgradeManager.instance.GetUpgrade("GrandmaSpritzer").isUnlocked)
            gameObject.SetActive(true);
        else
            gameObject.SetActive(false);
    }
}
