using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewUIManager : MonoBehaviour
{   
    public static NewUIManager instance;
    // reference to the Main Menu UI
    public GameObject mainMenuUI;
    // reference to the Pause Menu UI
    public GameObject pauseMenuUI;
    // reference to the Options Menu UI
    public GameObject optionsMenuUI;
    // reference to the Gameplay UI
    public GameObject gameplayUI;
    // reference to the results screen UI
    public GameObject resultsScreenUI;
    // reference to the credits UI
    public GameObject creditsUI;
    // reference to the level select UI
    public GameObject levelSelectUI;
    // reference to the upgrades UI
    public GameObject upgradesUI;
    // reference to the glossary UI
    public GameObject glossaryUI;
    // float for the time the state started
    private float TimeStartedState;
    // reference to the previous state
    private States previousState;
    // enum for the states
    public enum States
    {
        mainmenu = 0,
        pausemenu = 1,
        options = 2,
        gameplay = 3,
        results = 4,
        glossary = 5,
        credits = 6,
        upgrades = 7,
        levelselect = 8
    }
    private States _currentState = States.mainmenu;       //sets the starting state    
    public States currentState 
    {
        get => _currentState;
        set {
            if (_currentState != value) 
            {
                // Calling ended state for the previous state registered.
                OnEndedState(_currentState);
                
                // Setting the new current state based on active UI
                _currentState = value;
                
                // Registering here the time we're starting the state
                TimeStartedState = Time.time;
                
                // Call the started state method for the new state.
                OnStartedState(_currentState);
            }
        }
    }
    // OnStartedState is for things that should happen when a state first begins
    public void OnStartedState(States state) 
    {
        switch (state) 
        {
            case States.mainmenu:
                //Debug.Log("I am the main menu."); 
                mainMenuUI.SetActive(true);   
                break;
            case States.pausemenu:
                //Debug.Log("I am paused.");   
                pauseMenuUI.SetActive(true);  
                // stop time
                Time.timeScale = 0f;
                break;
            case States.options:
                //Debug.Log("I am options.");
                optionsMenuUI.SetActive(true);  
                // stop time
                Time.timeScale = 0f;  
                break;
            case States.gameplay:
                //Debug.Log("I am gameplay.");
                gameplayUI.SetActive(true);       
                break;
            case States.results:
                //Debug.Log("I am winscreen."); 
                resultsScreenUI.SetActive(true); 
                // stop time
                Time.timeScale = 0f;     
                break;
            case States.glossary:
                //Debug.Log("I am losescreen.");   
                glossaryUI.SetActive(true);  
                break;
            case States.credits:
                //Debug.Log("I am credits.");   
                creditsUI.SetActive(true);  
                break;
            case States.upgrades:
                //Debug.Log("I am upgrades.");   
                upgradesUI.SetActive(true);  
                break;
            case States.levelselect:
                //Debug.Log("I am level select.");   
                levelSelectUI.SetActive(true);  
                break;
        }
    }

    // OnEndedState is for things that should end or change when a state ends; for cleanup
    public void OnEndedState(States state) 
    {
        switch (state) 
        {
            case States.mainmenu:
                Debug.Log("I am leaving the main menu.");
                mainMenuUI.SetActive(false);
                // Sets the previous state variable to this state
                previousState = States.mainmenu;
                break;
            case States.pausemenu:
                //Debug.Log("I am paused."); 
                pauseMenuUI.SetActive(false);  
                // Sets the previous state variable to this state
                previousState = States.pausemenu;  
                // resume time
                Time.timeScale = 1f;          
                break;
            case States.options:
                //Debug.Log("I am options."); 
                optionsMenuUI.SetActive(false);  
                // Sets the previous state variable to this state
                previousState = States.options; 
                // resume time
                Time.timeScale = 1f;           
                break;
            case States.gameplay:
                //Debug.Log("I am gameplay.");
                gameplayUI.SetActive(false);    
                // Sets the previous state variable to this state
                previousState = States.gameplay;          
                break;
            case States.results:
                //Debug.Log("I am winscreen.");
                resultsScreenUI.SetActive(false); 
                // Sets the previous state variable to this state
                previousState = States.results; 
                // resume time
                Time.timeScale = 1f;              
                break;
            case States.glossary:
                //Debug.Log("I am losescreen.");
                glossaryUI.SetActive(false); 
                // Sets the previous state variable to this state
                previousState = States.glossary;             
                break;
            case States.credits:
                //Debug.Log("I am credits.");
                creditsUI.SetActive(false); 
                // Sets the previous state variable to this state
                previousState = States.credits;             
                break;
            case States.upgrades:
                //Debug.Log("I am upgrades.");
                upgradesUI.SetActive(false); 
                // Sets the previous state variable to this state
                previousState = States.upgrades;             
                break;
            case States.levelselect:
                //Debug.Log("I am level select.");
                levelSelectUI.SetActive(false); 
                // Sets the previous state variable to this state
                previousState = States.levelselect;             
                break;
        }
    }

    void Awake()
    {
        // Sets all UI to false
        SetAllUIToFalse();
        GameManager.GameStarted += GameplayUI;
    }
    void Start()
    {
        // Sets the current state to the default state
        OnStartedState(currentState);
    }

    void Update()
    {
        
    }

    // Sets all Ui elements to inactive
    public void SetAllUIToFalse()
    {
        mainMenuUI.SetActive(false);
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(false);
        gameplayUI.SetActive(false);
        resultsScreenUI.SetActive(false);
        creditsUI.SetActive(false);
        upgradesUI.SetActive(false);
        levelSelectUI.SetActive(false);
        glossaryUI.SetActive(false);
    }

    // This method activates the main menu UI
    public void MainMenu()
    {
        //Debug.Log("Main Menu clicked");
        currentState = States.mainmenu;
    }

    // This method activates the pause menu UI
    public void PauseMenu()
    {
        //Debug.Log("Pause Menu clicked");
        currentState = States.pausemenu;
    }

    // This method activates the options menu UI
    public void OptionsMenu()
    {
        //Debug.Log("Options Menu clicked");
        currentState = States.options;
    }

    // This method activates the gameplay UI
    public void GameplayUI()
    {
        //Debug.Log("Gameplay UI clicked");
        currentState = States.gameplay;
    }

    // This method activates the win screen UI
    public void Results()
    {
        currentState = States.results;
    }

    // This method activates the lose screen UI
    public void Glossary()
    {
        currentState = States.glossary;
    }

    // This method activates the credits UI
    public void Credits()
    {
        currentState = States.credits;
    }

    // This method returns to the state prior to the current state
    // Functions as a back/return button for the UI
    public void Return()
    {
        if (previousState == States.mainmenu)
        {
            currentState = States.mainmenu;
        }
        else if (previousState == States.gameplay)
        {
            currentState = States.gameplay;
        }
        else if (previousState == States.pausemenu)
        {
            currentState = States.pausemenu;
        }
        else if (previousState == States.options)
        {
            currentState = States.options;
        }
        else if (previousState == States.results)
        {
            currentState = States.results;
        }
        else if (previousState == States.glossary)
        {
            currentState = States.glossary;
        }
        else if (previousState == States.credits)
        {
            currentState = States.credits;
        }
        else if (previousState == States.upgrades)
        {
            currentState = States.upgrades;
        }
        else if (previousState == States.levelselect)
        {
            currentState = States.levelselect;
        }
    }

    // Returns the current state
    public States GetCurrentState() => currentState;

    // This method can be used to test if a certain time has elapsed since we registered an event time. 
    public bool TimeElapsedSince(float timeEventHappened, float testingTimeElapsed) => !(timeEventHappened + testingTimeElapsed > Time.time);
}