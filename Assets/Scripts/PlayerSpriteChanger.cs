using UnityEngine;
using System.Collections.Generic;

public class PlayerSpriteChanger : MonoBehaviour {
  public Sprite slimeSprite;
  public Sprite giantPillbugSprite;
  public Sprite evilEyeSprite;
  public Sprite snakeSprite;
  public Sprite stoneGolemSprite;


  private Dictionary<EntityType, Sprite> entityToSpriteMap;
  private SpriteRenderer spriteRenderer;

  void Start() {
    spriteRenderer = GetComponent<SpriteRenderer>();

    entityToSpriteMap = new Dictionary<EntityType, Sprite> 
    {
      { EntityType.Slime, slimeSprite },
      { EntityType.GiantPillbug, giantPillbugSprite },
      { EntityType.EvilEye, evilEyeSprite },
      { EntityType.Snake, snakeSprite },
      { EntityType.StoneGolem, stoneGolemSprite }
    };
  }

  public void ChangePlayerSprite(EntityType entityType) {
    if (entityToSpriteMap.TryGetValue(entityType, out Sprite newSprite)) {
      spriteRenderer.sprite = newSprite;
      spriteRenderer.color = Color.blue;
    } else {
      Debug.LogWarning($"No sprite found for entity type {entityType}");
    }
  }
}