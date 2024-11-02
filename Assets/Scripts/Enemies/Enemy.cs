using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public float health;
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
    
    /**
     * Determines the enemy's movement
     */
    protected abstract void Move();

    /**
     * Handles how the enemy attacks
     */
    protected abstract void Attack();
    
    public virtual void TakeDamage(float amount)
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
