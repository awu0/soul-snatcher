using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatusEffectType
{
    Poison,
    Block,
}

public abstract class StatusEffect : MonoBehaviour
{
    public StatusEffectType Type { get; protected set; }
    public int Duration { get; set; }
    protected Entity entity;

    public void Initialize(int duration, Entity entity)
    {
        Duration = duration;
        this.entity = entity;
        InitializeSub();
    }

    public void Removed()
    {   
        Debug.Log($"{this.name} has worn off.");
        Destroy(this);
    }

    public abstract void ActivateEffect();

    public abstract void InitializeSub(); //Init subclass things
}
