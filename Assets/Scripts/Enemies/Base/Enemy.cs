using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType {
    Slime,
    Weakling,
    GiantPillbug,
};

public abstract class Enemy : Entity
{
    public EnemyType type;
    protected GameObject Player;

    public new void Start()
    {
        base.Start();
        
        gameObject.tag = "Enemy";
        
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

    public override void Die()
    {
        Debug.Log($"{gameObject.name} has died.");

        SpawnSoul();
        Destroy(gameObject);
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
      GameObject soulPrefab = Resources.Load<GameObject>("Prefabs/Soul");;

      if (soulPrefab == null) {
        Debug.Log($"Could not load soul prefab.");
        return;
      }

      (int x, int y) enemyPosition = GetCurrentPosition();
      GameObject newSoul = Instantiate(soulPrefab, new Vector3(enemyPosition.x, enemyPosition.y, 0), Quaternion.identity);
      Soul soulClass = newSoul.GetComponent<Soul>();

      if (soulClass == null) {
        Debug.Log($"Could not load newly created soul.");
        return;
      }

      soulClass.Initialize(type);

      Grids gridsClass = grids.GetComponent<Grids>();
      gridsClass.SetCellSoul(enemyPosition.x, enemyPosition.y, soulClass);
      Debug.Log($"Soul of type {type} created at {enemyPosition.x}, {enemyPosition.y}");
    }
}