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
    public GameObject PrefabGrid2;
    public GameObject PrefabGrid3;

    // 2D array for the grids
    public GameObject[,] gridArray;
    
    // 2D array to determine which cells are occupied, contains the reference to Entities
    private Entity[,] entityCells;

    // 2D array to handle where the walls are. false if no wall, true if wall
    private bool[,] wallCells;

    // 2D array to determine which cells have souls. Separate from entityCells since souls don't hinder movement
    public Soul[,] soulCells;
    
    //generation point of the grids
    public Vector2 leftBottomLocation = new Vector2(0, 0);

    private void Awake()
    {
        // set the start point
        leftBottomLocation = transform.position;
        // gridArray = new GameObject[columns, rows];
        
        // entityCells = new Entity[columns, rows];
        // wallCells = new bool[columns, rows];
        // soulCells = new Soul[columns, rows];
        
        // generate initial grids
        GenerateGrid();
    }

    //Function for generating the grids, use the columns and rows and generate equal amount of grid prefab 
    public void GenerateGrid() { 
        gridArray = new GameObject[columns, rows];
        
        entityCells = new Entity[columns, rows];
        wallCells = new bool[columns, rows];
        soulCells = new Soul[columns, rows];
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
                GameObject chosenPrefab;
                float randomValue = Random.value;
                if (randomValue < 0.33f)
                {
                    chosenPrefab = PrefabGrid;
                }
                else if (randomValue < 0.66f)
                {
                    chosenPrefab = PrefabGrid2;
                }
                else
                {
                    chosenPrefab = PrefabGrid3;
                }

                // Randomly rotate the object in increments of 90 degrees
                Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(0, 4) * 90);

                GameObject obj = Instantiate(chosenPrefab, position, rotation);
                obj.transform.SetParent(gameObject.transform);
                gridArray[i, j] = obj;
            }
        }
    }

    /// <summary>
    /// Determines if (x, y) is within the map. Also returns false if the position is a wall.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public bool IsPositionWithinBounds(int x, int y)
    {
        return x >= 0 && x < columns && y >= 0 && y < rows && !IsWall(x, y);
    }

    public Entity GetEntityAt(int x, int y)
    {
        if (!IsPositionWithinBounds(x, y)) return null;
        
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
        
        if (!IsPositionWithinBounds(pos.x, pos.y))
        {
            Debug.Log($"MousePosition: {pos} is not on the grid.");
            return null;
        }
        
        return GetEntityAt(pos.x, pos.y);
    }
    
    public bool IsWall(int x, int y)
    {
        return wallCells[x, y];
    }

    public void SetWall(int x, int y, bool isWall)
    {
        wallCells[x, y] = isWall;
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

    public Vector2Int GetDirectionFromMouse(Vector2Int playerPos, Vector2 mousePos) {
      (int x, int y) mouseGridPos = ConvertToGridPosition(mousePos);

      int xDiff = mouseGridPos.x - playerPos.x;
      int yDiff = mouseGridPos.y - playerPos.y;

      if (xDiff == 0 && yDiff > 0) {
        return Vector2Int.up;
      } else if (xDiff == 0 && yDiff < 0) {
        return Vector2Int.down;
      } else if (xDiff > 0 && yDiff == 0) {
        return Vector2Int.right;
      } else if (xDiff < 0 && yDiff == 0) {
        return Vector2Int.left;
      } else {
        return Vector2Int.zero;
      }
    }

    public List<Vector2Int> GetAllValidPositions(bool[] map, int width, int height)
    {   
        List<Vector2Int> validPositions = new List<Vector2Int>();

        for (int y = 1; y < height - 1; y++)
        {
            for (int x = 1; x < width - 1; x++)
            {
                if (!map[x + y * width] && !IsCellOccupied(x, y)) 
                {   
                    validPositions.Add(new Vector2Int(x, y));
                }
            }
        }

        return validPositions;
    }

    public Vector2Int RandomValidSpawnPosition(bool[] map, int width, int height)
    {
        List<Vector2Int> validPositions = GetAllValidPositions(map, width, height);

        if (validPositions.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, validPositions.Count);
            return validPositions[randomIndex];
        }

        return new Vector2Int(1, 1);
    }

    public Vector2Int FindRandomDistantPosition(bool[] map, int width, int height, Vector2Int origin, int minDistance)
    {
        List<Vector2Int> validPositions = GetAllValidPositions(map, width, height);
        List<Vector2Int> distantPositions = new List<Vector2Int>();

        // Filter positions to ensure they are at least `minDistance` away from the origin
        foreach (Vector2Int pos in validPositions)
        {
            if (Vector2Int.Distance(pos, origin) >= minDistance)
            {
                distantPositions.Add(pos);
            }
        }

        // Return a random distant position, or Vector2Int.zero if none found
        return distantPositions.Count > 0 ? distantPositions[UnityEngine.Random.Range(0, distantPositions.Count)] : validPositions[UnityEngine.Random.Range(0, validPositions.Count)];
    }

    public void DeleteGridPrefabs()
    {
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                if (gridArray[i, j] != null)
                {
                    Destroy(gridArray[i, j]); // Delete the prefab from the scene
                    gridArray[i, j] = null; // Clear the reference from the array
                }
                entityCells[i, j] = null; // Clear the reference from the array
                wallCells[i, j] = false;
            }
        }
    }
}
