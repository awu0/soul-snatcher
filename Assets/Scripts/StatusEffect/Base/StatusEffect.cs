using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect : MonoBehaviour
{
    public int frequency;
    public int stacks;
    public int maxStacks;

    public void SetEffect(int freq, int maxStks)
    {
        frequency = freq;
        maxStacks = maxStks;
    }

    public abstract void ActivateEffect();
}
