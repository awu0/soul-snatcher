using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private const float DAMAGED_HEALTH_FADE_TIMER_MAX = 1f;
    public Player playerScript;
    public Image healthBar;
    public Image damagedBar;
    public bool takeDamage;
    public bool heal;
    private float currentHealth;
    private Color damagedColor;
    private float damagedHealthFadeTimer;

    private void Awake()
    {
        damagedColor = damagedBar.color;
        damagedColor.a = 0f;
        damagedBar.color = damagedColor;
    }
    private void Start()
    {
        playerScript = FindObjectOfType<Player>();
        currentHealth = playerScript.health;
    }

    private void Update()
    {
        if (playerScript != null) {
            if (playerScript.health < currentHealth)
            {
                SetDamage();
                currentHealth = playerScript.health;
            }
            else if (playerScript.health > currentHealth) {
                SetHeal();
                currentHealth = playerScript.health;
            }

            if (takeDamage)
            {
                playerScript.health -= 1;
                takeDamage = false;
            }
            if (heal)
            {
                playerScript.health += 1;
                heal = false;
            }
        }

        if (damagedColor.a > 0) { 
            damagedHealthFadeTimer -= Time.deltaTime;
            if (damagedHealthFadeTimer < 0) {
                float fadeAmount = 5f;
                damagedColor.a -= fadeAmount * Time.deltaTime;
                damagedBar.color = damagedColor;
            }
        }
    }

    private void SetDamage() { 
        //damaged bar is invisible
        if (damagedColor.a <= 0)
        {
            
            damagedBar.fillAmount = healthBar.fillAmount;
            damagedColor.a = 1;
            damagedBar.color = damagedColor;
            damagedHealthFadeTimer = DAMAGED_HEALTH_FADE_TIMER_MAX;
        }
        //damaged bar is already visible
        else {
            damagedColor.a = 1;
            damagedBar.color = damagedColor;
            damagedHealthFadeTimer = DAMAGED_HEALTH_FADE_TIMER_MAX;
        }

        healthBar.fillAmount = Mathf.Clamp((float)playerScript.health / playerScript.maxHealth, 0, 1);
    }

    private void SetHeal()
    {
        healthBar.fillAmount = Mathf.Clamp((float)playerScript.health / playerScript.maxHealth, 0, 1);
    }



}
