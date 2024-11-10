using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bash : Ability
{
    public override AbilityType Type => AbilityType.Targeted;
    void Start()
    {
    }

    protected override void ActivateInternal(AbilityContext context)
    {
        //target.changeHp(-damage);
        //Play animation
        //If has effect, apply it
    }
}
