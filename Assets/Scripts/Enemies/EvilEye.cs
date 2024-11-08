using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Description
 */
public class EvilEye : RangedEnemyType
{
    public new void Start()
    {
        base.Start();

        // set the base stats
        EntityBaseStats stats = EntityData.EntityBaseStatMap[EntityType.EvilEye];
        SetStats(maxHealth: stats.MaxHealth, stats.Attack, stats.Range, EntityType.EvilEye);
    }

    protected override void UseAbility()
    {
        
    }

    /**
     * Attack Range: straight line down
     * Conditions: directly across from player
     */
    protected override bool AbilityConditionsMet()
    {
        return false;
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