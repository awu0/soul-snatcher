using System;
using UnityEngine;

/// <summary>
/// The base class for all entities in the game, including the Player and all enemies.
/// </summary>
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
    protected Ability ability;
    
    protected Grids grids;

    public void Awake()
    {
        var gridObject = GameObject.FindGameObjectWithTag("Game Board");
        if (gridObject != null)
        {
            grids = gridObject.GetComponent<Grids>();
        } 
    }

    public void Start()
    {
        actionCount = maxActionCount; 
    }
    
    protected void SetStats(int maxHealth, int attack, int range)
    {
        this.maxHealth = maxHealth;
        this.health = maxHealth;
        this.attack = attack;
        this.range = range;
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
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
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
