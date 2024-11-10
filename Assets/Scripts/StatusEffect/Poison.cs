using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : StatusEffect
{   
    private int damage; 

    public override void InitializeSub()
    {
        damage = Mathf.Max(Mathf.RoundToInt(entity.maxHealth/12), 1);
        Type = StatusEffectType.Poison;
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
