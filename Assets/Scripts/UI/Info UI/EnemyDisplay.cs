using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDisplay : MonoBehaviour
{
    public GameManager gameManager;
    public Transform enemyPreviewParent; // The box holding the enemy preview
    public Button toggleVisibilityButton;

    public Sprite hookerSprite;
    public Sprite snakeSprite;
    public Sprite evilEyeSprite;
    public Sprite giantPillbugSprite;
    public Sprite stoneGolemSprite;

    private Dictionary<Type, Sprite> entityToSpriteMap;

    private List<GameObject> createdPreviews = new List<GameObject>();
    private bool arePreviewsVisible = true;

    public void Start()
    {
        entityToSpriteMap = new Dictionary<Type, Sprite>
        {
            { typeof(EvilEye), evilEyeSprite },
            { typeof(GiantPillbug), giantPillbugSprite },
            { typeof(StoneGolem), stoneGolemSprite },
            { typeof(Snake), snakeSprite },
            { typeof(Hooker), hookerSprite }
        };

        // Attach toggle functionality to the button
        if (toggleVisibilityButton != null)
        {
            toggleVisibilityButton.onClick.AddListener(ToggleVisibility);
        }
        else
        {
            Debug.LogWarning("Toggle Visibility Button is not assigned!");
        }
    }

    public void DisplayEnemies()
    {
        ClearEnemyPreviews();
        float yOffset = enemyPreviewParent.transform.localPosition.y;

        foreach (var entityType in gameManager.enemiesToSpawn)
        {
            if (entityToSpriteMap.TryGetValue(entityType, out Sprite newSprite))
            {
                GameObject enemyPreviewParentInstance = Instantiate(enemyPreviewParent.gameObject, enemyPreviewParent.parent); //create instance of the box
                enemyPreviewParentInstance.name = $"EnemyPreviewParent_{entityType.Name}";

                RectTransform parentRectTransform = enemyPreviewParentInstance.GetComponent<RectTransform>();
                parentRectTransform.anchoredPosition = new Vector2(0, yOffset);

                GameObject enemyPreview = new GameObject("EnemyPreview", typeof(RectTransform), typeof(Image)); //the enemy preview
                enemyPreview.transform.SetParent(enemyPreviewParentInstance.transform, false);
                
                Image imageComponent = enemyPreview.GetComponent<Image>(); //the image for the enemy
                imageComponent.sprite = newSprite;

                Color color = imageComponent.color;
                color.a = 0.8f; // Make enemy sprite a bit transparent
                imageComponent.color = color;

                RectTransform rectTransform = enemyPreview.GetComponent<RectTransform>();
                rectTransform.localScale = Vector3.one * 0.5f;

                createdPreviews.Add(enemyPreviewParentInstance);

                yOffset -= 50;
            }
            else
            {
                Debug.LogWarning($"No sprite found for enemy type: {entityType}");
            }
        }
        Image parentImage = enemyPreviewParent.GetComponent<Image>();
        if (parentImage != null)
        {
            parentImage.enabled = false;
        }
    }

    public void ClearEnemyPreviews()
    {
        foreach (var preview in createdPreviews)
        {
            Destroy(preview);
        }
        createdPreviews.Clear();
        Debug.Log("Cleared all enemy previews.");

        Image parentImage = enemyPreviewParent.GetComponent<Image>();
        if (parentImage != null)
        {
            parentImage.enabled = true;
        }
    }
    public void ToggleVisibility()
    {
        arePreviewsVisible = !arePreviewsVisible;

        foreach (var preview in createdPreviews)
        {
            preview.SetActive(arePreviewsVisible); // Show or hide each preview
        }
    }
}
