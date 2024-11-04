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
    public GameObject Player;
    //2D array for the grids
    public GameObject[,] gridArray;
    
    // 2D array to determine which cells are occupied
    private bool[,] occupiedCells;

    // 2D array to determine which cells have souls. Separate from occupied since souls don't hinder movement
    public Soul[,] soulCells;
    
    //generation point of the grids
    public Vector2 leftBottomLocation = new Vector2(0, 0);

    private void Awake()
    {
        // set the start point
        leftBottomLocation = transform.position;
        gridArray = new GameObject[columns, rows];
        
        occupiedCells = new bool[columns, rows];
        soulCells = new Soul[columns, rows];
        
        // generate initial grids
        GenerateGrid();

        // set start point for player
        if (Player != null) {
            playerX = Player.GetComponent<PlayerMovement>().startX;
            playerY = Player.GetComponent<PlayerMovement>().startY;
            Player.transform.position = gridArray[playerX, playerY].transform.position;
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
    
    public void SetCellOccupied(int x, int y, bool occupied)
    {
        if (x >= 0 && x < columns && y >= 0 && y < rows)
        {
            occupiedCells[x, y] = occupied;
        }
    }
    
    public bool IsCellOccupied(int x, int y)
    {
        if (x < 0 || x >= columns || y < 0 || y >= rows)
        {
            return true; // Treat out-of-bounds as occupied
        }
        return occupiedCells[x, y];
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
        if (Player != null)
        {
            int newX = playerX + x;
            int newY = playerY + y;

            // Check if the new position is within bounds and not occupied
            if (newX >= 0 && newX < columns && newY >= 0 && newY < rows && !IsCellOccupied(newX, newY))
            {
                // Mark the current cell as unoccupied
                SetCellOccupied(playerX, playerY, false);
            
                // Update player's position
                playerX = newX;
                playerY = newY;
                Player.transform.position = gridArray[playerX, playerY].transform.position;

                // Check if new cell has a soul in it
                if (DoesCellHaveSoul(playerX, playerY)) {
                  Player playerClass = Player.GetComponent<Player>();
                  Debug.Log(Player);
                  Soul soul = soulCells[playerX, playerY];

                  playerClass.PickUpSoul(soul);

                  soulCells[playerX, playerY] = null;
                  Destroy(soul.gameObject);
                }

                // Mark the new cell as occupied
                SetCellOccupied(playerX, playerY, true);
                return true;
            }
            else
            {
                Debug.Log("Cannot move to the desired position: out of bounds or occupied.");
            }
        }
        return false;
    }
}
