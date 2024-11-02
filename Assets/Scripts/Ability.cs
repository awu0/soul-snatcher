using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{   
    protected int damage;
    protected int range;
    protected GameObject target;
    [SerializeField] private GameObject effect;

    public abstract void ActivateAbility(); //Apply damage to target, effect to self/target, and play animation

    private void SetDamage(int damageAmt)
    {
        damage = damageAmt;
    }

    private void SetRange(int rangeAmt)
    {
        range = rangeAmt;
    }

    public void SetTarget(GameObject targetObj)
    {
        target = targetObj;
    }

    protected void SetUp(int damageAmt=0, int rangeAmt=1, GameObject targetObj=null) 
    {
        SetDamage(damageAmt);
        SetRange(rangeAmt);
        SetTarget(targetObj);
    }
}
