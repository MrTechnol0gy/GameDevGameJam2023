using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPositionsListener : MonoBehaviour
{
    private void OnEnable()
    {
        // Subscribe to the ShopPositionsCollected event
        Singleton.instance.GetComponentInChildren<ShopManager>().ShopPositionsCollected += OnShopPositionsCollected;
    }

    private void OnDisable()
    {
        // Unsubscribe from the ShopPositionsCollected event
        Singleton.instance.GetComponentInChildren<ShopManager>().ShopPositionsCollected -= OnShopPositionsCollected;
    }

    private void OnShopPositionsCollected()
    {
        // Log a message to the console when the event is raised
        Debug.Log("Shop positions collected!");
    }
}
