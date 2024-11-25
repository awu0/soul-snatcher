using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Description
 */
public class StoneGolem : ChargingEnemyType
{
    private bool _guardedLastTurn = true;
    
    private int turnCount = 0;
    private int enrTurnCount = 15;
    
    public new void Start()
    {   
        base.Start();

        // set the base stats
        EntityBaseStats stats = EntityData.EntityBaseStatMap[EntityType.StoneGolem];
        SetStats(maxHealth: stats.MaxHealth, stats.Attack, stats.Range, EntityType.StoneGolem);
        
        ability = gameObject.AddComponent<Guard>();
        ability.Initialize(this);
    }

    protected override void UseAbility()
    {
        var context = new BuffContext {
          Grids = grids,
        };
        ((Guard)ability).ActivateAbility(context);
        _guardedLastTurn = true;
    }

    /**
     * Attack Range: none
     * Conditions: Alternatives between moving/attack and guarding each turn
     */
    protected override bool AbilityConditionsMet()
    {
        return !_guardedLastTurn;
    }

    protected override void BasicAttack()
    {
        player.TakeDamage(attack);
    }

    protected override bool BasicAttackConditionMet()
    {
        var (playerX, playerY) = GetPlayerPosition();
        var (enemyX, enemyY) = GetCurrentPosition();
        
        int deltaX = Mathf.Abs(playerX - enemyX);
        int deltaY = Mathf.Abs(playerY - enemyY);
        
        // check if the player is exactly 1 space away on either the x or y-axis, not diagonally
        Debug.Log($"player pos: {playerX}, {playerY}, my pos: {enemyX}, {enemyY}");
        return (deltaX == 1 && deltaY == 0) || (deltaX == 0 && deltaY == 1);
    }

    public override void DetermineNextMove()
    {
        if (turnCount == enrTurnCount) 
        {
            //Becomes enraged
            maxActionCount = 2;
        }
        while (actionCount > 0)
        {
            if (BasicAttackConditionMet())
            {
                BasicAttack();
                _guardedLastTurn = false;
            }
            else if (actionCount > 1)
            {
                Move();
                _guardedLastTurn = false;
            }
            else if (AbilityConditionsMet())
            {
                Debug.Log($"Golem is blocking {BasicAttackConditionMet()}");
                UseAbility();
            }
            else {
                Move();
                _guardedLastTurn = false;
            }
            actionCount--;
        }
        turnCount += 1;
    }
}