using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public int locX;
    public int locY;
    
    public int maxActionCount = 1;
    public int actionCount = 0;
    
    public int health;
    public int attack;
    public int maxHealth;   
    
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
        maxHealth = maxHp;
        health = maxHealth;
        attack = atk;
    }

    /**
     * Moves this entity to another point.
     * Updates occupiedCells accordingly.
     */
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
    protected (int x, int y) GetCurrentPosition()
    {
        return (locX, locY);
    }
}
