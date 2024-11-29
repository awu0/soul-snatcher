using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Frame : MonoBehaviour
{
    public UnityEngine.UI.Image frame;
    public GameManager manager;

    private bool playerTurn = false;

    private void Update()
    {
        if (manager != null && frame != null) {
            playerTurn = manager.GetComponent<GameManager>().state == GameManager.STATES.PLAYER_ROUND;
            if (playerTurn)
            {
                frame.color = Color.yellow;
            }
            else
            {
                frame.color = Color.white;
            }
        }
    }
}
