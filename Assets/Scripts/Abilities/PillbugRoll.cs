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
    /// 
    /// </summary>
    /// <param name="grids">The Grids object</param>
    /// <param name="direction">Which way to roll: Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right</param>
    public void ActivateAbility(Grids grids, Vector2Int direction)
    {
        // moves continuously in the specified direction until something is hit
        var (startX, startY) = Caster.GetCurrentPosition();
        
        Vector2Int current = new Vector2Int(startX, startY) + direction;
        
        while (!grids.IsCellOccupied(current.x, current.y) && grids.IsPositionWithinBounds(current.x, current.y))
        {
            Caster.MoveTo(current.x, current.y);
            current += direction;
        }
        
        // do damage
        Debug.Log($"{gameObject.name} used ability.");
    }
}
