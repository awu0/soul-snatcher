using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : StatusEffect
{   
    private int damage; 

    public Poison(Entity entity, int duration) 
        : base(StatusEffectType.Poison, duration, entity)
    {
        damage = Mathf.Max(Mathf.FloorToInt(entity.maxHealth/10), 1);
    }

    public override void ActivateEffect()
    {
        if (entity != null)
        {
            entity.TakeDamage(damage);
            Debug.Log($"Poison effect: {damage} damage dealt to {entity.name}.");
        }
    }
}
