using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{   
    protected StatusEffectManager statusEffectManager;
    
    protected Entity Caster;
    protected Entity Target;
    protected int damage;

    public void Initialize(Entity caster, Entity target=null, int damage=0)
    {
        Caster = caster;
        Target = target;
        this.damage = damage;
    }

    public abstract void ActivateAbility(); // Apply damage to target, effect to self/target, and play animation

    private void SetDamage(int damageAmt)
    {
        damage = damageAmt;
    }
}
