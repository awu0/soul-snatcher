using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType {
    Weakling,
};

public abstract class Enemy : MonoBehaviour
{
    public int health;
    public int attack;
    public int maxHealth;
    public EnemyType type;

    protected Grids grids;
    protected GameObject Player;

    public void Start()
    {
        gameObject.tag = "Enemy";
        
        var gridObject = GameObject.FindGameObjectWithTag("Game Board");
        if (gridObject != null)
        {
            grids = gridObject.GetComponent<Grids>();
        }

        Player = GameObject.FindGameObjectWithTag("Player");
    }

    /**
     * This function will be exposed to determine the behavior of the enemy.
     *
     * i.e.
     * - When they will attack
     * - Will they run away, charge the player, etc...
     */
    public abstract void DetermineNextMove();

    /**
     * Handles enemy movement, should only be called within DetermineNextMove
     */
    protected abstract void Move();

    /**
     * Handles ability use, should only be called within DetermineNextMove
     */
    protected abstract void UseAbility();

    /**
     * Checks whether the conditions are met for ability use.
     * True if you can use ability.
     */
    protected abstract bool AbilityConditionsMet();

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

    public void Die()
    {
        Debug.Log($"{gameObject.name} has died.");

        SpawnSoul();
        Destroy(gameObject);
    }

    /**
     * Returns this enemy's current position
     */
    protected (int x, int y) GetCurrentPosition()
    {
        return ((int)transform.position.x, (int)transform.position.y);
    }

    /**
     * Returns the player's current position
     */
    protected (int x, int y) GetPlayerPosition()
    {
        return ((int)Player.transform.position.x, (int)Player.transform.position.y);
    }

    /**
    * Spawns soul death object upon death
    */
    protected void SpawnSoul() {
      GameObject soul = Resources.Load<GameObject>("Prefabs/Soul");;

      if (soul == null) {
        Debug.Log($"Could not load soul object.");
        return;
      }

      (int x, int y) enemyPosition = GetCurrentPosition();
      Instantiate(soul, new Vector3(enemyPosition.x, enemyPosition.y, 0), Quaternion.identity);
      Soul soulScript = soul.GetComponent<Soul>();

      if (soulScript != null) {
        soulScript.Initialize(type);
      }
    }
}