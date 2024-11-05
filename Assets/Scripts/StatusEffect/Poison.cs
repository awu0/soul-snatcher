using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : StatusEffect
{   
    private Enemy target;

    void Start()
    {
        SetEffect(freq:1, maxStks:10);
    }

    public override void ActivateEffect()
    {
        target.health -= target.health/10;
    }
}
