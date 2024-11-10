using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{   
    /// The entity that will be using this ability.
    protected Entity Caster;

    public enum AbilityType {
      Targeted,
      Directional,
      Buff
    }

    public abstract AbilityType Type { get; }
    

    public void Initialize(Entity caster, int damage=0)
    {
        Caster = caster;
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
