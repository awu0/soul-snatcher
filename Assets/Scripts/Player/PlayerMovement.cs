using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public int startX = 0;
    public int startY = 0;
    public int locX;
    public int locY;
    public int maxActionCount = 1;
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
        if (Input.GetKeyDown(KeyCode.W) && grids.HandlePlayerMovement(0, 1))
        {
            locY += 1;
            actionCount -= 1;
        }
        //move down
        else if (Input.GetKeyDown(KeyCode.S) && grids.HandlePlayerMovement(0, -1))
        {
            locY -= 1;
            actionCount -= 1;
        }
        //move left
        else if (Input.GetKeyDown(KeyCode.A) && grids.HandlePlayerMovement(-1, 0))
        {
            locX -= 1;
            actionCount -= 1;
        }
        //move right
        else if (Input.GetKeyDown(KeyCode.D) && grids.HandlePlayerMovement(1, 0))
        {
            locX += 1;
            actionCount -= 1;
        }
    }
}
