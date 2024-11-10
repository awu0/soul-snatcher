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
        for (int i = 0; i < activeEffects.Count; i++)
        {
            var effect = activeEffects[i];

            effect.ActivateEffect();
            effect.Duration--;

            if (effect.Duration <= 0)
            {
                effect.Remove();
            }
        }
    }
}
