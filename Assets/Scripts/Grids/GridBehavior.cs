using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBehavior : MonoBehaviour
{
    public int rows = 10;
    public int columns = 10;
    public int scale = 1;
    public GameObject gridPrefab;
    public Vector2 leftBottomLocation = new Vector2 (0, 0);

    private void Awake()
    {
        if (gridPrefab)
        {
            GenerateGrid();
        }
        else {
            Debug.Log("Missing gridprefab, please assign.");
        }
    }

    void GenerateGrid() {
        //generating each column
        for (int i = 0; i < columns; i++)
        {
            //generating each row
            for (int j = 0; j < rows; j++) {
                Vector3 position = new Vector3(
                    leftBottomLocation.x + i * scale,
                    leftBottomLocation.y + j * scale,
                    0
                    );
                //generating grid
                GameObject obj = Instantiate(gridPrefab, position, Quaternion.identity);
                obj.transform.SetParent(gameObject.transform);
                obj.GetComponent<GridStat>().x = i;
                obj.GetComponent<GridStat>().y = j;
            }
        }
    }
}
