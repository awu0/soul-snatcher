using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ModeSelectionUI : MonoBehaviour
{
    public TextMeshProUGUI modeText;
    public Player player;

    public Image meleeImage;
    public Image abilityImage;
    public Image transformImage;

    // ability sprites
    public Sprite meleeSprite;
    public Sprite snakeSprite;
    public Sprite evilEyeSprite;
    public Sprite pillbugSprite;
    public Sprite stoneGolemSprite;

    // recent enemy transform sprites
    public Sprite slimeSprite;
    public Sprite slimeSnakeSprite;
    public Sprite slimeGolemSprite;
    public Sprite slimePillbugSprite;
    public Sprite slimeEyeSprite;


    public Color inactiveColor = new Color(63f / 255f, 28f / 255f, 28f / 255f, 1f);
    public Color activeColor = new Color(255f / 255f, 174f / 255f, 0f / 255f, 1f);

    public UnityEngine.UI.Image attackSlot;
    public UnityEngine.UI.Image abilitySlotA;
    public UnityEngine.UI.Image previousTransformSlot;

    private UnityEngine.UI.Image selectedSlot;
    private void Start()
    {
        selectedSlot = attackSlot;
        UpdateSlotColor(selectedSlot, activeColor);
    }

    private void Update()
    {
        if (player == null || modeText == null) return;

        UnityEngine.UI.Image currentSlot = null;

        switch (player.selectedAction)
        {
            case Player.SELECTED.ATTACK:
                modeText.text = "Mode: Attack";
                currentSlot = attackSlot;
                break;

            case Player.SELECTED.ABILITY:
                modeText.text = "Mode: Ability";
                currentSlot = abilitySlotA;
                break;

            case Player.SELECTED.RECENT_TRANSFORM:
                modeText.text = "Mode: Transform";
                currentSlot = previousTransformSlot;
                break;
        }


        if (currentSlot != selectedSlot)
        {
            UpdateSlotColor(selectedSlot, inactiveColor);
            UpdateSlotColor(currentSlot, activeColor);
            selectedSlot = currentSlot;
        }

        // set ability image
        if (player.abilities.ToArray().Length < 1) {
            ChangeAbilitySprite(null);
            abilityImage.enabled = false;
        }
        else if (player.abilities.Peek().GetType().Name == "SnakeBite")
        {
            ChangeAbilitySprite(snakeSprite);
            abilityImage.enabled = true;
        }
        else if (player.abilities.Peek().GetType().Name == "PillbugRoll")
        {
            ChangeAbilitySprite(pillbugSprite);
            abilityImage.enabled = true;
        }
        else if (player.abilities.Peek().GetType().Name == "EyeLaser")
        {
            ChangeAbilitySprite(evilEyeSprite);
            abilityImage.enabled = true;
        }
        else if (player.abilities.Peek().GetType().Name == "Guard")
        {
            ChangeAbilitySprite(stoneGolemSprite);
            abilityImage.enabled = true;
        }

        // set recent transform image
        if (player.previousEntityType == null) {
            ChangePreviousTransformSprite(null);
            transformImage.enabled = false;
        }
        else if (player.previousEntityType == EntityType.Slime) {
            ChangePreviousTransformSprite(slimeSprite);
            transformImage.enabled = true;
        }
        else if (player.previousEntityType == EntityType.Snake)
        {
            ChangePreviousTransformSprite(slimeSnakeSprite);
            transformImage.enabled = true;
        }
        else if (player.previousEntityType == EntityType.GiantPillbug)
        {
            ChangePreviousTransformSprite(slimePillbugSprite);
            transformImage.enabled = true;
        }
        else if (player.previousEntityType == EntityType.EvilEye)
        {
            ChangePreviousTransformSprite(slimeEyeSprite);
            transformImage.enabled = true;
        }
        else if (player.previousEntityType == EntityType.StoneGolem)
        {
            ChangePreviousTransformSprite(slimeGolemSprite);
            transformImage.enabled = true;
        }
    }

    void UpdateSlotColor(UnityEngine.UI.Image slot, Color color)
    {
        if (slot != null)
        {
            slot.color = color;
        }
    }

    public void ChangeAbilitySprite(Sprite sprite) {
        if (sprite == null) {
          abilityImage.sprite = null;
        }

        if (abilityImage != null) { 
            abilityImage.sprite = sprite;
        }
    }

    public void ChangePreviousTransformSprite(Sprite sprite) {
      if (sprite == null) {
        transformImage.sprite = null;
      }

      if (transformImage != null) {
        transformImage.sprite = sprite;
      }
    }
}