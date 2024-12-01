using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class HealthText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI healthTextDisplay; // Reference to the health text display
    public Player playerScript;

    private void Awake()
    {
        
    }

    // Show health text when the mouse is over the child object
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (healthTextDisplay != null)
        {
            healthTextDisplay.text = $"{playerScript.health} / {playerScript.maxHealth}";
        }
    }

    // Hide health text when the mouse leaves the child object
    public void OnPointerExit(PointerEventData eventData)
    {
        if (healthTextDisplay != null)
        {
            healthTextDisplay.text = $"{playerScript.health}";
        }
    }
}
