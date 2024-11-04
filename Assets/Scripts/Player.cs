using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    [NonSerialized] public int startX = 1;
    [NonSerialized] public int startY = 1;
    
    private EnemyType type;

    public GameObject turnManager;
    
    private new void Start()
    {
        base.Start();
        
        locX = startX;
        locY = startY;
        
        actionCount = maxActionCount;
        
        type = EnemyType.Slime;
        
    }

    private void Update()
    {
        if (turnManager != null) {
            if (turnManager.GetComponent<TurnManager>().state == TurnManager.STATES.PLAYER_ROUND) { 
                DetectForMovement();
            }
        }
    }
    
    private void DetectForMovement() {
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
    
    public void PickUpSoul(Soul soul) {
        Debug.Log($"Picked up new soul type: {soul.Type}");
        type = soul.Type;
    }
}
