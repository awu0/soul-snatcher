using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Moves
/// </summary>
public class GiantPillbug : OneTileMovePerTurnEnemyType
{
    
    private List<Vector2Int> _path;
    
    public new void Start()
    {
        base.Start();

        // set the base stats
        EntityBaseStats stats = EntityData.EntityBaseStatMap[EntityType.GiantPillbug];
        SetStats(maxHealth: stats.MaxHealth, stats.Attack, stats.Range, EntityType.GiantPillbug);
        
        ability = gameObject.AddComponent<PillbugRoll>();
        ability.Initialize(caster: this, damage: attack);
    }

    protected override void Move()
    {
        if (_path.Count > 1)
        {
            Vector2Int nextMove = _path[1]; // The first move towards the target
            MoveTo(nextMove.x, nextMove.y); 
        }
    }

    protected override void UseAbility()
    {
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

            ((PillbugRoll)ability).ActivateAbility(context);
        }
    }
    
    protected void UseAbilityWithDirection(Vector2Int direction)
    {
        if (direction != Vector2Int.zero)
        {
            var context = new DirectionalContext {
                Grids = grids,
                Damage = attack,
                Direction = direction,
            };

            ((PillbugRoll)ability).ActivateAbility(context);
        }
    }

    /**
     * Attack Range: Unobstructed line of attack towards the player
     * Conditions: Player is in same row or column
     */
    protected override bool AbilityConditionsMet()
    {
        Vector2Int playerPosition = new Vector2Int(GetPlayerPosition().x, GetPlayerPosition().y);
        Vector2Int currentPosition = new Vector2Int(GetCurrentPosition().x, GetCurrentPosition().y);

        foreach (var direction in Directions)
        {
            Vector2Int current = currentPosition + direction;

            while (IsPositionValid(current) && !grids.IsCellOccupied(current.x, current.y))
            {
                current += direction;
            }
            
            if (current == playerPosition)
            {
                return true; 
            }
        }

        return false; 
    }

    protected override void DetermineNextMove()
    {
        // distance grid for regular movement
        var currentPosition = new Vector2Int(GetCurrentPosition().x, GetCurrentPosition().y);
        var distanceGrid = GetGridWithDistances(currentPosition);
        _path = GetPathToPlayer(distanceGrid);
        int normalMovementTurns = _path.Count - 1;
        
        // evaluate the cost of using the ability in all four directions
        int bestAbilityTurns = int.MaxValue;
        Vector2Int bestAbilityDirection = Vector2Int.zero;
        
        foreach (var direction in Directions)
        {
            int abilityTurns = SimulateAbility(direction);
            if (abilityTurns < bestAbilityTurns)
            {
                bestAbilityTurns = abilityTurns;
                bestAbilityDirection = direction;
            }
        }
        
        if (AbilityConditionsMet())
        {
            UseAbility(); 
        }
        else if (bestAbilityTurns < normalMovementTurns)
        {
            // use the ability
            Debug.Log($"Using ability to roll towards player. Direction: {bestAbilityDirection}");
            UseAbilityWithDirection(bestAbilityDirection);
        }
        else
        {
            // use normal movement
            Debug.Log("Using normal movement to approach player.");
            Move();
        }
    }

    private List<Vector2Int> GetPathToPlayer(int[,] distanceGrid)
    {
        var (playerX, playerY) = GetPlayerPosition();
        Vector2Int targetPosition = new Vector2Int(playerX, playerY);
        
        var path = GetPathToPoint(distanceGrid, targetPosition); // Get the path to the player
        return path;
    }
    
    /// <summary>
    /// Calcuates the total number of turns by normal movement it would take to reach the player after rolling.
    /// Considers stuns by adding 1 turn.
    /// </summary>
    /// <param name="direction">A direction to stimulate.</param>
    /// <returns>The number of turns</returns>
    private int SimulateAbility(Vector2Int direction)
    {
        var (startX, startY) = GetCurrentPosition();
        Vector2Int current = new Vector2Int(startX, startY);
    
        // how many turns are used up
        int turns = 1;
    
        // simulate using pill bug roll
        while (IsPositionValid(current + direction))
        {
            current += direction;
        }
        
        Vector2Int collidedPosition = current + direction;
    
        // check where the roll ends
        if (!grids.IsCellOccupied(collidedPosition.x, collidedPosition.y))
        {
            // hits a wall, so add 1 turn for the stun penalty
            turns++;
        }
        
        // calulate distance grid at new position
        int [,] stimulatedDistanceGrid = GetGridWithDistances(current);
        List<Vector2Int> stimulatedPath = GetPathToPlayer(stimulatedDistanceGrid);
        
        return stimulatedPath.Count - 1 + turns;
    }
}