using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public enum SELECTED
    {
        ATTACK,
        ABILITY, //Ability1, Ability2, ... 
    }

    public SELECTED selectedAction = SELECTED.ATTACK;

    public GameManager gameManager;

    public Queue<Ability> abilities = new Queue<Ability>();
    public AudioSource damageSFX;
    private Ability selectedAbility;

    private new void Start()
    {
        base.Start();

        actionCount = maxActionCount;

        EntityBaseStats stats = EntityData.EntityBaseStatMap[EntityType.Slime];
        SetStats(maxHealth: stats.MaxHealth, stats.Attack, stats.Range, EntityType.Slime);
    }

    private void Update()
    {
        if (gameManager != null)
        {
            if (gameManager.state == GameManager.STATES.PLAYER_ROUND && actionCount > 0)
            {   
              DetectForAbilitySelection();
              if (selectedAbility != null) {
                HandleAbilityInput();
              } else {
                HandleLeftClickAction();
                DetectForMovement();
              }
            }
        }
    }

    private void DetectForMovement()
    {
        // move up
        if (Input.GetKeyDown(KeyCode.W))
        {
            HandlePlayerMovement(0, 1);
            actionCount -= 1;
        }
        // move down
        else if (Input.GetKeyDown(KeyCode.S))
        {
            HandlePlayerMovement(0, -1);
            actionCount -= 1;
        }
        // move left
        else if (Input.GetKeyDown(KeyCode.A))
        {
            HandlePlayerMovement(-1, 0);
            actionCount -= 1;
        }
        // move right
        else if (Input.GetKeyDown(KeyCode.D))
        {
            HandlePlayerMovement(1, 0);
            actionCount -= 1;
        }
    }

    private void DetectForAbilitySelection()
    {
        // if (Input.GetKeyDown(KeyCode.Alpha1))
        // {
        //     Debug.Log("Selected Basic Attack");
        //     selectedAbility = null;
        //     selectedAction = SELECTED.ATTACK;
        // }

        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectAbility(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectAbility(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SelectAbility(2);
    }

    /// <summary>
    /// Function to change the player's position
    /// </summary>
    /// <param name="x">x delta</param>
    /// <param name="y">y delta</param>
    private void HandlePlayerMovement(int x, int y)
    {
        var (playerX, playerY) = GetCurrentPosition();
        int newX = playerX + x;
        int newY = playerY + y;

        MoveTo(newX, newY);
    }

    private void HandleLeftClickAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Entity entity = GetEntityAtMouse();
            if (entity && entity != this)
            {   
              if (InRange(range, entity)) {
                BasicAttack(entity);
                actionCount -= 1;
              }
            }
        }
    }

    private Entity GetEntityAtMouse()
    {
        return grids.GetEntityAtMouse(Input.mousePosition);
    }

    private void BasicAttack(Entity entity) //Maybe only usable when your form allows basic attacks
    {
        Debug.Log($"Attacked: {entity}");
        entity.TakeDamage(attack);
    }

    private void SelectAbility(int index)
    {
        Ability[] abilitiesArray = abilities.ToArray();
        if (index >= abilitiesArray.Length)
        {
            Debug.Log($"Ability not found in slot: {index}");
            return;
        }

      if (selectedAbility == abilitiesArray[index]) {
        selectedAbility = null;
        Debug.Log($"Deselected {abilitiesArray[index].GetType().Name}");
        return;
      }

      Ability ability = abilitiesArray[index];

        Debug.Log($"Selecting ability: {ability.GetType().Name}");

        selectedAbility = ability;
    }

    private void HandleAbilityInput()
    {
        switch (selectedAbility.Type)
        {
            // Directional Abilities use WASD
            case Ability.AbilityType.Directional:
                Vector2Int direction = Vector2Int.zero;

                if (Input.GetKeyDown(KeyCode.W)) direction.y = 1;
                if (Input.GetKeyDown(KeyCode.S)) direction.y = -1;
                if (Input.GetKeyDown(KeyCode.D)) direction.x = 1;
                if (Input.GetKeyDown(KeyCode.A)) direction.x = -1;

                if (direction != Vector2Int.zero)
                {
                    UseAbility(direction: direction);
                }

                break;
            // Targeted Abilities use mouse input
            case Ability.AbilityType.Targeted:
                Entity target = null;

                if (Input.GetMouseButtonDown(0)) {
                  target = GetEntityAtMouse();
                  if (!InRange(range, target)) {
                    target = null;
                  }
                }

                if (target != null)
                {
                    UseAbility(target: target);
                }

                break;
            // Buff Abilities activate automatically
            case Ability.AbilityType.Buff:
                UseAbility();
                break;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Deselected Ability");
            selectedAbility = null;
            return;
        }
    }

    public void AbsorbSoul(Soul soul)
    {
        Debug.Log($"Absorbed new soul type: {soul.Type}");
        this.type = soul.Type;

        EntityBaseStats enemyStats = EntityData.EntityBaseStatMap[soul.Type];
        EntityBaseStats newStats = CalculateNewStats(enemyStats);

        // EntityBaseStats newStats = EntityData.EntityBaseStatMap[soul.Type];
        SetStats(maxHealth: newStats.MaxHealth, newStats.Attack, newStats.Range, soul.Type);

        GainAbility(soul.Type);

        Debug.Log($"Player is now of type: {this.type}");
        Debug.Log($"Player maxHealth: {this.maxHealth}");
        LogCurrentAbilities();
    }

    public EntityBaseStats CalculateNewStats(EntityBaseStats enemyStats)
    {
        EntityBaseStats oriStats = EntityData.EntityBaseStatMap[EntityType.Slime];

        int enemyStatTotal = enemyStats.Attack + enemyStats.MaxHealth;
        int statTotal = oriStats.MaxHealth + oriStats.Attack;

        int newAttack = Mathf.RoundToInt(statTotal * (enemyStats.Attack / (float)enemyStatTotal));
        int newMaxHealth = Mathf.RoundToInt(statTotal * (enemyStats.MaxHealth / (float)enemyStatTotal));

        return new EntityBaseStats(attack: newAttack, maxHealth: newMaxHealth, range: enemyStats.Range);
    }

    public void GainAbility(EntityType type)
    {
        // Handle enemies without an ability
        if (!EntityData.EntityAbilityMap.TryGetValue(type, out Type abilityType) || abilityType == null)
        {
            Debug.Log("No ability found for enemy type: " + type);
            return;
        }

        // Remove old ability if we have too many
        if (abilities.Count >= 3)
        {
            Ability oldAbility = abilities.Dequeue();
            if (oldAbility != null)
            {
                Destroy(oldAbility);
                Debug.LogWarning("Removed ability: " + oldAbility);
            }
        }

        // Add ability
        if (!typeof(Ability).IsAssignableFrom(abilityType))
        {
            Debug.LogError($"Type {abilityType.Name} does not derive from Ability!");
            return;
        }


        Ability newAbility = gameObject.AddComponent(abilityType) as Ability;
        newAbility.Initialize(caster: this, damage: attack);

        if (newAbility != null)
        {
            abilities.Enqueue(newAbility);
            Debug.Log("Successfully added ability: " + abilityType.Name);
        }
        else
        {
            Debug.LogWarning("Failed to add ability component of type: " + abilityType.Name);
        }
    }

    public void LogCurrentAbilities()
    {
        foreach (var ability in abilities)
        {
            if (ability != null)
            {
                name = ability.GetType().Name;
                Debug.Log($"Ability: {name}");
            }
            else
            {
                Debug.Log("Null ability found in queue");
            }
        }
    }

    private void UseAbility(Vector2Int direction = default, Entity target = null)
    {
        if (selectedAbility == null) return;

        switch (selectedAbility.Type)
        {
            case Ability.AbilityType.Directional:
                var dirContext = new DirectionalContext
                {
                    Grids = grids,
                    Direction = direction,
                    Damage = selectedAbility.damage
                };

                selectedAbility.ActivateAbility(dirContext);
                break;
            case Ability.AbilityType.Targeted:
                var targetedContext = new TargetedContext
                {
                    Grids = grids,
                    Target = target,
                    Damage = selectedAbility.damage
                };

                selectedAbility.ActivateAbility(targetedContext);
                break;
            case Ability.AbilityType.Buff:
                var buffContext = new BuffContext
                {
                    Grids = grids
                };

                selectedAbility.ActivateAbility(buffContext);
                break;
        }

        selectedAbility = null;
        actionCount -= 1;
    }

    public bool InRange(int range, Entity entity)
    {
      if (entity != null)
      {
        int distanceX = Mathf.Abs(entity.locX - this.locX);
        int distanceY = Mathf.Abs(entity.locY - this.locY);

        int totalDistance = distanceX + distanceY;
        return totalDistance <= range;
      }

      return false;
    }

    override public int TakeDamage(int amount)
    {
        if (statusEffectManager.HasStatusEffect<Guarding>())
        {
            amount = 0;
        }

        
        damageSFX.Play();
        health -= amount;

        if (health <= 0)
        {
            Die();
        }

        return amount;
    }
}