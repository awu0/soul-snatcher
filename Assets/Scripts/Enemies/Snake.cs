using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : ChargingEnemyType
{
    public new void Start()
    {
        base.Start();
        
        // set the base stats
        EntityBaseStats stats = EntityData.EntityBaseStatMap[EntityType.Snake];
        SetStats(maxHealth: stats.MaxHealth, stats.Attack, stats.Range, EntityType.Snake);

        ability = gameObject.AddComponent<SnakeBite>();
        ability.Initialize(caster: this, damage: stats.Attack);
    }
    
    protected override void UseAbility()
    {   
        var context = new TargetedContext {
            Grids = grids,
            Damage = 3,
            Target = player,
        };

        animator.SetTrigger("Attacking");
        ((SnakeBite)ability).ActivateAbility(context);
        Debug.Log($"{gameObject.name} used ability.");
    }
    
    /**
     * Attack Range: 1, no diagonal
     * Conditions: Player is in range
     */
    protected override bool AbilityConditionsMet()
    {
        var (playerX, playerY) = GetPlayerPosition();
        var (enemyX, enemyY) = GetCurrentPosition();
        
        int deltaX = Mathf.Abs(playerX - enemyX);
        int deltaY = Mathf.Abs(playerY - enemyY);
        
        // check if the player is exactly 1 space away on either the x or y-axis, not diagonally
        return (deltaX == 1 && deltaY == 0) || (deltaX == 0 && deltaY == 1);
    }
    
    public override void DetermineNextMove()
    {
        if (AbilityConditionsMet())
        {
            UseAbility();
        }
        else
        {
            Move();
        }
    }
}
