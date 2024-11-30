using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : StatusEffect
{   
    private int damage; 

    public override void InitializeSub()
    {
        damage = Mathf.Max(Mathf.FloorToInt(entity.maxHealth/6), 1);
        Type = StatusEffectType.Poison;
    }

    public override void ActivateEffect()
    {
        if (entity != null)
        {   
            damage = Mathf.Max(Mathf.FloorToInt(entity.maxHealth/6), 1); //In case max hp changed
            if (entity.type == EntityType.Snake)
            {
                damage = 0;
            }
            float damageTaken = entity.TakeDamage(damage);
            Debug.Log($"Poison effect: {damageTaken} damage dealt to {entity.name}.");
        }
    }
}
