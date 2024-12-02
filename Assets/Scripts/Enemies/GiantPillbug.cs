using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Moves
/// </summary>
public class GiantPillbug : OneTileMovePerTurnEnemyType
{
    
    private int[,] _distanceGrid;
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

    /**
     * Attack Range: Unobstructed line of attack towards the player
     * Conditions: Player is in same row or column
     */
    protected override bool AbilityConditionsMet()
    {
        // if the path to the player is 2, that means that the next move will reach the player
        // so use the ability to charge the player
        return _path.Count == 2;
    }

    public override void DetermineNextMove()
    {
        var currentPosition = new Vector2Int(GetCurrentPosition().x, GetCurrentPosition().y);
        _distanceGrid = GetGridWithDistances(currentPosition);
        _path = GetPathToPlayer(_distanceGrid);
        
        if (AbilityConditionsMet())
        {
            UseAbility();
        }
        else
        {
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
}