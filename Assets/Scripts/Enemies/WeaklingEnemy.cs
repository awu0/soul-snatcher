using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaklingEnemy : ChargingEnemyType
{
    // Start is called before the first frame update
    public new void Start()
    {
        base.Start();
        
        // set the base stats
        EntityBaseStats stats = EntityData.EntityBaseStatMap[EntityType.Weakling];
        SetStats(maxHealth: stats.MaxHealth, stats.Attack, stats.Range, EntityType.Weakling);
    }
    
    protected override void UseAbility()
    {
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
