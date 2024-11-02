using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaklingEnemy : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        SetEnemyStats(maxHp: 20, atk: 2);
        Debug.Log("spawned a weakling"); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void TakeTurn()
    {
        
    }

    public override void Move()
    {
        Debug.Log("move a weakling"); 
    }

    public override void UseAbility()
    {
        Debug.Log($"{gameObject.name} used ability.");
    }

    public override bool CheckAbilityConditions()
    {
        Debug.Log($"{gameObject.name}'s ability conditions are met.");
        return true;
    }
}
