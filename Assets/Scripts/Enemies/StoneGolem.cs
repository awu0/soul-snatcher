using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Description
 */
public class StoneGolem : ChargingEnemyType
{
    public new void Start()
    {
        base.Start();

        // set the base stats
        EntityBaseStats stats = EntityData.EntityBaseStatMap[EntityType.StoneGolem];
        SetStats(maxHealth: stats.MaxHealth, stats.Attack, stats.Range, EntityType.StoneGolem);
    }

    protected override void UseAbility()
    {
        
    }

    /**
     * Attack Range:
     * Conditions:
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