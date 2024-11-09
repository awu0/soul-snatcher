using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Guards against attacks for 1 turn.
/// </summary>
public class Guard : Ability
{
    public override void ActivateAbility()
    {
        Debug.Log($"{gameObject} used Guard");
        
        // blocks all damage
        Caster.guarding = true;
        Caster.guardingDuration = 1;
    }
}
