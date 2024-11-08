using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{   
    /// The entity that will be using this ability.
    protected Entity Caster;
    
    protected int damage;

    public void Initialize(Entity caster, int damage=0)
    {
        Caster = caster;
        this.damage = damage;
    }

    public abstract void ActivateAbility(); // Apply damage to target, effect to self/target, and play animation

    private void SetDamage(int damageAmt)
    {
        damage = damageAmt;
    }
}
