using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Damages every entity in a straight line starting from the Caster.
/// </summary>
public class EyeLaser : Ability
{
    public override AbilityType Type => AbilityType.Directional;

    protected override void ActivateInternal(AbilityContext context) {
        Debug.Log($"{gameObject} used EyeLaser");
        audioManager.playEyeAbility();
        var directionalContext = (DirectionalContext)context;
        var (startX, startY) = Caster.GetCurrentPosition();

        Vector2Int current = new Vector2Int(startX, startY) + directionalContext.Direction;
        Grids grids = directionalContext.Grids;

        while (grids.IsPositionWithinBounds(current.x, current.y))
        {
            Entity entity = grids.GetEntityAt(current.x, current.y);
            if (entity != null)
            {
                entity.TakeDamage(directionalContext.Damage);     
            }
            current += directionalContext.Direction;
        }
    }
}
