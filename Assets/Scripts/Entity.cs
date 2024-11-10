using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The base class for all entities in the game, including the Player and all enemies.
/// </summary>
/// 
public abstract class Entity : MonoBehaviour
{
    [NonSerialized] public int locX;
    [NonSerialized] public int locY;
    
    [NonSerialized] public int maxActionCount = 1;
    [NonSerialized] public int actionCount = 0;
    
    [NonSerialized] public int health;
    [NonSerialized] public int attack;
    [NonSerialized] public int maxHealth;   
    [NonSerialized] public int range;
    [NonSerialized] public EntityType type;

    /// <summary>
    /// If guarding, takes no damage.
    /// </summary>
    [NonSerialized] public bool guarding = false;

    public int guardingDuration = 0;
    
    protected Ability ability;
    
    protected Grids grids;

    protected StatusEffectManager statusEffectManager;

    public void Awake()
    {
        var gridObject = GameObject.FindGameObjectWithTag("Game Board");
        if (gridObject != null)
        {
            grids = gridObject.GetComponent<Grids>();
        } 
        
    statusEffectManager = GetComponent<StatusEffectManager>();
    if (statusEffectManager == null)
    {
        statusEffectManager = gameObject.AddComponent<StatusEffectManager>();
    }
    }

    public void Start()
    {
        actionCount = maxActionCount; 
    }
    
    protected void SetStats(int maxHealth, int attack, int range, EntityType type)
    {
        this.maxHealth = maxHealth;
        this.health = maxHealth;
        this.attack = attack;
        this.range = range;
        this.type = type;
    }

    /// <summary>
    /// Moves this entity to another point.
    /// Updates occupiedCells accordingly.
    /// </summary>
    /// <param name="newX">the x-coord to go to</param>
    /// <param name="newY">the y-coord to go to</param>
    public void MoveTo(int newX, int newY)
    {
        if (!grids.IsPositionWithinBounds(newX, newY))
        {
            Debug.LogWarning($"{gameObject}.MoveTo({newX}, {newY}) is out of range!");
            return;
        }

        if (grids.IsCellOccupied(newX, newY))
        {
            Debug.LogWarning($"{gameObject}.MoveTo({newX}, {newY}), but there is already something there!");
            return;
        }
        
        var (x, y) = GetCurrentPosition();
        grids.SetCellOccupied(x, y, null);
            
        transform.position = new Vector3(newX, newY, transform.position.z);
            
        grids.SetCellOccupied(newX, newY, this);
        
        locX = newX;
        locY = newY;
    }
    
    public virtual void TakeDamage(int amount)
    {
        if (guarding) amount = 0;
        
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    public void ReceiveStatusEffect(StatusEffect effect)
    {
        statusEffectManager.AddStatusEffect(effect);
    }

    public void TickDownStatusEffectsAndBuffs()
    {   
        statusEffectManager.UpdateStatuses();
        if (guardingDuration > 0) guardingDuration--;
    }

    /// <summary>
    /// For guarding: Damage reduction resets at the beginning of the round.
    /// </summary>
    public void RemoveStatusEffectsAndBuffs()
    {
        if (guardingDuration == 0) guarding = false;
    }
    
    public virtual void Die()
    {
        Debug.Log($"{gameObject.name} has died.");

        Destroy(gameObject);
    }
    
    /**
     * Returns this entity's current position
     */
    public (int x, int y) GetCurrentPosition()
    {
        return (locX, locY);
    }
}
