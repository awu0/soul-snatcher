# Template for creating new enemy

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
        SetStats(maxHp: 20, atk: 2);
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
        return true;
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