using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Each Entity has its own status manager
*/

public class StatusEffectManager : MonoBehaviour
{
    private List<StatusEffect> activeEffects = new List<StatusEffect>();

    public void AddStatusEffect(StatusEffect effect)
    {
        activeEffects.Add(effect);
    }

    public void UpdateStatuses()
    {
        foreach (var effect in activeEffects)
        {
            effect.ActivateEffect();
            effect.Duration--;
        }
        
        // remove expired effects
        activeEffects.RemoveAll(effect =>
        {
            if (effect.Duration <= 0)
            {
                effect.Removed();
                return true;
            }
            return false;
        });
    }

    public bool HasStatusEffect<T>() where T : StatusEffect
    {
        foreach (var effect in activeEffects)
        {
            if (effect is T) return true;
        }

        return false;
    }
}