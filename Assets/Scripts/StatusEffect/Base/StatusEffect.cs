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
    public StatusEffectType Type { get; private set; }
    public int Duration { get; set; }
    protected Entity entity;

    public StatusEffect(StatusEffectType type, int duration, Entity entity)
    {
        Type = type;
        Duration = duration;
        this.entity = entity;
    }

    public void Remove()
    {
        Destroy(this.gameObject);
    }

    public abstract void ActivateEffect();
}
