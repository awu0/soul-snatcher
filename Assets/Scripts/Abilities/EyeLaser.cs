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
    public GameObject laserPrefab;
    public AnimationPlayer animationPlayer;

    protected override void ActivateInternal(AbilityContext context) {
        Debug.Log($"{gameObject} used EyeLaser");
        if (animationPlayer == null)
        {
            animationPlayer = FindObjectOfType<AnimationPlayer>();
            if (animationPlayer == null)
            {
                Debug.LogError("AnimationPlayer not found in the scene!");
                return;
            }
        }
        audioManager.playEyeAbility();
        var directionalContext = (DirectionalContext)context;
        var (startX, startY) = Caster.GetCurrentPosition();

        Vector2Int current = new Vector2Int(startX, startY) + directionalContext.Direction;
        Grids grids = directionalContext.Grids;

        if (laserPrefab == null)
        {
            Debug.LogWarning("laserPrefab is not assigned in the Inspector!");
        }
        else
        {
            Debug.Log("laserPrefab is assigned.");
        }

        int length = 0;
        while (grids.IsPositionWithinBounds(current.x, current.y))
        {
            Entity entity = grids.GetEntityAt(current.x, current.y);

            if (entity != null)
            {
                entity.TakeDamage(directionalContext.Damage);     
            }
            else
            {
                //Vector3 spawnPosition = new Vector3(current.x, current.y, 0f);
                //Instantiate(laserPrefab, spawnPosition, Quaternion.identity);
            }
            current += directionalContext.Direction;
            length += 1;
        }

        animationPlayer.ShootLaser(startX, startY, directionalContext.Direction, length);
    }
}
