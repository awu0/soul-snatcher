using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This abstract enemy has a movement type of finding the shortest path to the player.
/// Moves 1 tile per turn.
/// The enemy that inherits this will have this movement behavior.
/// </summary>
public abstract class ChargingEnemyType : OneTileMovePerTurnEnemyType
{
    protected override void Move()
    {
        var currentPosition = new Vector2Int(GetCurrentPosition().x, GetCurrentPosition().y);
        var distanceGrid = GetGridWithDistances(currentPosition);
        
        var (playerX, playerY) = GetPlayerPosition();
        Vector2Int targetPosition = new Vector2Int(playerX, playerY);
        
        var path = GetPathToPoint(distanceGrid, targetPosition); // Get the path to the player

        if (path.Count > 1)
        {
            Vector2Int nextMove = path[1]; // The first move towards the target
            MoveTo(nextMove.x, nextMove.y);
        }
    }
}