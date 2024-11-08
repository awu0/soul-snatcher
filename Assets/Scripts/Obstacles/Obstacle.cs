using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Obstacle : Entity
{
    public new void Start()
    {
        base.Start();

        gameObject.tag = "Obstacle";
        
        // set the base stats
        EntityBaseStats stats = EntityData.EntityBaseStatMap[EntityType.Obstacle];
        SetStats(maxHealth: stats.MaxHealth, stats.Attack, stats.Range, EntityType.Obstacle);
    }
}