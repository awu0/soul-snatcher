using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class Enemy : Entity
{
    protected readonly Vector2Int[] Directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
    
    protected Player player;
    
    public AudioSource damageSFX;

    private GameObject enemyHitPrefab;

    public new void Start()
    {
        base.Start();
        
        gameObject.tag = "Enemy";
        
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        player = playerObject.GetComponent<Player>();

        enemyHitPrefab = Resources.Load<GameObject>("Prefabs/UI/EnemyHitText");
        if (enemyHitPrefab == null)
        {
            Debug.LogError("Prefab not found!");
        }
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

    /// <summary>
    /// Basic attack, which an enemy might have
    /// </summary>
    protected virtual void BasicAttack() {}
    protected virtual bool BasicAttackConditionMet() { return false; }

    /**
     * Checks whether the conditions are met for ability use.
     * True if you can use ability.
     */
    protected abstract bool AbilityConditionsMet();

    public override int TakeDamage(int amount)
    {
        damageSFX.Play();

        if (enemyHitPrefab != null)
        {
            showHitText(true, amount);
        }

        return base.TakeDamage(amount);
        
    }

    public override void Die()
    {
        Debug.Log($"{gameObject.name} has died.");

        // Create a soul object for dying enemy
        GameObject soulPrefab = Resources.Load<GameObject>("Prefabs/Soul");
        (int x, int y) enemyPosition = GetCurrentPosition();
        GameObject newSoul = Instantiate(soulPrefab, new Vector3(enemyPosition.x, enemyPosition.y, 0), Quaternion.identity);
        Soul soulClass = newSoul.GetComponent<Soul>();
        if (soulClass == null) {
          Debug.Log($"Could not load newly created soul.");
          return;
        }

        soulClass.Initialize(type);
        Destroy(gameObject);

        player.AbsorbSoul(soulClass);

        // Tempoary. Will add logic that displays the soul moving towards the player automatically
        Destroy(newSoul);
    }


    /**
     * Returns the player's current position
     */
    protected (int x, int y) GetPlayerPosition()
    {
        return (player.locX, player.locY);
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

    private void showHitText(bool takenDamage, int amount) {
        var hitText = Instantiate(enemyHitPrefab, gameObject.transform.position, Quaternion.identity);
        hitText.GetComponent<TextMeshPro>().text = amount.ToString();
        hitText.GetComponent<TextMeshPro>().color = Color.white;
    }

}