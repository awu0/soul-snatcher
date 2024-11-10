using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBite : Ability
{
    public override void ActivateAbility()
    {
        
    }

    public void ActivateAbility(Entity target) {
        StatusEffect poison = new Poison(target, 2);
        target.ReceiveStatusEffect(poison);
    }
}
