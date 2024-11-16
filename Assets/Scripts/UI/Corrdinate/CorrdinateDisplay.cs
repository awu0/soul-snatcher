using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CorrdinateDisplay : MonoBehaviour
{
    public TextMeshProUGUI coordinateText;
    public Grids gridScript;
    public Player player;

    private void Update()
    {
        Vector2 mouseScreenPosition = Input.mousePosition;

        if (gridScript != null && player != null) {
            (int x, int y) gridPosition = gridScript.ConvertToGridPosition(mouseScreenPosition);

            if (gridPosition.x < 0)
            {
                gridPosition.x = 0;
            }
            else if (gridPosition.x > gridScript.rows-1) { 
                gridPosition.x = gridScript.rows-1;
            }
            if (gridPosition.y < 0) {
                gridPosition.y = 0;
            }
            else if(gridPosition.y > gridScript.columns-1)
            {
                gridPosition.y = gridScript.columns-1;
            }

            int stepToX = gridPosition.x - player.GetCurrentPosition().x;
            int stepToY = gridPosition.y - player.GetCurrentPosition().y;

            coordinateText.text = $"{gridPosition.x} ({stepToX}), {gridPosition.y} ({stepToY})";
        }
    }
}
