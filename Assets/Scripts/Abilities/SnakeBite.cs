using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBite : Ability
{
    // public override void ActivateAbility() {
    //     Poison poison = Target.gameObject.AddComponent<Poison>();
    //     poison.Initialize(duration: 3, entity: Target);
    //     Target.ReceiveStatusEffect(poison);
    //     Target.TakeDamage(damage);
    public override AbilityType Type => AbilityType.Targeted;

    protected override void ActivateInternal(AbilityContext context)
    {
        Debug.Log($"{gameObject} used SnakeBite");
        var targetedContext = (TargetedContext)context;
        Entity target = targetedContext.Target;

        Poison poison = target.gameObject.AddComponent<Poison>();
        poison.Initialize(duration: 2, entity: target);
        target.ReceiveStatusEffect(poison);
        target.TakeDamage(damage);
    }

    // public void ActivateAbility(Entity target) {
    //     StatusEffect poison = new Poison(target, 2);
    //     target.ReceiveStatusEffect(poison);
    // }
}
