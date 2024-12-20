using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class Player : Entity
{
    public enum SELECTED
    {
        ATTACK,
        ABILITY, //Ability1, Ability2, ... 
        RECENT_TRANSFORM,
    }

    public SELECTED selectedAction = SELECTED.ATTACK;

    public GameManager gameManager;

    public Queue<Ability> abilities = new Queue<Ability>();
    public Ability selectedAbility;

    public EntityType? previousEntityType;
    public TextMeshProUGUI soulsSnatchedCountUI;

    public AudioSource damageSFX;
    public AudioSource transformSFX;
    public Animator animator;
    public int soulsSnatched;

    private new void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();

        actionCount = maxActionCount;
        previousEntityType = null;

        EntityBaseStats stats = EntityData.EntityBaseStatMap[EntityType.Slime];
        SetStats(maxHealth: stats.MaxHealth, stats.Attack, stats.Range, EntityType.Slime);

        soulsSnatched = 0;
    }

    private void Update()
    {
        if (!gameManager)
            return;

        if (gameManager.state != GameManager.STATES.PLAYER_ROUND || actionCount <= 0)
            return;
        
        DetectForModeSelectionInput();

        if (selectedAbility)
        {
            HandleAbilityInput();
            updateSelectedAction();
        }
        else if (selectedAction == SELECTED.RECENT_TRANSFORM)
        {
            HandleRecentTransformInput();
        }
        else
        {
            DetectForMovementInput();
            HandleLeftClickAction();
        }

        soulsSnatchedCountUI.text = $"Souls Snatched: {soulsSnatched}";
    }

    private void DetectForMovementInput()
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

    private void DetectForModeSelectionInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("Selected Basic Attack");
            selectedAbility = null;
            selectedAction = SELECTED.ATTACK;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectAbility(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedAbility = null;

            if (previousEntityType != null)
            {
                selectedAction = SELECTED.RECENT_TRANSFORM;
            }
        }

        // if (Input.GetKeyDown(KeyCode.Alpha3)) SelectAbility(1);
        // if (Input.GetKeyDown(KeyCode.Alpha4)) SelectAbility(2);
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

        // if the player tries to move to a spot that has an entity, attack it instead
        Entity possibleEntity = grids.GetEntityAt(newX, newY);
        if (possibleEntity != null)
        {
            BasicAttack(possibleEntity);
        }

        MoveTo(newX, newY);
    }

    private void HandleLeftClickAction()
    {
        if (selectedAction == SELECTED.ATTACK && Input.GetMouseButtonDown(0))
        {
            Entity entity = GetEntityAtMouse();
            if (entity && entity != this)
            {
                if (InRange(range, entity))
                {
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

        if (SceneData.isTutorial && gameManager.tutorialStep == 3) {
          gameManager.tutorialStep = 4;
          StartCoroutine(gameManager.ChangeTutorialStepEffect());
        }
    }

    private void SelectAbility(int index)
    {
        Ability[] abilitiesArray = abilities.ToArray();
        if (index >= abilitiesArray.Length)
        {
            Debug.Log($"Ability not found in slot: {index}");
            return;
        }

        // if (selectedAbility == abilitiesArray[index]) {
        //   selectedAbility = null;
        //   Debug.Log($"Deselected {abilitiesArray[index].GetType().Name}");
        //   return;
        // }

        Ability ability = abilitiesArray[index];
        Debug.Log($"Selecting ability: {ability.GetType().Name}");
        selectedAbility = ability;
    }

    private void HandleAbilityInput()
    {
        // used for handling WASD input for ability use
        Vector2Int direction = Vector2Int.zero;
        if (Input.GetKeyDown(KeyCode.W)) direction = Vector2Int.up;
        else if (Input.GetKeyDown(KeyCode.S)) direction = Vector2Int.down;
        else if (Input.GetKeyDown(KeyCode.A)) direction = Vector2Int.left;
        else if (Input.GetKeyDown(KeyCode.D)) direction = Vector2Int.right;
        
        switch (selectedAbility.Type)
        {
            // Directional Abilities use directional mouse input
            case Ability.AbilityType.Directional:
                (int x, int y) playerGridPosition = GetCurrentPosition();

                if (Input.GetMouseButtonDown(0))
                {
                    direction = grids.GetDirectionFromMouse(
                        playerPos: new Vector2Int(playerGridPosition.x, playerGridPosition.y),
                        mousePos: Input.mousePosition
                    );
                }

                if (direction != Vector2Int.zero)
                {
                    UseAbility(direction: direction);
                }

                break;
            // Targeted Abilities use mouse input
            case Ability.AbilityType.Targeted:
                Entity target = null;

                if (Input.GetMouseButtonDown(0))
                {
                    target = GetEntityAtMouse();
                }

                if (direction != Vector2Int.zero)
                {
                    (int x, int y) playerPos = GetCurrentPosition();
                    
                    target = grids.GetEntityAt(playerPos.x + direction.x, playerPos.y + direction.y);
                }
                
                if (!InRange(range, target))
                {
                    target = null;
                }

                if (target != null)
                {
                    UseAbility(target: target);
                }

                break;
            // Buff Abilities activate automatically
            case Ability.AbilityType.Buff:
                if (Input.GetMouseButtonDown(0))
                {
                    UseAbility();
                }
                
                if (direction != Vector2Int.zero)
                {
                    UseAbility();
                }

                break;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Deselected Ability");
            selectedAbility = null;
            return;
        }
    }

    private void HandleRecentTransformInput()
    {
        KeyCode[] keys = { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D };

        if (Input.GetMouseButtonDown(0) || keys.Any(Input.GetKeyDown))
        {
            TransformToMostRecentEnemy();
        }
    }

    public void AbsorbSoul(Soul soul)
    {
        Debug.Log($"Storing recent entity type: {this.type}");
        this.previousEntityType = this.type;

        transformSFX.volume = 0.5f;
        transformSFX.Play();

        SoulSnatchedUIController soulSnatchedUIController = gameObject.GetComponent<SoulSnatchedUIController>();
        soulSnatchedUIController.UpdateSoulSnatchedUIText(this.type, soul.Type);
        soulSnatchedUIController.DisplayAndFadeText(false);

        Debug.Log($"Absorbed new soul type: {soul.Type}");
        this.type = soul.Type;

        EntityBaseStats enemyStats = EntityData.EntityBaseStatMap[soul.Type];
        //EntityBaseStats newStats = CalculateNewStats(enemyStats);
        EntityBaseStats newStats = enemyStats;

        // EntityBaseStats newStats = EntityData.EntityBaseStatMap[soul.Type];
        SetStats(maxHealth: newStats.MaxHealth, newStats.Attack, newStats.Range, soul.Type);
        //Heal(maxHealth/5);

        // Gain ability of enemy
        GainAbility(soul.Type);

        // Gain Sprite of enemy
        PlayerSpriteChanger playerSpriteChanger = gameObject.GetComponent<PlayerSpriteChanger>();
        playerSpriteChanger.ChangePlayerSprite(soul.Type);

        Debug.Log($"Player is now of type: {this.type}");
        Debug.Log($"Player maxHealth: {this.maxHealth}");
        LogCurrentAbilities();

        soulsSnatched++;

        if (SceneData.isTutorial && gameManager.tutorialStep == 4) {
          gameManager.tutorialStep = 5;
          StartCoroutine(gameManager.ChangeTutorialStepEffect());
        } 
        else if (SceneData.isTutorial && (gameManager.tutorialStep == 8 || gameManager.tutorialStep == 9)) {
          gameManager.tutorialStep = 10;
          StartCoroutine(gameManager.ChangeTutorialStepEffect());
        }
    }

    private void TransformToMostRecentEnemy()
    {
        Debug.Log("Transforming to most recent enemy");

        if (this.previousEntityType == null)
        {
            Debug.Log("No recent enemy to turn into!");
            return;
        }

        this.selectedAbility = null;

        // we already asserted that previousEntityType is not null, so safe to cast here
        EntityType typeToTransformInto = (EntityType)this.previousEntityType;

        SoulSnatchedUIController soulSnatchedUIController = gameObject.GetComponent<SoulSnatchedUIController>();
        soulSnatchedUIController.UpdateSoulSnatchedUIText(this.type, typeToTransformInto);
        soulSnatchedUIController.DisplayAndFadeText(true);

        this.previousEntityType = null;

        // let's always select the attack after this occurs
        this.selectedAction = SELECTED.ATTACK;

        EntityBaseStats newEntityStats = EntityData.EntityBaseStatMap[typeToTransformInto];
        SetStats(maxHealth: newEntityStats.MaxHealth, newEntityStats.Attack, newEntityStats.Range, typeToTransformInto);

        GainAbility(typeToTransformInto);

        PlayerSpriteChanger playerSpriteChanger = gameObject.GetComponent<PlayerSpriteChanger>();
        playerSpriteChanger.ChangePlayerSprite(typeToTransformInto);
        this.spriteFlasher.CallTransformSpriteFlasher();

        Debug.Log($"Player is now of type: {this.type}");
        LogCurrentAbilities();

        soulsSnatched--;

        if (SceneData.isTutorial && gameManager.tutorialStep == 11) {
          gameManager.tutorialStep = 12;
          StartCoroutine(gameManager.ChangeTutorialStepEffect());
        } 
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
        // always clear old abilities first
        abilities.Clear();

        // Handle enemies without an ability
        if (!EntityData.EntityAbilityMap.TryGetValue(type, out Type abilityType) || abilityType == null)
        {
            Debug.Log("No ability found for enemy type: " + type);
            return;
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

        if (SceneData.isTutorial && gameManager.tutorialStep == 6) 
        {
          gameManager.tutorialStep = 7;
          StartCoroutine(gameManager.ChangeTutorialStepEffect());
        }
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

    public override int TakeDamage(int amount)
    {
        damageSFX.volume = 0.5f;
        damageSFX.Play();

        return base.TakeDamage(amount);
    }

    public void updateSelectedAction()
    {
        if (selectedAbility == null)
        {
            selectedAction = SELECTED.ATTACK;
        }
        else
        {
            selectedAction = SELECTED.ABILITY;
        }
    }

    public override void Die()
    {
        animator.SetTrigger("death");
        gameManager.ToggleDeathScreen(true);
        audioManager.playDead();
        audioManager.PauseMainTheme();
        gameManager.StopAllCoroutines();
    }

    public void Reset()
    {
        // reset health
        EntityBaseStats stats = EntityData.EntityBaseStatMap[this.type];
        SetStats(maxHealth: stats.MaxHealth, stats.Attack, stats.Range, this.type);

        selectedAction = SELECTED.ATTACK;
        // This is commented out since we're keeping enemy type on level swap
        // PlayerSpriteChanger playerSpriteChanger = gameObject.GetComponent<PlayerSpriteChanger>();
        // playerSpriteChanger.ChangePlayerSprite(EntityType.Slime);
        // ability = null;
        // abilities.Clear();
    }

    public string GetAbilityName()
    {
        if (ability != null)
        {
            return (ability.GetType().Name);
        }

        return null;
    }
}