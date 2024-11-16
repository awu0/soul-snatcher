using Microsoft.Unity.VisualStudio.Editor;
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

    public Color inactiveColor = new Color(63f / 255f, 28f / 255f, 28f / 255f, 1f);
    public Color activeColor = new Color(255f / 255f, 174f / 255f, 0f / 255f, 1f);

    public UnityEngine.UI.Image attackSlot;
    public UnityEngine.UI.Image abilitySlotA;

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

        switch (player.selectedAction) {
            case Player.SELECTED.ATTACK:
                modeText.text = "Mode: Attack";
                currentSlot = attackSlot;
                break;

            case Player.SELECTED.ABILITY:
                modeText.text = "Mode: Ability";
                currentSlot = abilitySlotA;
                break;
        }

        if (currentSlot != selectedSlot) {
            UpdateSlotColor(selectedSlot, inactiveColor);
            UpdateSlotColor(currentSlot, activeColor);
            selectedSlot = currentSlot;
        }
    }

    void UpdateSlotColor(UnityEngine.UI.Image slot, Color color) {
        if (slot != null) {
            slot.color = color;
        }
    }
}