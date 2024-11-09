using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class HealthText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI healthTextDisplay; // Reference to the health text display

    private void Awake()
    {
        // Hide the health text initially
        if (healthTextDisplay != null)
        {
            healthTextDisplay.gameObject.SetActive(false);
        }
    }

    // Show health text when the mouse is over the child object
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (healthTextDisplay != null)
        {
            healthTextDisplay.gameObject.SetActive(true);
        }
    }

    // Hide health text when the mouse leaves the child object
    public void OnPointerExit(PointerEventData eventData)
    {
        if (healthTextDisplay != null)
        {
            healthTextDisplay.gameObject.SetActive(false);
        }
    }
}
