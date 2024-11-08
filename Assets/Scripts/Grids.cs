using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grids : MonoBehaviour
{
    //variable for the grid setting
    public int rows = 10;
    public int columns = 10;
    public int scale = 1;
    
    //prefab of a grid
    public GameObject PrefabGrid;
    
    // 2D array for the grids
    public GameObject[,] gridArray;
    
    // 2D array to determine which cells are occupied, contains the reference to Entities
    private Entity[,] entityCells;

    // 2D array to determine which cells have souls. Separate from entityCells since souls don't hinder movement
    public Soul[,] soulCells;
    
    //generation point of the grids
    public Vector2 leftBottomLocation = new Vector2(0, 0);

    private void Awake()
    {
        // set the start point
        leftBottomLocation = transform.position;
        gridArray = new GameObject[columns, rows];
        
        entityCells = new Entity[columns, rows];
        soulCells = new Soul[columns, rows];
        
        // generate initial grids
        GenerateGrid();
    }

    //Function for generating the grids, use the columns and rows and generate equal amount of grid prefab 
    void GenerateGrid() { 
        //columns
        for (int i = 0; i < columns; i++)
        {
            //rows
            for (int j = 0; j < rows; j++) {
                Vector3 position = new Vector3(
                    leftBottomLocation.x + i * scale,
                    leftBottomLocation.y + j * scale,
                    0
                    );

                GameObject obj = Instantiate(PrefabGrid, position, Quaternion.identity);
                obj.transform.SetParent(gameObject.transform);
                gridArray[i, j] = obj;
            }
        }
    }

    public bool IsPositionWithinBounds(int x, int y)
    {
        return x >= 0 && x < columns && y >= 0 && y < rows;
    }

    public Entity GetEntityAt(int x, int y)
    {
        return entityCells[x, y];
    }
    
    public void SetCellOccupied(int x, int y, Entity entity)
    {
        if (x >= 0 && x < columns && y >= 0 && y < rows)
        {
            entityCells[x, y] = entity;
        }
    }
    
    public bool IsCellOccupied(int x, int y)
    {
        return IsPositionWithinBounds(x, y) && entityCells[x, y] != null;
    }

    public Entity GetEntityAtMouse(Vector2 mousePos)
    {
        (int x, int y) pos = ConvertToGridPosition(mousePos);
        Debug.Log($"MousePosition: {pos}");
        
        return GetEntityAt(pos.x, pos.y);
    }
    
    public bool DoesCellHaveSoul(int x, int y)
    {
      return soulCells[x, y] != null;
    }

    public bool SetCellSoul(int x, int y, Soul soul)
    {
      if (x >= 0 && x < columns && y >= 0 && y < rows)
      {
          soulCells[x, y] = soul;
          return true;
      }
      return false;
    }
    
    public void PrintEntityGrid()
    {
        Debug.Log("Entity Grid:");
    
        // Iterate over rows from top to bottom for a more natural display
        for (int y = rows - 1; y >= 0; y--)
        {
            string gridOutput = "";
            for (int x = 0; x < columns; x++)
            {
                if (entityCells[x, y] != null)
                {
                    // Shorten to first character or name for compactness
                    gridOutput += entityCells[x, y].gameObject.name[0] + "\t";
                }
                else
                {
                    gridOutput += "x\t"; // Use "x" to indicate an empty cell
                }
            }
            Debug.Log(gridOutput);
        }

    }

    public (int x, int y) ConvertToGridPosition(Vector2 screenPosition) // Converts screen position to grid position
    {
    // Convert screen position to world position
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, Camera.main.nearClipPlane));
        int gridX = Mathf.RoundToInt(worldPosition.x / scale);
        int gridY = Mathf.RoundToInt(worldPosition.y / scale);

        return (gridX, gridY);
    }
}
