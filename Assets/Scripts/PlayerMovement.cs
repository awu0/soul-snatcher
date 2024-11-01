using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public int startX = 0;
    public int startY = 0;
    public int locX;
    public int locY;
    public GameObject grids;

    private void Start()
    {
        locX = startX;
        locY = startY;

    }

    private void Update()
    {
        //move up
        if (Input.GetKeyDown(KeyCode.W))
        {
            locY += 1;
            grids.GetComponent<Grids>().HandelPlayerMovement(0, 1);
        }
        //move down
        else if (Input.GetKeyDown(KeyCode.S))
        {
            locY -= 1;
            grids.GetComponent<Grids>().HandelPlayerMovement(0, -1);
        }
        //move left
        else if (Input.GetKeyDown(KeyCode.A))
        {
            locX -= 1;
            grids.GetComponent<Grids>().HandelPlayerMovement(-1, 0);
        }
        //move right
        else if (Input.GetKeyDown(KeyCode.D))
        {
            locX += 1;
            grids.GetComponent<Grids>().HandelPlayerMovement(1, 0);
        }
    }
}
