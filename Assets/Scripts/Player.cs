using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    private EnemyType type;

    public GameManager gameManager;

    private new void Start()
    {
        base.Start();

        actionCount = maxActionCount;

        type = EnemyType.Slime;

        SetStats(20, 5);
    }

    private void Update()
    {
        if (gameManager != null)
        {
            if (gameManager.state == GameManager.STATES.PLAYER_ROUND && actionCount > 0)
            {
                DetectForMovement();
            }
        }
    }

    private void DetectForMovement()
    {
        // move up
        if (Input.GetKeyDown(KeyCode.W))
        {
            HandlePlayerMovement(0, 1);
            actionCount -= 1;
        }
        // move down
        else if (Input.GetKeyDown(KeyCode.S))
        {
            HandlePlayerMovement(0, -1);
            actionCount -= 1;
        }
        // move left
        else if (Input.GetKeyDown(KeyCode.A))
        {
            HandlePlayerMovement(-1, 0);
            actionCount -= 1;
        }
        // move right
        else if (Input.GetKeyDown(KeyCode.D))
        {
            HandlePlayerMovement(1, 0);
            actionCount -= 1;
        }
    }

    /// <summary>
    /// Function to change the player's position
    /// </summary>
    /// <param name="x">x delta</param>
    /// <param name="y">y delta</param>
    private void HandlePlayerMovement(int x, int y)
    {
        var (playerX, playerY) = GetCurrentPosition();
        int newX = playerX + x;
        int newY = playerY + y;

        MoveTo(newX, newY);
        
        // Check if new cell has a soul in it
        if (grids.DoesCellHaveSoul(newX, newY)) {
            Soul soul = grids.soulCells[playerX, playerY];
            PickUpSoul(soul);
            grids.soulCells[playerX, playerY] = null;
            Destroy(soul.gameObject);
        }
    }

    public void PickUpSoul(Soul soul)
    {
        Debug.Log($"Picked up new soul type: {soul.Type}");
        type = soul.Type;
    }
}