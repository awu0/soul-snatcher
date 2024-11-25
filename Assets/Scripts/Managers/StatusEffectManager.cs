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

    // we use the sprite flasher to handle certain effect colors on sprites
    protected SpriteFlasher spriteFlasher;

    public void Awake() {
      spriteFlasher = gameObject.GetComponent<SpriteFlasher>();
      if (spriteFlasher == null) {
        spriteFlasher = gameObject.AddComponent<SpriteFlasher>();
      }
    }

    public void AddStatusEffect(StatusEffect effect)
    {
        if (effect.Type == StatusEffectType.Guarding) {
          spriteFlasher.CallGuardSpriteTint(true);
        }

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
                if (effect.Type == StatusEffectType.Guarding) {
                  spriteFlasher.CallGuardSpriteTint(false);
                }
                
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