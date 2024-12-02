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
        ability.Initialize(caster: this, damage: stats.Attack);
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
            var context = new DirectionalContext {
              Grids = grids,
              Damage = attack,
              Direction = direction,
            };

            ((EyeLaser)ability).ActivateAbility(context);
        }
    }

    /**
     * Attack Range: straight line down; no wall
     * Conditions: directly across from player
     */
    protected override bool AbilityConditionsMet()
    {
        var (currentX, currentY) = GetCurrentPosition();
        var (playerX, playerY) = GetPlayerPosition();

        if (currentX != playerX && currentY != playerY) return false;
        
        // check if the player is in sight and not blocked by walls
        
        // same column
        if (currentX == playerX)
        {
            int step = playerY > currentY ? 1 : -1;
            for (int y = currentY + step; y != playerY; y += step)
            {
                if (!grids.IsPositionWithinBounds(currentX, y)) return false;
            }
        }
        
        // same row
        if (currentY == playerY)
        {
            int step = playerX > currentX ? 1 : -1;
            for (int x = currentX + step; x != playerX; x += step)
            {
                if (!grids.IsPositionWithinBounds(x, currentY)) return false;
            }
        }

        return true;
    }

    protected override void DetermineNextMove()
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
 //       public override void Die()
 //   {
 //       Debug.Log($"{gameObject.name} has died.");
 //       audioManager.playEyeDeath();
 //       Destroy(gameObject);
 //   }
}