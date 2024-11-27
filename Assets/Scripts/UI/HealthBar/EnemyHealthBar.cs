using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Enemy Enemy;
    public UnityEngine.UI.Image healthBar;

    public float health;
    public float maxHealth;

    private void Update()
    {
        if (Enemy != null && healthBar != null) {
            health = Enemy.health;
            maxHealth = Enemy.maxHealth;
            float fill = health / maxHealth;
            healthBar.fillAmount = fill; ;
        }
    }
}
