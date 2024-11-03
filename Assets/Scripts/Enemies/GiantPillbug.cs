using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *
 */
public class GiantPillbug : Enemy
{
    public new void Start()
    {
        base.Start();

        // set the base stats
        SetStats(maxHp: 20, atk: 2);
    }

    protected override void Move()
    {
        Debug.Log($"{gameObject.name} is moving.");
    }

    protected override void UseAbility()
    {
        var (playerX, playerY) = GetPlayerPosition();
        var (enemyX, enemyY) = GetCurrentPosition();
        
        Debug.Log($"{gameObject.name} used ability.");
    }

    /**
     * Attack Range: entire row or column
     * Conditions: Player is in same row or column
     */
    protected override bool AbilityConditionsMet()
    {
        var (playerX, playerY) = GetPlayerPosition();
        var (enemyX, enemyY) = GetCurrentPosition();

        return playerX == enemyX || playerY == enemyY;
    }

    public override void DetermineNextMove()
    {
        if (AbilityConditionsMet())
        {
            UseAbility();
        }
        else
        {
            Move();
        }
    }
}