using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : StatusEffect
{   
    public override void InitializeSub()
    {
        Type = StatusEffectType.Stun;
    }

    public override void ActivateEffect()
    {
        // reduces action count to 0
        if (entity != null)
        {
            entity.actionCount = 0;
        }
    }
}