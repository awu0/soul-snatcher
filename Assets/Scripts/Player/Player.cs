using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerMovement movement;
    private PlayerStats stats;

    void Start()
    {
        movement = GetComponent<PlayerMovement>();
        stats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        
    }

    public void TakeDamage(int amount)
    {
        stats.ChangeHealth(stats.health-amount);
        if (stats.health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        
    }
}
