using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public enum SELECTED
    {
        ATTACK,
        ABILITY, //Ability1, Ability2, ... 
    }

    public SELECTED selectedAction = SELECTED.ATTACK;

    public GameManager gameManager;

    private new void Start()
    {
        base.Start();

        actionCount = maxActionCount;

        EntityBaseStats stats = EntityData.EntityBaseStatMap[EntityType.Slime];
        SetStats(maxHealth: stats.MaxHealth, stats.Attack, stats.Range, EntityType.Slime);
    }

    private void Update()
    {
        if (gameManager != null)
        {
            if (gameManager.state == GameManager.STATES.PLAYER_ROUND && actionCount > 0)
            {   
                HandleLeftClickAction();
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
    }

    private void HandleLeftClickAction()
    {
        if (Input.GetMouseButtonDown(0))
        {   
            Entity entity = grids.GetEntityAtMouse(Input.mousePosition);
            if (entity && entity != this)
            {   
                BasicAttack(entity);
                actionCount -= 1;
            }
        }
    }

    private void BasicAttack(Entity entity) //Maybe only usable when your form allows basic attacks
    {   
        Debug.Log($"Attacked: {entity}");
        entity.TakeDamage(attack);
    }

    public void AbsorbSoul(Soul soul)
    {
        Debug.Log($"Absorbed new soul type: {soul.Type}");
        this.type = soul.Type;

        EntityBaseStats newStats = EntityData.EntityBaseStatMap[soul.Type];
        SetStats(maxHealth: newStats.MaxHealth, newStats.Attack, newStats.Range, soul.Type);

        Debug.Log($"Player is now of type: {this.type}");
    }
}