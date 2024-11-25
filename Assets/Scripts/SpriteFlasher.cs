using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlasher : MonoBehaviour {
  [SerializeField] private Color transformFlashColor = Color.white;
  [SerializeField] private Color damageFlashColor = Color.red;
  [SerializeField] private float flashTime = 0.2f;
  [SerializeField] private int numFlashes = 2;

  private SpriteRenderer spriteRenderer;
  private Material material;

  private Coroutine spriteFlasherCoroutine;

  private void Awake() {
    spriteRenderer = GetComponent<SpriteRenderer>();
    material = spriteRenderer.material;
  }

  public void CallTransformSpriteFlasher() {
    spriteFlasherCoroutine = StartCoroutine(SpriteFlasherCoroutine(transformFlashColor));
  }

  public void CallDamageSpriteFlasher() {
    spriteFlasherCoroutine = StartCoroutine(SpriteFlasherCoroutine(damageFlashColor));
  }

  private IEnumerator SpriteFlasherCoroutine(Color flashColor) {
    // set color
    material.SetColor("_FlashColor", flashColor);

    for (int i = 0; i < numFlashes; i++) {
      material.SetFloat("_FlashAmount", 1f);
      yield return new WaitForSeconds(flashTime / 2f);
      material.SetFloat("_FlashAmount", 0f);
      yield return new WaitForSeconds(flashTime / 2f);
    }
  }
}