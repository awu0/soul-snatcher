using UnityEngine;
using System.Collections.Generic;

public class PlayerSpriteChanger : MonoBehaviour {
  public Sprite slimeSprite;
  public Sprite giantPillbugSprite;
  public Sprite evilEyeSprite;
  public Sprite snakeSprite;
  public Sprite stoneGolemSprite;


  private Dictionary<EntityType, Sprite> entityToSpriteMap;
  private Dictionary<EntityType, string> entityToAnimMap;
  private SpriteRenderer spriteRenderer;
  public Animator animator;

    void Start() {
    spriteRenderer = GetComponent<SpriteRenderer>();
    animator = GetComponent<Animator>();
    entityToSpriteMap = new Dictionary<EntityType, Sprite> 
    {
      { EntityType.Slime, slimeSprite },
      { EntityType.GiantPillbug, giantPillbugSprite },
      { EntityType.EvilEye, evilEyeSprite },
      { EntityType.Snake, snakeSprite },
      { EntityType.StoneGolem, stoneGolemSprite }
    };

    entityToAnimMap = new Dictionary<EntityType, string>
    {
      { EntityType.Slime, "isSlime" },
      { EntityType.GiantPillbug, "isBug" },
      { EntityType.EvilEye, "isEye" },
      { EntityType.Snake, "isSnake" },
      { EntityType.StoneGolem, "isGolem" }
    };
    }

  public void ChangePlayerSprite(EntityType entityType) {
        animator.SetBool("isBug", false);
        animator.SetBool("isEye", false);
        animator.SetBool("isGolem", false);
        animator.SetBool("isSnake", false);

        if (entityToSpriteMap.TryGetValue(entityType, out Sprite newSprite) && entityToAnimMap.TryGetValue(entityType, out string newAnim)) {
          spriteRenderer.sprite = newSprite;
          animator.SetBool(newAnim, true);
          spriteRenderer.color = Color.blue;
        } else {
          Debug.LogWarning($"No sprite found for entity type {entityType}");
        }
      }
}