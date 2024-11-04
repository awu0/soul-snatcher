using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerMovement movement;
    private PlayerStats stats;
    private EnemyType type;

    void Start()
    {
        movement = GetComponent<PlayerMovement>();
        stats = GetComponent<PlayerStats>();
        type = EnemyType.Slime;
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

    public void PickUpSoul(Soul soul) {
      Debug.Log($"Picked up new soul type: {soul.Type}");
      type = soul.Type;
    }

    public void Die()
    {
        
    }
}
