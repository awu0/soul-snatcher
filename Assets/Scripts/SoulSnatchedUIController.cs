using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SoulSnatchedUIController : MonoBehaviour
{
  public GameObject soulSnatchedUIParent;
  public TextMeshProUGUI soulSnatchTitleText;
  public TextMeshProUGUI soulSnatchEntityValue;

  public TextMeshProUGUI soulSnatchAbilityTitle;
  public TextMeshProUGUI soulSnatchAbilityValue;
  public TextMeshProUGUI soulSnatchMaxHealthTitle;
  public TextMeshProUGUI soulSnatchMaxHealthValue;
  public TextMeshProUGUI soulSnatchAttackTitle;
  public TextMeshProUGUI soulSnatchAttackValue;

  private float displayDuration = 4f;
  private float fadeDuration = 1f;

  private float flashFrequency = 0.5f;
  private Color flashColor1 = Color.white;
  private Color flashColor2 = Color.magenta;

  private Coroutine fadeCoroutine;
  private Coroutine flashCoroutine;

  public void UpdateSoulSnatchedUIText(EntityType previous, EntityType current) {
    string oldType = EntityData.EntityStringMap[previous];
    string currentType = EntityData.EntityStringMap[current];

    EntityBaseStats oldStats = EntityData.EntityBaseStatMap[previous];
    EntityBaseStats currentStats = EntityData.EntityBaseStatMap[current];

    soulSnatchEntityValue.text = $"{oldType} -> {currentType}";
    soulSnatchMaxHealthValue.text = $"{oldStats.MaxHealth} -> {currentStats.MaxHealth}";
    soulSnatchAttackValue.text = $"{oldStats.Attack} -> {currentStats.Attack}";
    soulSnatchAbilityValue.text = $"{oldStats.Ability} -> {currentStats.Ability}";
  }

  public void DisplayAndFadeText() {
    if (fadeCoroutine != null)
        StopCoroutine(fadeCoroutine);
    if (flashCoroutine != null)
        StopCoroutine(flashCoroutine);

    // reset colors
    soulSnatchedUIParent.SetActive(true);
    Color textColor = Color.white;
    textColor.a = 1;

    soulSnatchTitleText.color = textColor;

    soulSnatchEntityValue.color = textColor;
    soulSnatchAbilityValue.color = textColor;
    soulSnatchMaxHealthValue.color = textColor;
    soulSnatchAttackValue.color = textColor;

    soulSnatchAbilityTitle.color = textColor;
    soulSnatchMaxHealthTitle.color = textColor;
    soulSnatchAttackTitle.color = textColor;

    fadeCoroutine = StartCoroutine(FadeSoulSnatchedText());
    flashCoroutine = StartCoroutine(FlashSoulSnatchedText());

  }

  private IEnumerator FadeSoulSnatchedText() {
        yield return new WaitForSeconds(displayDuration);

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            
            Color textColor = Color.white;
            textColor.a = alpha;

            soulSnatchTitleText.color = textColor;

            soulSnatchEntityValue.color = textColor;
            soulSnatchAbilityValue.color = textColor;
            soulSnatchMaxHealthValue.color = textColor;
            soulSnatchAttackValue.color = textColor;

            soulSnatchAbilityTitle.color = textColor;
            soulSnatchMaxHealthTitle.color = textColor;
            soulSnatchAttackTitle.color = textColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        soulSnatchedUIParent.SetActive(false);
    }

  private IEnumerator FlashSoulSnatchedText() {
    float elapsedTime = 0f;
    bool isFirstColor = true;

    while (elapsedTime < displayDuration + fadeDuration) {
      soulSnatchTitleText.color = isFirstColor ? flashColor1 : flashColor2;

      yield return new WaitForSeconds(flashFrequency);

      isFirstColor = !isFirstColor;
      elapsedTime += flashFrequency;
    }

    soulSnatchTitleText.color = flashColor1;
  }
}
