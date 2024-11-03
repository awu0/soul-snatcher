using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *
 */
public class GiantPillbug : Enemy
{
    private readonly Vector2Int[] _directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
    private int[,] _distanceGrid;
    private List<Vector2Int> _path;
    
    public new void Start()
    {
        base.Start();

        // set the base stats
        SetStats(maxHp: 20, atk: 2);
    }

    protected override void Move()
    {
        if (_path.Count > 1)
        {
            var (x, y) = GetCurrentPosition();
            grids.SetCellOccupied(x, y, false);
            
            Vector2Int nextMove = _path[1]; // The first move towards the target
            transform.position = new Vector3(nextMove.x, nextMove.y, transform.position.z);
            
            grids.SetCellOccupied(nextMove.x, nextMove.y, true);
        }
    }

    protected override void UseAbility()
    {
        var (x, y) = GetCurrentPosition();
        grids.SetCellOccupied(x, y, false);
        
        Vector2Int nextMove = _path[1];
        
        // offset the pill bug by 1 to make sure it is not on top of the player
        int nextMoveX = nextMove.x;
        int nextMoveY = nextMove.y;
        // moving along the y-axis
        if (nextMoveX == x)
        {
            nextMoveY += (y - nextMoveY) / Math.Abs(y - nextMoveY);
        }
        // moving long the x-axis
        else if (nextMoveY == y)
        {
            nextMoveX += (x - nextMoveX) / Math.Abs(x - nextMoveX);
        }
        
        transform.position = new Vector3(nextMoveX, nextMoveY, transform.position.z);
        grids.SetCellOccupied(nextMoveX, nextMoveY, true);
        
        // do damage
        Debug.Log($"{gameObject.name} used ability.");
    }

    /**
     * Attack Range: Unobstructed line of attack towards the player
     * Conditions: Player is in same row or column
     */
    protected override bool AbilityConditionsMet()
    {
        // if the path to the player is 2, that means that the next move will reach the player
        // so use the ability to charge the player
        return _path.Count == 2;
    }

    public override void DetermineNextMove()
    {
        _distanceGrid = GetGridWithDistances();
        _path = GetPathToPlayer(_distanceGrid);
        
        if (AbilityConditionsMet())
        {
            UseAbility();
        }
        else
        {
            Move();
        }
    }
    
   private int[,] GetGridWithDistances()
    {
        int[,] distanceGrid = new int[grids.columns, grids.rows];
        
        // Initialize distances to -1 (unreachable)
        for (int i = 0; i < grids.columns; i++)
        {
            for (int j = 0; j < grids.rows; j++)
            {
                distanceGrid[i, j] = -1;
            }
        }
        
        Vector2Int start = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        distanceGrid[start.x, start.y] = 0;

        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        queue.Enqueue(start);

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();
            int currentDistance = distanceGrid[current.x, current.y];

            // Traverse in each direction until blocked
            foreach (var direction in _directions)
            {
                Vector2Int neighbor = current + direction;
                while (IsPositionValid(neighbor) && distanceGrid[neighbor.x, neighbor.y] == -1)
                {
                    distanceGrid[neighbor.x, neighbor.y] = currentDistance + 1;
                    queue.Enqueue(neighbor);
                    neighbor += direction; // Continue in the same direction
                }
            }
        }
        
        return distanceGrid;
    }

    private bool IsPositionValid(Vector2Int position)
    {
        return position.x >= 0 && position.x < grids.columns &&
               position.y >= 0 && position.y < grids.rows &&
               !grids.IsCellOccupied(position.x, position.y); // Check for obstacles
    }

    private List<Vector2Int> GetPathToPlayer(int[,] distanceGrid)
    {
        var (playerX, playerY) = GetPlayerPosition();
        Vector2Int targetPosition = new Vector2Int(playerX, playerY);
        List<Vector2Int> path = new List<Vector2Int>();

        Vector2Int currentPosition = targetPosition;

        while (distanceGrid[currentPosition.x, currentPosition.y] != 0)
        {
            path.Add(currentPosition);

            Vector2Int nextStep = Vector2Int.zero;
            int minDistance = int.MaxValue;

            foreach (var direction in _directions)
            {
                Vector2Int neighbor = currentPosition + direction;
                while (IsWithinBounds(neighbor) && distanceGrid[neighbor.x, neighbor.y] != -1)
                {
                    int neighborDistance = distanceGrid[neighbor.x, neighbor.y];
                    if (neighborDistance < minDistance)
                    {
                        minDistance = neighborDistance;
                        nextStep = neighbor;
                    }
                    neighbor += direction;
                }
            }

            if (nextStep == Vector2Int.zero)
            {
                Debug.LogWarning("No valid next step found.");
                break;
            }

            currentPosition = nextStep;
        }

        path.Add(new Vector2Int((int)transform.position.x, (int)transform.position.y));
        path.Reverse();
        return path;
    }

    private bool IsWithinBounds(Vector2Int position)
    {
        return position.x >= 0 && position.x < grids.columns && 
               position.y >= 0 && position.y < grids.rows;
    }
    
    private void PrintGrid(int[,] grid)
    {
        Debug.Log("Distance grid:");
        for (int j = grids.rows - 1; j >= 0; j--)
        {
            string line = "";
            for (int i = 0; i < grids.columns; i++)
            {
                line += grid[i, j] + "\t";
            }
            Debug.Log(line);
        }
    }
}