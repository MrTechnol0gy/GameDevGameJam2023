using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{   
    public static UIManager instance;

    [Header("UI Elements")]
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
    // reference to the upgrades UI
    public GameObject upgradesUI;
    // reference to the victory UI
    public GameObject victoryUI;
    // reference to the level select UI
    public GameObject levelSelectUI;
    // Header
    [Header("Upgrade Screen UI Elements")]
    public TextMeshProUGUI cashText;
    public TextMeshProUGUI progressText;
    public TextMeshProUGUI informationText;
    public TextMeshProUGUI grandmaSpritzerText;
    public TextMeshProUGUI securityGuardText;
    public TextMeshProUGUI rooftopSniperText;
    public TextMeshProUGUI rocketPoweredScooterText;
    public TextMeshProUGUI localWrestlerText;
    public Image upgradeImage;


    [Header("Gameplay UI Elements")]
    public TextMeshProUGUI cashTextGameplay;
    [Header("Results Screen UI Elements")]
    public TextMeshProUGUI muggersClickedText;
    public TextMeshProUGUI cultistsClickedText;
    public TextMeshProUGUI clownsClickedText;
    public TextMeshProUGUI finalResultsText;
    public TextMeshProUGUI progressTrackerText;
    public GameObject victory;
    public GameObject defeat;
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
        upgrades = 5,
        victorious = 6,
        intro = 7, 
        levelselect = 8
    }
    private States _currentState = States.intro;       //sets the starting state    
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
                // Tell the audio manager to start playing the BGM
                AudioManager.instance.PlayBackgroundMusic();
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
                // Stop the BGM
                AudioManager.instance.StopBackgroundMusic();
                //Debug.Log("I am gameplay.");
                gameplayUI.SetActive(true);
                // Update gameplay UI elements
                UpdateGameplayUI();
                break;
            case States.results:
                //Debug.Log("I am results."); 
                resultsScreenUI.SetActive(true); 
                // stop time
                Time.timeScale = 0f;
                // update the UI elements
                UpdateResultsScreenUI();
                // Check if the player is victorious
                ResultsManager.instance.VictoryCheck();
                break;
            case States.upgrades:
                //Debug.Log("I am upgrades.");   
                upgradesUI.SetActive(true);  
                // update the UI elements
                UpdateUpgradeScreenUI();
                // listen for the hover event
                HoverHandler.OnHoverEnter += UpgradeHover;
                break;
            case States.victorious:
                victoryUI.SetActive(true);
                //Debug.Log("I am victorious.");
                break;
            case States.intro:
                //Debug.Log("I am the intro.");
                break;
            case States.levelselect:
                //Debug.Log("I am the how to play screen.");
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
                //Debug.Log("I am the main menu."); 
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
                // Save the game
                SaveLoadManager.instance.SaveGame();
                break;
            case States.upgrades:
                //Debug.Log("I am upgrades.");
                upgradesUI.SetActive(false); 
                // Sets the previous state variable to this state
                previousState = States.upgrades;    
                // Stop listening for the event
                HoverHandler.OnHoverEnter -= UpgradeHover;         
                break;
            case States.victorious:
                victoryUI.SetActive(false);
                //Debug.Log("I am victorious.");
                break;
            case States.intro:
                //Debug.Log("I am the intro.");
                break;
            case States.levelselect:
                //Debug.Log("I am the how to play screen.");
                levelSelectUI.SetActive(false);
                break;
        }
    }

    void Awake()
    {
        // Check if there is an instance of the UIManager
        if (instance == null)
        {
            // If not, set the instance to this
            instance = this;
        }
        else
        {
            // If there is, destroy this object
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        // Sets all UI to false
        SetAllUIToFalse();
        LevelLoadManager.GameStarted += GameplayUI;
        UpgradeManager.OnUpgradeButtonClickedEvent += UpdateUpgradeScreenUI;
        AIMugger.muggerClicked += UpdateGameplayUI;
        AICultist.cultistClicked += UpdateGameplayUI;
        AIBalloonClown.clownClicked += UpdateGameplayUI;
        SphereSpawner.sphereClicked += UpdateGameplayUI;
    }
    void Start()
    {
        // Sets the current state to the default state
        OnStartedState(currentState);
    }

    // Sets all Ui elements to inactive
    public void SetAllUIToFalse()
    {
        mainMenuUI.SetActive(false);
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(false);
        gameplayUI.SetActive(false);
        resultsScreenUI.SetActive(false);
        upgradesUI.SetActive(false);
        victoryUI.SetActive(false);
        levelSelectUI.SetActive(false);
    }

    void Update()
    {
        // If the player presses escape, open the pause menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // If the current state is gameplay, open the pause menu
            if (currentState == States.gameplay)
            {
                PauseMenu();
            }
            // If the current state is pause menu, return to gameplay
            else if (currentState == States.pausemenu)
            {
                GameplayUI();
            }
        }
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

    // This method activates the results screen UI
    public void Results()
    {
        currentState = States.results;
    }

    // This method activates the upgrades UI
    public void Upgrades()
    {
        currentState = States.upgrades;
    }

    // This method activates the how to play UI
    public void LevelSelectUI()
    {
        currentState = States.levelselect;
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
            if (SceneManager.GetActiveScene().name == "MainMenu")
            {
                currentState = States.mainmenu;
            }
            else
            {
                currentState = States.gameplay;
            }
        }
        else if (previousState == States.results)
        {
            currentState = States.results;
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

    // Activates the Main Menu UI when leaving the results screen
    public void ContinueFromResults()
    {
        if (ResultsManager.instance.GetPlayerVictory() == true)
        {
            currentState = States.victorious;
            
            // Load the victory level
            LevelLoadManager.instance.LoadVictoryLevel();
        }
        else 
        {
            currentState = States.mainmenu;

            // Load the main menu level
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void ContinueFromUpgrades()
    {
        currentState = States.gameplay;
    }

    // Updates the various UI elements on the upgrade screen
    public void UpdateUpgradeScreenUI()
    {
        Debug.Log("Updating upgrade screen UI");
        // update the cash text
        cashText.text = "Cash: $" + UpgradeManager.instance.GetCash().ToString("D2");
        // update the progress text
        // progressText.text = "Presents To Party: " + ResultsManager.instance.GetProgress().ToString("D1") + "/" + GameManager.instance.totalShopsToVisitForVictory.ToString("D1");
        // update the upgrade text
        grandmaSpritzerText.text = "$" + UpgradeManager.instance.GetUpgrade("GrandmaSpritzer").cost.ToString("D2");
        securityGuardText.text = "$" + UpgradeManager.instance.GetUpgrade("SecurityGuard").cost.ToString("D2");
        rooftopSniperText.text = "$" + UpgradeManager.instance.GetUpgrade("RooftopSniper").cost.ToString("D2");
        rocketPoweredScooterText.text = "$" + UpgradeManager.instance.GetUpgrade("RocketPoweredScooter").cost.ToString("D2");
        localWrestlerText.text = "$" + UpgradeManager.instance.GetUpgrade("LocalWrestler").cost.ToString("D2");
    }

    public void UpdateGameplayUI()
    {
        // update the cash text
        cashTextGameplay.text = "Cash: $" + UpgradeManager.instance.cash.ToString("D2");
    }

    public void UpdateResultsScreenUI()
    {
        int muggersClicked = ResultsManager.instance.GetMuggerAmountClicked();
        int muggerValue = UpgradeManager.instance.muggerValue;
        int totalMuggerValue = muggersClicked * muggerValue;
        muggersClickedText.text = "$" + totalMuggerValue.ToString("D2");
        int cultistsClicked = ResultsManager.instance.GetCultistAmountClicked();
        int cultistValue = UpgradeManager.instance.cultistValue;
        int totalCultistValue = cultistsClicked * cultistValue;
        cultistsClickedText.text = "$" + totalCultistValue.ToString("D2");
        int clownsClicked = ResultsManager.instance.GetClownAmountClicked();
        int clownValue = UpgradeManager.instance.clownValue;
        int balloonsClicked = ResultsManager.instance.GetBalloonAmountClicked();
        int balloonValue = UpgradeManager.instance.balloonValue;
        int totalClownValue = (clownsClicked * clownValue) + (balloonsClicked * balloonValue);
        clownsClickedText.text = "$" + totalClownValue.ToString("D2");
        // other villain scores go here
        int finalResultsValue = totalMuggerValue + totalCultistValue + totalClownValue;
        finalResultsText.text = "$" + finalResultsValue.ToString("D2");
        // update the total progress tracker
        int totalProgressTracker = ResultsManager.instance.GetProgress();
        progressTrackerText.text = totalProgressTracker.ToString("D2") + "/" + GameManager.instance.totalShopsToVisitForVictory.ToString("D3");
        // If the player is victorious, display the victory screen
        if (ResultsManager.instance.GetVillainVictory() == true)
        {
            //Debug.Log("Villain victory!");
            victory.SetActive(false);
            defeat.SetActive(true);
        }
        else 
        {
            //Debug.Log("Grandma victory!");
            victory.SetActive(true);
            defeat.SetActive(false);
        }
    }

    // This method is called when the player hovers over an upgrade button, and displays the upgrade's description and image
    void UpgradeHover(string elementName)
    {
        UpgradeManager.Upgrade upgrade = UpgradeManager.instance.GetUpgrade(elementName);
        if (upgrade.description != null)
        {
            informationText.text = upgrade.description;
        }
        else
        {
            informationText.text = "No description available.";
        }
        if (upgrade.image != null)
        {
            upgradeImage.sprite = upgrade.image;
        }
        else
        {
            Debug.Log("No image available.");
            upgradeImage.sprite = null;
        }
    }

    // Returns the current state
    public States GetCurrentState() => currentState;

    // This method can be used to test if a certain time has elapsed since we registered an event time. 
    public bool TimeElapsedSince(float timeEventHappened, float testingTimeElapsed) => !(timeEventHappened + testingTimeElapsed > Time.time);
}