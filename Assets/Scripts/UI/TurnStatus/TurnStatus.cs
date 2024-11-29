using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnStatus : MonoBehaviour
{
    public TextMeshProUGUI statusText;
    public GameManager manager;

    private bool playerTurn = false;

    private void Update()
    {
        if (manager != null) {
            playerTurn = manager.GetComponent<GameManager>().state == GameManager.STATES.PLAYER_ROUND;



            if (statusText != null) {
                if (playerTurn)
                {
                    statusText.text = "Your turn";
                    statusText.color = Color.yellow;
                }
                else {
                    statusText.text = "Enemy turn";
                    statusText.color = Color.gray;
                }
            }
        }
    }
}
