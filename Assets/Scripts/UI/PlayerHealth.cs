using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Player playerScript;
    public Image healthBar;
    public bool takeDamage;

    private void Start()
    {
        playerScript = FindObjectOfType<Player>();
    }

    private void Update()
    {
        if (playerScript != null)
        {
            healthBar.fillAmount = Mathf.Clamp((float)playerScript.health / playerScript.maxHealth, 0, 1);
        }
        else {
            healthBar.fillAmount = 0;
        }

        if(takeDamage) {
            playerScript.health -= 1;
            takeDamage = false;
        }

        //Debug.Log(playerScript.health);
    }

    

}
