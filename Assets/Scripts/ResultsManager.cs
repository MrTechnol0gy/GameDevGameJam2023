using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultsManager : MonoBehaviour
{
    // Singleton
    public static ResultsManager instance;
    // Variables
    private int totalShopsVisited = 0;
    private int muggersClicked = 0;
    private int cultistsClicked = 0;
    private int clownsClicked = 0;
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
        LevelLoadManager.GameStarted += StartTracking;
        // Subscribe to villain clicked events
        AIMugger.muggerClicked += MuggerClicked;
        AICultist.cultistClicked += CultistClicked;
        AIBalloonClown.clownClicked += ClownClicked;

        // Subscribe to the villain victory conditions
        AIMugger.muggerEscaped += VillainVictory;

        AIGrandma.GrandmaFinishedShoppingEvent += IncreaseWinCondition;
    }

    private void StartTracking()
    {
        // resets all tracked stats to base values
        villainVictory = false;
        muggersClicked = 0;
        cultistsClicked = 0;
        clownsClicked = 0;
    }
    private void IncreaseWinCondition()
    {
        totalShopsVisited++;
    }
    private void MuggerClicked()
    {
        // Debug.Log("Mugger clicked!");
        muggersClicked++;
    }

    private void CultistClicked()
    {
        // Debug.Log("Cultist clicked!");
        cultistsClicked++;
    }

    private void ClownClicked()
    {
        // Debug.Log("Clown clicked!");
        clownsClicked++;
    }

    private void VillainVictory()
    {
        // Debug.Log("Villain victory!");
        villainVictory = true;
    }
    public int GetMuggerAmountClicked()
    {
        return muggersClicked;
    }

    public int GetCultistAmountClicked()
    {
        return cultistsClicked;
    }

    public int GetClownAmountClicked()
    {
        return clownsClicked;
    }

    public bool GetVillainVictory()
    {
        return villainVictory;
    }
    public int GetTotalShopsVisited()
    {
        return totalShopsVisited;
    }
}
