using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    [NonSerialized] public float moveSpeed = 5f;
    [NonSerialized] public bool isMoving = false;
    private GameObject hitSparkPrefab;

    protected Ability ability;

    protected Grids grids;

    protected StatusEffectManager statusEffectManager;
    protected AudioManager audioManager;
    protected SpriteFlasher spriteFlasher;
    protected Player player;

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

        audioManager = FindAnyObjectByType<AudioManager>();

        spriteFlasher = gameObject.GetComponent<SpriteFlasher>();
        if (spriteFlasher == null) {
          spriteFlasher = gameObject.AddComponent<SpriteFlasher>();
        }

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null) 
        {
            player = playerObject.GetComponent<Player>();
        }

        hitSparkPrefab = Resources.Load<GameObject>("Prefabs/HitSpark");
        if (hitSparkPrefab == null)
        {
            Debug.LogError("hit spark prefab not found!");
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

    public void PlaceEntity(int x, int y) {
      if (!grids.IsPositionWithinBounds(x, y))
        {
            Debug.LogWarning($"{gameObject}.PlaceEntity({x}, {y}) is out of range!");
            return;
        }

        if (grids.IsCellOccupied(x, y))
        {
            Debug.LogWarning($"{gameObject}.PlaceEntity({x}, {y}), but there is already something there!");
            return;
        }

        var (oldX, oldY) = GetCurrentPosition();
        grids.SetCellOccupied(oldX, oldY, null);

        transform.position = new Vector3(x, y, transform.position.z);
        
        locX = x;
        locY = y;

        grids.SetCellOccupied(locX, locY, this);
    }

    /// <summary>
    /// Moves this entity to another point.
    /// Updates occupiedCells accordingly.
    /// </summary>
    /// <param name="newX">the x-coord to go to</param>
    /// <param name="newY">the y-coord to go to</param>
    
    public void MoveTo(int newX, int newY) {
      StartCoroutine(MoveToCoroutine(newX, newY));
    }

    public IEnumerator MoveToCoroutine(int newX, int newY) {
      isMoving = true;
      Debug.Log(this.type + " started moviing " + transform.position);

      var moveTask = MoveEntity(newX, newY);
      while (!moveTask.IsCompleted) {
        yield return null;
      }

      isMoving = false;
      Debug.Log(this.type + " finished moviing " + transform.position);
    }
    
    public async Task MoveEntity(int newX, int newY) {
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

        var (oldX, oldY) = GetCurrentPosition();
        grids.SetCellOccupied(oldX, oldY, null);

        Vector3 newPosition = new Vector3(newX, newY, transform.position.z);        

        await MoveEntityAsync(newPosition);
        
        locX = newX;
        locY = newY;

        grids.SetCellOccupied(locX, locY, this);
    }

    private async Task MoveEntityAsync(Vector3 newPosition) {
      Vector3 startPosition = transform.position;
      float journeyLength = Vector3.Distance(startPosition, newPosition);
      float startTime = Time.time;

      float moveTime = journeyLength / moveSpeed;
      float elapsedTime = 0;

      while (elapsedTime < moveTime) {
        elapsedTime += Time.deltaTime;
        float journeyFraction = elapsedTime / moveTime;
        
        transform.position = Vector3.Lerp(startPosition, newPosition, journeyFraction);
        await Task.Yield();
      }

      transform.position = newPosition;
    }

    /// <summary>
    /// Makes the entity take damage.
    /// </summary>
    /// <param name="amount">How much damage to take</param>
    /// <returns>The actual damage amount after modifiers</returns>
    public virtual int TakeDamage(int amount)
    {
        if (statusEffectManager.HasStatusEffect<Guarding>())
        {
            return 0;
        }

        Instantiate(hitSparkPrefab, transform.position, Quaternion.identity);

        health -= amount;
        if (health <= 0)
        {
            Die();
        }

        spriteFlasher.CallDamageSpriteFlasher();
        return amount;
    }

    public int Heal(int amount)
    {
        if (health + amount > maxHealth) {
            amount = maxHealth - health;
        }
        health += amount;

        return amount;
    }

    public void ReceiveStatusEffect(StatusEffect effect)
    {
        statusEffectManager.AddStatusEffect(effect);
    }

    public void TickDownStatusEffectsAndBuffs()
    {
        statusEffectManager.UpdateStatuses();
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