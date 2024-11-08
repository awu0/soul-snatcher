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

        var distanceGrid = GetGridWithDistances(currentPosition);

        // get the target position, which is the shortest distance directly across from the player
        Vector2Int targetPosition = FindClosestStraightLinePosition(distanceGrid);

        var path = GetPathToPoint(distanceGrid, targetPosition); // Get the path to the player

        if (path.Count > 1)
        {
            Vector2Int nextMove = path[1]; // The first move towards the target
            MoveTo(nextMove.x, nextMove.y);
        }
    }

    private Vector2Int FindClosestStraightLinePosition(int[,] distanceGrid)
    {
        Vector2Int playerPosition = new Vector2Int(GetPlayerPosition().x, GetPlayerPosition().y);

        // check all horizontal and vertical cells from player and see which cell has the shortest travel distance
        int shortestDistance = int.MaxValue;
        Vector2Int closestPosition = new Vector2Int(GetCurrentPosition().x, GetCurrentPosition().y);
        foreach (var direction in Directions)
        {
            Vector2Int position = playerPosition + direction;

            while (grids.IsPositionWithinBounds(position.x, position.y))
            {
                // Check if this position aligns with the player's position in a straight line
                if (
                    (position.x == playerPosition.x || position.y == playerPosition.y) &&
                    !grids.IsCellOccupied(position.x, position.y) &&
                    distanceGrid[position.x, position.y] != -1
                )
                {
                    if (distanceGrid[position.x, position.y] < shortestDistance)
                    {
                        shortestDistance = distanceGrid[position.x, position.y];
                        closestPosition = position;
                    }
                }

                position += direction;
            }
        }

        return closestPosition;
    }
}