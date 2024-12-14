using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScreenElements : MonoBehaviour
{
    public Button startButton;
    public Button tutorialButton;
    void Start()
    {
        if (startButton != null)
        {
            startButton.onClick.AddListener(StartGame);
        }
        else
        {
            Debug.LogWarning("Start button not assigned!");
        }
        if (tutorialButton != null)
        {
            tutorialButton.onClick.AddListener(StartTutorial);
        }
        else
        {
            Debug.LogWarning("Tutorial button not assigned!");
        }
    }

    void Update()
    {
        
    }

    void StartGame()
    {
        SceneData.isTutorial = false;
        SceneManager.LoadScene("SampleScene");
    }

    void StartTutorial()
    {   
        SceneData.isTutorial = true;
        SceneManager.LoadScene("SampleScene");
    }
}
