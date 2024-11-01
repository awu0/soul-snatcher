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
    //generation point of the grids
    public Vector2 leftBottomLocation = new Vector2(0, 0);

    private void Start()
    {
        //set the start point
        leftBottomLocation = transform.position;
        gridArray = new GameObject[columns, rows];
        //generate initial grids
        GenerateGrid();

        //set start point for player
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

    //function to change the player's position
    public void HandelPlayerMovement(int x, int y) {
        if (Player != null) {
            if(playerX + x >= 0 && playerX + x < columns) {
                playerX += x;
            }
            if (playerY + y >= 0 && playerY + y < rows) {
                playerY += y;
            }
            Player.transform.position = gridArray[playerX, playerY].transform.position;
        }
    }
}
