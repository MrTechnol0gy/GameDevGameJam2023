using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public void StartGame()
    {
        // Loads the gameplay scene
        SceneManager.LoadScene("Gameplay");
    }
    
    // Quits the application
    public void QuitGame()
    {
        Application.Quit();
    }
}
