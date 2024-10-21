using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // Start to Pick Level button
    public void StartToPickLevel()
    {
        SceneManager.LoadScene("LevelPickScene");  // Loads the level selection scene
    }

    // Help button
    public void Help()
    {
        SceneManager.LoadScene("Tutorial"); 
    }

    // Quit button
    public void QuitGame()
    {
        // Application.Quit();
    }
}
