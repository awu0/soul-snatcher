using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlasher : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Material material;

    private Dictionary<string, Color> activeTints = new Dictionary<string, Color>();
    private Coroutine spriteFlasherCoroutine;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        material = spriteRenderer.material;
    }

    // Flash white color
    public void CallTransformSpriteFlasher()
    {
        spriteFlasherCoroutine = StartCoroutine(SpriteFlasherCoroutine("TransformFlash", Color.white, 3, .25f));
    }

    // Flash red color for damage
    public void CallDamageSpriteFlasher()
    {
        spriteFlasherCoroutine = StartCoroutine(SpriteFlasherCoroutine("DamageFlash", Color.red, 2, .2f));
    }

    // Add a blue tint for guarding
    public void CallGuardSpriteTint(bool isGuarding)
    {
        if (isGuarding)
        {
            AddSpriteTint("Guard", Color.blue);
        }
        else
        {
            RemoveSpriteTint("Guard");
        }
    }

    // Add a red tint for enrage
    public void CallEnrageSpriteTint(bool isEnraged)
    {
        if (isEnraged)
        {
            AddSpriteTint("Enrage", Color.red);
        }
        else
        {
            RemoveSpriteTint("Enrage");
        }
    }

    // Remove all tints
    public void CallRemoveSpriteTint()
    {
        activeTints.Clear();
        UpdateCombinedTint();
    }

    // Add a specific tint
    public void AddSpriteTint(string tintKey, Color tintColor)
    {
        if (!activeTints.ContainsKey(tintKey))
        {
            activeTints[tintKey] = tintColor;
        }
        UpdateCombinedTint();
    }

    // Remove a specific tint
    public void RemoveSpriteTint(string tintKey)
    {
        if (activeTints.ContainsKey(tintKey))
        {
            activeTints.Remove(tintKey);
        }
        UpdateCombinedTint();
    }

    // Update the combined tint color
    private void UpdateCombinedTint()
    {
        if (activeTints.Count == 0)
        {
            material.SetFloat("_FlashAmount", 0f);
            return;
        }

        // Combine all active tints
        Color combinedColor = Color.black;
        foreach (var tint in activeTints.Values)
        {
            combinedColor += tint;
        }
        combinedColor /= activeTints.Count;

        material.SetColor("_FlashColor", combinedColor);
        material.SetFloat("_FlashAmount", 0.75f); 
    }

    // Coroutine to flash the sprite
    private IEnumerator SpriteFlasherCoroutine(string flashKey, Color flashColor, int numFlashes, float flashTime)
    {
        AddSpriteTint(flashKey, flashColor);

        for (int i = 0; i < numFlashes; i++)
        {
            material.SetFloat("_FlashAmount", 1f);
            yield return new WaitForSeconds(flashTime / 2f);
            material.SetFloat("_FlashAmount", 0f);
            yield return new WaitForSeconds(flashTime / 2f);
        }

        RemoveSpriteTint(flashKey);
    }
}
