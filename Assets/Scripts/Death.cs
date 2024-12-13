using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Death : MonoBehaviour
{
    public Button startButton;
    public Button menuButton;
    public GameManager gameManager;

    void Start()
    {
        if (startButton != null)
        {
            startButton.onClick.AddListener(StartGame);
        }
        if (menuButton != null)
        {
            menuButton.onClick.AddListener(MenuOpen);
        }
    }

    void Update()
    {
        
    }

    void StartGame()
    {
        gameManager.ResetGame();
        gameManager.ToggleDeathScreen(false);
    }

    void MenuOpen()
    {
        SceneManager.LoadScene("StartScreen");
        gameManager.ToggleDeathScreen(false);
    }
}
