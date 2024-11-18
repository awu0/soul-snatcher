using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{   
    protected StatusEffectManager statusEffectManager;
    protected AudioManager audioManager;
    
    protected Entity Caster;
    public int damage;

    void Start()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
    }

    public enum AbilityType {
      Targeted,
      Directional,
      Buff
    }

    public abstract AbilityType Type { get; }
    
    public void Initialize(Entity caster, int damage = 0)
    {
        Caster = caster;
        this.damage = damage;
    }

    public virtual void ActivateAbility(AbilityContext context) {
      bool isValidContext = (Type == AbilityType.Targeted && context is TargetedContext) ||
                            (Type == AbilityType.Directional && context is DirectionalContext) ||
                            (Type == AbilityType.Buff && context is BuffContext);

      if (!isValidContext) {
        Debug.LogError($"Invalid context type for {GetType().Name}");
        return;
      }

      ActivateInternal(context);
    }

    protected abstract void ActivateInternal(AbilityContext context);
}
