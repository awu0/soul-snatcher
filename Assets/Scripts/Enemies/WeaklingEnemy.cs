using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaklingEnemy : ChargingEnemyType
{
    // Start is called before the first frame update
    public new void Start()
    {
        base.Start();
        
        health = 100;
        Debug.Log("spawned a weakling"); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void DetermineNextMove()
    {
        if (PlayerIsInRange())
        {
            Attack();
        }
        else
        {
            Move();
        }
    }
    
    protected override void Attack()
    {
        Debug.Log($"{gameObject.name} is attacking.");
    }

    /**
     * Weakling has an attack range of 1 and can't attack diagonally.
     */
    protected override bool PlayerIsInRange()
    {
        var (playerX, playerY) = GetPlayerPosition();
        int enemyX = (int)transform.position.x;
        int enemyY = (int)transform.position.y;
        
        int deltaX = Mathf.Abs(playerX - enemyX);
        int deltaY = Mathf.Abs(playerY - enemyY);
        
        // check if the player is exactly 1 space away on either the x or y-axis, not diagonally
        return (deltaX == 1 && deltaY == 0) || (deltaX == 0 && deltaY == 1);
    }
}
