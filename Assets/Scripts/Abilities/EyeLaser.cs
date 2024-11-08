using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Damages every entity in a straight line starting from the Caster.
/// </summary>
public class EyeLaser : Ability
{
    public override void ActivateAbility()
    {
        
    }

    /// <summary>
    /// Damages every entity in the given direction until a wall is hit or another Entity is hit. 
    /// </summary>
    /// <param name="grids">The Grids object</param>
    /// <param name="direction">The direction of damage: Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right</param>
    public void ActivateAbility(Grids grids, Vector2Int direction)
    {
        Debug.Log($"{gameObject} used EyeLaser");
        
        // moves continuously in the specified direction until out of bounds
        var (startX, startY) = Caster.GetCurrentPosition();
        
        Vector2Int current = new Vector2Int(startX, startY) + direction;
        
        while (grids.IsPositionWithinBounds(current.x, current.y))
        {
            Entity entity = grids.GetEntityAt(current.x, current.y);
            if (entity != null)
            {
                entity.TakeDamage(damage);     
            }
            current += direction;
        }
    }
}