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
    
    private int _health;
    private int _attack;
    private int _maxHealth;   
    protected Ability ability;
    
    protected Grids grids;

    public void Start()
    {
        actionCount = maxActionCount; 
        
        // initialize the Grids object
        var gridObject = GameObject.FindGameObjectWithTag("Game Board");
        if (gridObject != null)
        {
            grids = gridObject.GetComponent<Grids>();
        }
    }
    
    protected void SetStats(int maxHp, int atk)
    {
        _maxHealth = maxHp;
        _health = _maxHealth;
        _attack = atk;
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
        
        var (x, y) = GetCurrentPosition();
        grids.SetCellOccupied(x, y, false);
            
        transform.position = new Vector3(newX, newY, transform.position.z);
            
        grids.SetCellOccupied(newX, newY, true);
        
        locX = newX;
        locY = newY;
    }
    
    public virtual void TakeDamage(int amount)
    {
        _health -= amount;
        if (_health <= 0)
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
    protected (int x, int y) GetCurrentPosition()
    {
        return (locX, locY);
    }
}
