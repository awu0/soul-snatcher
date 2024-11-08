using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This abstract enemy has a movement type of trying to get the player in range on a straight line.
/// Moves 1 tile per turn.
/// The enemy that inherits this will have this movement behavior.
/// </summary>
public abstract class RangedEnemyType : OneTileMovePerTurnEnemyType
{
    protected override void Move()
    {
        var currentPosition = new Vector2Int(GetCurrentPosition().x, GetCurrentPosition().y);
        var playerPosition = new Vector2Int(GetPlayerPosition().x, GetPlayerPosition().y);
        
        var distanceGrid = GetGridWithDistances(currentPosition);
        
        // get the target position, which is the shortest distance directly across from the player
        Vector2Int targetPosition = FindClosestStraightLinePosition(currentPosition, playerPosition);
        
        var path = GetPathToPoint(distanceGrid, targetPosition); // Get the path to the player

        if (path.Count > 1)
        {
            Vector2Int nextMove = path[1]; // The first move towards the target
            MoveTo(nextMove.x, nextMove.y);
        }
    }
    
    private Vector2Int FindClosestStraightLinePosition(Vector2Int enemyPosition, Vector2Int playerPosition)
    {
        // Initialize variables to track the closest valid position
        Vector2Int closestPosition = enemyPosition;
        int shortestDistance = int.MaxValue;

        foreach (var direction in Directions)
        {
            int moves = 0;
            Vector2Int position = enemyPosition + direction;
            while (grids.IsPositionWithinBounds(position.x, position.y) && !grids.IsCellOccupied(position.x, position.y))
            {
                moves++;
                
                // Check if this position aligns with the player's position in a straight line
                if ((position.x == playerPosition.x || position.y == playerPosition.y) && position != enemyPosition)
                {
                    if (moves < shortestDistance)
                    {
                        shortestDistance = moves;
                        closestPosition = position;
                    }
                }
                position += direction;
            }
        }

        return closestPosition;
    }
}
