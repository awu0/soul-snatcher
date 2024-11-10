using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{   
    protected StatusEffectManager statusEffectManager;
    
    protected Entity Caster;
    protected Entity Target;
    protected int damage;

    public enum AbilityType {
      Targeted,
      Directional,
      Buff
    }

    public abstract AbilityType Type { get; }
    
    public void Initialize(Entity caster, Entity target=null, int damage=0)
    {
        Caster = caster;
        Target = target;
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
