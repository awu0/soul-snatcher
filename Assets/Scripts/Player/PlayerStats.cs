using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int level;
    public int maxHealth;
    public int health;
    public int attack;

    public void ChangeAttack(int atk)
    {
        attack = atk;
    }

    public void ChangeMaxHealth(int maxHp)
    {
        int currHealthRatio = health/maxHealth; //So the player doesnt just heal to full
        maxHealth = maxHp;
        health = maxHealth * currHealthRatio;
    }

    public void ChangeHealth(int hp)
    {
        health = hp;
    }
}
