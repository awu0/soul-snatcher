using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public int health;
    public int attack;
    public int maxHealth;

    /**
     * This function will be exposed to determine the behavior of the enemy.
     *
     * i.e.
     * - When they will attack
     * - Will they run away, charge the player, etc...
     */
    public virtual void DetermineNextMove()
    {
        // default behavior is just moving around
        Move();
    }

    //Handles enemy movement, should only be called within DetermineNextMove
    protected abstract void Move();

    //Handles ability use, should only be called within DetermineNextMove
    protected abstract void UseAbility();

    //Checks whether or not the conditions are met for ability use
    protected abstract bool CheckAbilityConditions();

    protected void SetEnemyStats(int maxHp, int atk) 
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
