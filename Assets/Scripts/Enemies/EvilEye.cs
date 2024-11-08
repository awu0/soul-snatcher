using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Shoots everything in a straight line, multi hit
 */
public class EvilEye : RangedEnemyType
{
    public new void Start()
    {
        base.Start();

        // set the base stats
        EntityBaseStats stats = EntityData.EntityBaseStatMap[EntityType.EvilEye];
        SetStats(maxHealth: stats.MaxHealth, stats.Attack, stats.Range, EntityType.EvilEye);
        
        ability = gameObject.AddComponent<EyeLaser>();
        ability.Initialize(this, stats.Attack);
    }

    protected override void UseAbility()
    {
        Debug.Log($"{gameObject.name} used ability.");
        
        // determine the correct direction
        var (playerX, playerY) = GetPlayerPosition();
        var (currentX, currentY) = GetCurrentPosition();
        
        Vector2Int direction = Vector2Int.zero;

        // determine direction if in the same row or column
        if (currentX == playerX) // same column, move vertically
        {
            direction = playerY > currentY ? Vector2Int.up : Vector2Int.down;
        }
        else if (currentY == playerY) // same row, move horizontally
        {
            direction = playerX > currentX ? Vector2Int.right : Vector2Int.left;
        }
        
        // Only activate the ability if a direction is determined
        if (direction != Vector2Int.zero)
        {
            ((EyeLaser)ability).ActivateAbility(grids, direction);
        }
    }

    /**
     * Attack Range: straight line down
     * Conditions: directly across from player
     */
    protected override bool AbilityConditionsMet()
    {
        return GetCurrentPosition().x == GetPlayerPosition().x || GetCurrentPosition().y == GetPlayerPosition().y;
    }

    public override void DetermineNextMove()
    {
        if (AbilityConditionsMet())
        {
            UseAbility();
        }
        else
        {
            Move();
        }
    }
}