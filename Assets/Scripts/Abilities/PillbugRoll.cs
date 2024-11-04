using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillbugRoll : Ability
{
    public override void ActivateAbility()
    {
        
    }

    /// <summary>
    /// Moves continuously in the given direction until a wall is hit or another Entity is hit. 
    /// </summary>
    /// <param name="grids">The Grids object</param>
    /// <param name="direction">Which way to roll: Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right</param>
    public void ActivateAbility(Grids grids, Vector2Int direction)
    {
        Debug.Log($"{gameObject} used PillbugRoll");
        
        // moves continuously in the specified direction until something is hit
        var (startX, startY) = Caster.GetCurrentPosition();
        
        Vector2Int current = new Vector2Int(startX, startY) + direction;
        
        while (!grids.IsCellOccupied(current.x, current.y) && grids.IsPositionWithinBounds(current.x, current.y))
        {
            Caster.MoveTo(current.x, current.y);
            current += direction;
        }
        
        
        // check if the immediate next tile is an Entity, if it is do damage to it
        if (grids.IsPositionWithinBounds(current.x, current.y) && grids.IsCellOccupied(current.x, current.y))
        {
            Entity target = grids.GetEntityAt(current.x, current.y);
            target.TakeDamage(damage);
        }
    }
}
