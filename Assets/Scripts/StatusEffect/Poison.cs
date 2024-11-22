using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : StatusEffect
{   
    private int damage; 

    public override void InitializeSub()
    {
        damage = 1; //Mathf.Max(Mathf.RoundToInt(entity.maxHealth/6), 1);
        Type = StatusEffectType.Poison;
    }

    public override void ActivateEffect()
    {
        if (entity != null)
        {
            float damageTaken = entity.TakeDamage(damage);
            Debug.Log($"Poison effect: {damageTaken} damage dealt to {entity.name}.");
        }
    }
}
