using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager get;
    [SerializeField] GameObject UIscreen;
    [SerializeField] GameObject YouWinScreen;
    [SerializeField] GameObject YouLoseScreen;

    void Awake()
    {
        get = this;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void EndGame()
    {
        Application.Quit();
    }
    public void YouWin()
    {
        Debug.Log("You win!");
        Time.timeScale = 0f;
        UIscreen.SetActive(true);
        YouWinScreen.SetActive(true);
    }
    public void YouLose()
    {
        Debug.Log("You lose.");
        Time.timeScale = 0f;
        UIscreen.SetActive(true);
        YouLoseScreen.SetActive(true);
    }
}
