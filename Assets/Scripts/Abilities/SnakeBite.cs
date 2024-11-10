using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBite : Ability
{
    public override AbilityType Type => AbilityType.Targeted;

    protected override void ActivateInternal(AbilityContext context)
    {
      Debug.Log($"{gameObject} used SnakeBite");
      var targetedContext = (TargetedContext)context;
      Entity target = targetedContext.Target;

      StatusEffect poison = new Poison(target, 2);
      target.ReceiveStatusEffect(poison);
    }

    // public void ActivateAbility(Entity target) {
    //     StatusEffect poison = new Poison(target, 2);
    //     target.ReceiveStatusEffect(poison);
    // }
}
