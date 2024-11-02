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

    public override void Move()
    {
        Debug.Log("move a weakling"); 
    }

    public override void Attack()
    {
        Debug.Log($"{gameObject.name} is attacking.");
    }
}
