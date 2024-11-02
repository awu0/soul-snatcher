using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaklingEnemy : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        health = 100;
        Debug.Log("spawned a weakling"); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    protected override void Move()
    {
        Debug.Log($"{gameObject.name} is moving.");
    }

    protected override void Attack()
    {
        Debug.Log($"{gameObject.name} is attacking.");
    }
}
