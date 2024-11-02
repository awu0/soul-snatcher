using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public int health;
    public int attack;
    public int maxHealth;

    //Handles enemy movement, should only be called within TakeTurn
    public abstract void Move();

    //Handles ability use, should only be called within TakeTurn
    public abstract void UseAbility();

    //Checks whether or not the conditions are met for ability use
    public abstract bool CheckAbilityConditions();

    //Each enemy has their own system to decide on their action
    public abstract void TakeTurn();

    public void SetEnemyStats(int maxHp, int atk) 
    {
        maxHealth = maxHp;
        health = maxHealth;
        attack = atk;
    }
    
    public virtual void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        Destroy(gameObject);
    }
}
