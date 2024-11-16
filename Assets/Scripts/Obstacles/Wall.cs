using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = System.Object;

public class Wall : Entity
{
    public new void Start()
    {
        base.Start();
        
        gameObject.tag = "Wall";
    }

    public override int TakeDamage(int amount)
    {
        return 0;
    }

    public override void Die() { }
}