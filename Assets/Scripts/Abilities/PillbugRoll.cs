using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillbugRoll : Ability
{
    public override AbilityType Type => AbilityType.Directional;

    protected override void ActivateInternal(AbilityContext context)
    {
        Debug.Log($"{gameObject} used PillbugRoll");

        var directionalContext = (DirectionalContext)context;
        var (startX, startY) = Caster.GetCurrentPosition();

        Vector2Int currentPosition = new Vector2Int(startX, startY);
        Grids grids = directionalContext.Grids;

        Vector2Int nextPosition = currentPosition + directionalContext.Direction;
        while (!grids.IsCellOccupied(nextPosition.x, nextPosition.y) && grids.IsPositionWithinBounds(nextPosition.x, nextPosition.y))
        {
            currentPosition = nextPosition;
            nextPosition += directionalContext.Direction;
        }
        Caster.MoveTo(currentPosition.x, currentPosition.y);

        // check if the immediate next tile is an Entity, if so do damage to it
        if (grids.IsPositionWithinBounds(nextPosition.x, nextPosition.y) && grids.IsCellOccupied(nextPosition.x, nextPosition.y))
        {
            Entity target = grids.GetEntityAt(nextPosition.x, nextPosition.y);
            StartCoroutine(DealDamageWhenDoneMoving(Caster, directionalContext, target));
            // target.TakeDamage(directionalContext.Damage);
        }
        else
        {
            // entity is stunned for 1 turn otherwise
            Stun stunEffect = gameObject.AddComponent<Stun>();
            stunEffect.Initialize(1, Caster);
            
            Caster.ReceiveStatusEffect(stunEffect);
        }
    }

    private IEnumerator DealDamageWhenDoneMoving(Entity caster, DirectionalContext directionalContext, Entity target) {
        yield return new WaitUntil(() => caster.isMoving == false);

        // Move is complete, now deal damage
        target.TakeDamage(directionalContext.Damage);
    }

    /// <summary>
    /// Moves continuously in the given direction until a wall is hit or another Entity is hit. 
    /// </summary>
    /// <param name="grids">The Grids object</param>
    /// <param name="direction">Which way to roll: Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right</param>
    // public void ActivateAbility(Grids grids, Vector2Int direction)
    // {
    //     Debug.Log($"{gameObject} used PillbugRoll");
        
    //     // moves continuously in the specified direction until something is hit
    //     var (startX, startY) = Caster.GetCurrentPosition();
        
    //     Vector2Int current = new Vector2Int(startX, startY) + direction;
        
    //     while (!grids.IsCellOccupied(current.x, current.y) && grids.IsPositionWithinBounds(current.x, current.y))
    //     {
    //         Caster.MoveTo(current.x, current.y);
    //         current += direction;
    //     }
        
        
    //     // check if the immediate next tile is an Entity, if it is do damage to it
    //     if (grids.IsPositionWithinBounds(current.x, current.y) && grids.IsCellOccupied(current.x, current.y))
    //     {
    //         Entity target = grids.GetEntityAt(current.x, current.y);
    //         target.TakeDamage(damage);
    //     }
    // }
}
