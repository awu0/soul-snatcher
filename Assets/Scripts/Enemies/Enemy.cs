using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public int health;
    public int attack;
    public int maxHealth;
    
    protected Grid grids;
    protected GameObject player;

    public void Start()
    { 
        var gridObject = GameObject.FindGameObjectWithTag("Game Board");
        if (gridObject != null)
        {
            grids = gridObject.GetComponent<Grid>();
        }
        
        player = GameObject.FindGameObjectWithTag("Player");
    }

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

    protected void SetStats(int maxHp, int atk) 
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

    /**
     * Determines if the player is in range. Use this function to create the enemy's attack range.
     */
    protected virtual bool PlayerIsInRange()
    {
        return false;
    }

    public void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        Destroy(gameObject);
    }

    protected (int x, int y) GetPlayerPosition()
    {
        return ((int) player.transform.position.x, (int) player.transform.position.y);
    }
}
