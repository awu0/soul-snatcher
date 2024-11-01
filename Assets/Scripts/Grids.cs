using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grids : MonoBehaviour
{
    public int rows = 10;
    public int columns = 10;
    public int scale = 1;
    private int playerX;
    private int playerY;
    public GameObject PrefabGrid;
    public GameObject Player;
    public GameObject[,] gridArray;
    public Vector2 leftBottomLocation = new Vector2(0, 0);

    private void Start()
    {
        gridArray = new GameObject[columns, rows];
        GenerateGrid();
        if (Player != null) {
            playerX = Player.GetComponent<PlayerMovement>().startX;
            playerY = Player.GetComponent<PlayerMovement>().startY;
            Player.transform.position = gridArray[playerX, playerY].transform.position;
        }
    }

    void GenerateGrid() { 
        for (int i = 0; i < columns; i++)
        {
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

    public void HandelPlayerMovement(int x, int y) {
        if (Player != null) {
            if(playerX + x >0 && playerX + x < columns) {
                playerX += x;
            }
            if (playerY + y > 0 && playerY + y < rows) {
                playerY += y;
            }
            Player.transform.position = gridArray[playerX, playerY].transform.position;
        }
    }
}
