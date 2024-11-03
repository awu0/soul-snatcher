using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public int startX = 0;
    public int startY = 0;
    public int locX;
    public int locY;
    public int maxActionCount = 10;
    public int actionCount = 0;
    public Grids grids;
    public GameObject turnManager;

    private void Start()
    {
        locX = startX;
        locY = startY;
        actionCount = maxActionCount;
        
        grids.SetCellOccupied(locX, locY, true);
    }

    private void Update()
    {
        if (turnManager != null) {
            if (turnManager.GetComponent<TurnManager>().state == TurnManager.STATES.PLAYER_ROUND) { 
                DetectForMovement();
            }
        }
    }

    void DetectForMovement() {
        //move up
        if (Input.GetKeyDown(KeyCode.W))
        {
            locY += 1;
            grids.HandlePlayerMovement(0, 1);
            actionCount -= 1;
        }
        //move down
        else if (Input.GetKeyDown(KeyCode.S))
        {
            locY -= 1;
            grids.HandlePlayerMovement(0, -1);
            actionCount -= 1;
        }
        //move left
        else if (Input.GetKeyDown(KeyCode.A))
        {
            locX -= 1;
            grids.HandlePlayerMovement(-1, 0);
            actionCount -= 1;
        }
        //move right
        else if (Input.GetKeyDown(KeyCode.D))
        {
            locX += 1;
            grids.HandlePlayerMovement(1, 0);
            actionCount -= 1;
        }
    }
}
