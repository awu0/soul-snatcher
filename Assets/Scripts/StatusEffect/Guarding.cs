using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guarding : StatusEffect
{   
    public override void InitializeSub()
    {
        Type = StatusEffectType.Guarding;
    }

    public override void ActivateEffect()
    {
        // block has no activated effects
    }
}