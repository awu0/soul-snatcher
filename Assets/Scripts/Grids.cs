using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grids : MonoBehaviour
{
    //variable for the grid setting
    public int rows = 10;
    public int columns = 10;
    public int scale = 1;
    
    //variable for handle the player movement
    private int playerX;
    private int playerY;
    
    //prefab of a grid
    public GameObject PrefabGrid;
    
    public Player player;
    
    // 2D array for the grids
    public GameObject[,] gridArray;
    
    // 2D array to determine which cells are occupied
    private Entity[,] entityCells;

    // 2D array to determine which cells have souls. Separate from occupied since souls don't hinder movement
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

        // set start point for player
        if (player != null) {
            playerX = player.startX;
            playerY = player.startY;
            player.transform.position = gridArray[playerX, playerY].transform.position;
            SetCellOccupied(playerX, playerY, player);
        }
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
    
    // Function to change the player's position
    public bool HandlePlayerMovement(int x, int y)
    {
        if (player != null)
        {
            int newX = playerX + x;
            int newY = playerY + y;

            // Check if the new position is within bounds and not occupied
            if (newX >= 0 && newX < columns && newY >= 0 && newY < rows && !IsCellOccupied(newX, newY))
            {
                // Mark the current cell as unoccupied
                SetCellOccupied(playerX, playerY, null);
            
                // Update player's position
                playerX = newX;
                playerY = newY;
                player.transform.position = gridArray[playerX, playerY].transform.position;

                // Check if new cell has a soul in it
                if (DoesCellHaveSoul(playerX, playerY)) {
                  Debug.Log(player);
                  Soul soul = soulCells[playerX, playerY];

                  player.PickUpSoul(soul);

                  soulCells[playerX, playerY] = null;
                  Destroy(soul.gameObject);
                }

                // Mark the new cell as occupied
                SetCellOccupied(playerX, playerY, player);
                return true;
            }
            else
            {
                Debug.Log("Cannot move to the desired position: out of bounds or occupied.");
            }
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
                    gridOutput += entityCells[x, y].gameObject.name[0] + " ";
                }
                else
                {
                    gridOutput += "x "; // Use "." to indicate an empty cell
                }
            }
            Debug.Log(gridOutput);
        }

    }
}
