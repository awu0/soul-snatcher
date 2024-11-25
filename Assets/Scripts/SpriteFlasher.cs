using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlasher : MonoBehaviour {
  private SpriteRenderer spriteRenderer;
  private Material material;

  private Coroutine spriteFlasherCoroutine;

  private void Awake() {
    spriteRenderer = GetComponent<SpriteRenderer>();
    material = spriteRenderer.material;
  }

  public void CallTransformSpriteFlasher() {
    spriteFlasherCoroutine = StartCoroutine(SpriteFlasherCoroutine(Color.white, 3, .25f));
  }

  public void CallDamageSpriteFlasher() {
    spriteFlasherCoroutine = StartCoroutine(SpriteFlasherCoroutine(Color.red, 2, .2f));
  }

  public void CallGuardSpriteTint(bool isGuarding) {
    if (isGuarding) {
      material.SetColor("_FlashColor", Color.blue);
      material.SetFloat("_FlashAmount", 0.75f);
    } else {
      material.SetFloat("_FlashAmount", 0f);
    }
  }

  public void CallRemoveSpriteTint() {
    material.SetFloat("_FlashAmount", 0f);
  }

  private IEnumerator SpriteFlasherCoroutine(Color flashColor, int numFlashes, float flashTime) {
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