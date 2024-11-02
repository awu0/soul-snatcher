using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bash : Ability
{
    void Start()
    {
        SetUp(damageAmt: 10);
    }

    public override void ActivateAbility()
    {
        //target.changeHp(-damage);
        //Play animation
        //If has effect, apply it
    }
}
