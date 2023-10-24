using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultsManager : MonoBehaviour
{
    // Singleton
    public static ResultsManager instance;

    private int muggersClicked = 0;
    private bool villainVictory = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            // Debug.Log("ResultsManager instance already exists, destroying object!");
            Destroy(this);
        }
        DontDestroyOnLoad(this);
    }
    
    void Start()
    {
        // Subscribe to the scene change event
        GameManager.LevelLoaded += StartTracking;
        // Subscribe to villain clicked events
        AIMugger.muggerClicked += MuggerClicked;
        // Other villain clicked events go here

        // Subscribe to the villain victory conditions
        AIMugger.muggerEscaped += VillainVictory;
    }

    private void StartTracking()
    {
        // resets all tracked stats to base values
        villainVictory = false;
        muggersClicked = 0;
    }
    private void MuggerClicked()
    {
        // Debug.Log("Mugger clicked!");
        muggersClicked++;
    }
    private void VillainVictory()
    {
        // Debug.Log("Villain victory!");
        villainVictory = true;
    }
    public int GetScores()
    {
        return muggersClicked;
    }
}
