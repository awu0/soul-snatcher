using System;
using System.Collections.Generic;
using UnityEngine;

/**
 * This enemy has a movement type of finding the shortest path to the player. Moves 1 tile per turn.
 */
public abstract class ChargingEnemyType : Enemy
{
    // private Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

    protected override void Move()
    {
        var (playerX, playerY) = GetPlayerPosition();
        int enemyX = (int)transform.position.x;
        int enemyY = (int)transform.position.y;
        
        int differenceX = playerX - enemyX;
        int differenceY = playerY - enemyY;
        
        int newX = (int)enemyX;
        int newY = (int)enemyY;

        if (differenceX != 0)
        {
            newX += differenceX / Math.Abs(differenceX);
        }
        else if (differenceY != 0)
        {
            newY += differenceY / Math.Abs(differenceY);
        }
        
        transform.position = new Vector3(newX, newY, transform.position.z);
    }
}
