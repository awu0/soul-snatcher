using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBite : Ability
{
    public override void ActivateAbility() {
        Poison poison = Target.gameObject.AddComponent<Poison>();
        poison.Initialize(duration: 3, entity: Target);
        Target.ReceiveStatusEffect(poison);
        Target.TakeDamage(damage);
    }
}
