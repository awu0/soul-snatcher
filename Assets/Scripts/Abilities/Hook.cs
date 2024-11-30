using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Damages every entity in a straight line starting from the Caster.
/// </summary>
public class Hook : Ability
{
    public override AbilityType Type => AbilityType.Directional;

    protected override void ActivateInternal(AbilityContext context) {
        Debug.Log($"{gameObject} used Hook");
        
        audioManager.playHookAbility();
        
        var directionalContext = (DirectionalContext)context;
        var (startX, startY) = Caster.GetCurrentPosition();

        Vector2Int current = new Vector2Int(startX, startY) + directionalContext.Direction;
        Grids grids = directionalContext.Grids;
        
        // calculate the position directly in front of the caster
        Vector2Int hookedEntityNewPosition = new Vector2Int(startX, startY) + directionalContext.Direction;

        while (grids.IsPositionWithinBounds(current.x, current.y))
        {
            Entity entity = grids.GetEntityAt(current.x, current.y);
            if (entity != null)
            {
                entity.TakeDamage(directionalContext.Damage);
                
                // pull the entity towards the caster
                entity.MoveTo(hookedEntityNewPosition.x, hookedEntityNewPosition.y);
                
                // targets one entity
                break;
            }
            current += directionalContext.Direction;
        }
    }
}
