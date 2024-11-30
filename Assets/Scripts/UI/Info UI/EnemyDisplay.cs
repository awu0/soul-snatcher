using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;

public class EnemyDisplay : MonoBehaviour 
{
    public GameManager gameManager;
    public Transform enemyPreviewParent; // A parent transform for organizing preview sprites

    public Sprite meleeSprite;
    public Sprite snakeSprite;
    public Sprite evilEyeSprite;
    public Sprite pillbugSprite;
    public Sprite stoneGolemSprite;

    public void DisplayEnemies()
    {
        // Clear any previous previews
        foreach (Transform child in enemyPreviewParent)
        {
            Destroy(child.gameObject);
        }

        // Loop through enemiesToSpawn and create previews using the corresponding sprites
        foreach (var enemy in gameManager.enemiesToSpawn)
        {
            // Determine the appropriate sprite based on the enemy type
            Sprite enemySprite = GetSpriteForEnemy(enemy);

            if (enemySprite != null)
            {
                // Create a new GameObject to display the sprite
                GameObject enemyPreview = new GameObject("EnemyPreview");

                // Attach it to the parent
                enemyPreview.transform.SetParent(enemyPreviewParent);

                // Add a SpriteRenderer and assign the sprite
                SpriteRenderer spriteRenderer = enemyPreview.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = enemySprite;

                // Optionally adjust position or scale
                enemyPreview.transform.localPosition = Vector3.zero; // Adjust as needed
                enemyPreview.transform.localScale = Vector3.one;     // Uniform scale
            }
            else
            {
                Debug.LogWarning($"No sprite found for enemy type: {enemy}");
            }
        }
    }

    // Helper method to get the appropriate sprite for a given enemy
    private Sprite GetSpriteForEnemy(object enemy)
    {
        switch (enemy.ToString()) // Assumes enemy is a string or enum
        {
            case "Melee":
                return meleeSprite;
            case "Snake":
                return snakeSprite;
            case "EvilEye":
                return evilEyeSprite;
            case "Pillbug":
                return pillbugSprite;
            case "StoneGolem":
                return stoneGolemSprite;
            default:
                return null;
        }
    }
}