﻿# Template for creating new enemy

```csharp
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Description
 */
public class NewEnemy : Enemy
{
    public new void Start()
    {
        base.Start();

        // set the base stats
        EntityBaseStats stats = EntityData.EntityBaseStatMap[EntityType.NewEnemy];
        SetStats(maxHealth: stats.MaxHealth, stats.Attack, stats.Range, EntityType.NewEnemy);
    }

    protected override void Move()
    {
        
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
```