using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaklingEnemy : ChargingEnemyType
{
    // Start is called before the first frame update
    public new void Start()
    {
        base.Start();
        
        SetStats(maxHp: 20, atk: 2);
        Debug.Log("spawned a weakling"); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void UseAbility()
    {
        Debug.Log($"{gameObject.name} used ability.");
    }

    protected override bool CheckAbilityConditions()
    {
        Debug.Log($"{gameObject.name}'s ability conditions are met.");
        return true;
    }

    /**
     * Weakling has an attack range of 1 and can't attack diagonally.
     */
    protected override bool PlayerIsInRange()
    {
        var (playerX, playerY) = GetPlayerPosition();
        var (enemyX, enemyY) = GetCurrentPosition();
        
        int deltaX = Mathf.Abs(playerX - enemyX);
        int deltaY = Mathf.Abs(playerY - enemyY);
        
        // check if the player is exactly 1 space away on either the x or y-axis, not diagonally
        return (deltaX == 1 && deltaY == 0) || (deltaX == 0 && deltaY == 1);
    }
}
