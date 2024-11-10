﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Moves like a rook.
/// </summary>
public class GiantPillbug : Enemy
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
            ((PillbugRoll)ability).ActivateAbility(grids, direction);
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
        _distanceGrid = GetGridWithDistances();
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
    
   private int[,] GetGridWithDistances()
    {
        int[,] distanceGrid = new int[grids.columns, grids.rows];
        
        // Initialize distances to -1 (unreachable)
        for (int i = 0; i < grids.columns; i++)
        {
            for (int j = 0; j < grids.rows; j++)
            {
                distanceGrid[i, j] = -1;
            }
        }
        
        Vector2Int start = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        distanceGrid[start.x, start.y] = 0;

        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        queue.Enqueue(start);

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();
            int currentDistance = distanceGrid[current.x, current.y];

            // Traverse in each direction until blocked
            foreach (var direction in Directions)
            {
                Vector2Int neighbor = current + direction;
                while (IsPositionValid(neighbor) && distanceGrid[neighbor.x, neighbor.y] == -1)
                {
                    distanceGrid[neighbor.x, neighbor.y] = currentDistance + 1;
                    queue.Enqueue(neighbor);
                    neighbor += direction; // Continue in the same direction
                }
            }
        }
        
        return distanceGrid;
    }

    private bool IsPositionValid(Vector2Int position)
    {
        return position.x >= 0 && position.x < grids.columns &&
               position.y >= 0 && position.y < grids.rows &&
               !grids.IsCellOccupied(position.x, position.y); // Check for obstacles
    }

    private List<Vector2Int> GetPathToPlayer(int[,] distanceGrid)
    {
        var (playerX, playerY) = GetPlayerPosition();
        Vector2Int targetPosition = new Vector2Int(playerX, playerY);
        List<Vector2Int> path = new List<Vector2Int>();

        Vector2Int currentPosition = targetPosition;

        while (distanceGrid[currentPosition.x, currentPosition.y] != 0)
        {
            path.Add(currentPosition);

            Vector2Int? nextStep = null;
            int minDistance = int.MaxValue;

            foreach (var direction in Directions)
            {
                Vector2Int neighbor = currentPosition + direction;
                while (IsWithinBounds(neighbor) && distanceGrid[neighbor.x, neighbor.y] != -1)
                {
                    int neighborDistance = distanceGrid[neighbor.x, neighbor.y];
                    if (neighborDistance < minDistance)
                    {
                        minDistance = neighborDistance;
                        nextStep = neighbor;
                    }
                    neighbor += direction;
                }
            }

            if (nextStep == null)
            {
                Debug.LogWarning("No valid next step found.");
                break;
            }

            currentPosition = nextStep.Value;
        }

        path.Add(new Vector2Int((int)transform.position.x, (int)transform.position.y));
        path.Reverse();
        return path;
    }

    private bool IsWithinBounds(Vector2Int position)
    {
        return position.x >= 0 && position.x < grids.columns && 
               position.y >= 0 && position.y < grids.rows;
    }
    
    private void PrintGrid(int[,] grid)
    {
        Debug.Log("Distance grid:");
        for (int j = grids.rows - 1; j >= 0; j--)
        {
            string line = "";
            for (int i = 0; i < grids.columns; i++)
            {
                line += grid[i, j] + "\t";
            }
            Debug.Log(line);
        }
    }
}