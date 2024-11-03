using System;
using System.Collections.Generic;
using UnityEngine;

/**
 * This enemy has a movement type of finding the shortest path to the player. Moves 1 tile per turn.
 */
public abstract class ChargingEnemyType : Enemy
{
    private readonly Vector2Int[] _directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

    protected override void Move()
    {
        var distanceGrid = GetGridWithDistances();
        var path = GetPathToPlayer(distanceGrid); // Get the path to the player

        if (path.Count > 1)
        {
            var (x, y) = GetCurrentPosition();
            grids.SetCellOccupied(x, y, false);
            
            Vector2Int nextMove = path[1]; // The first move towards the target
            transform.position = new Vector3(nextMove.x, nextMove.y, transform.position.z);
            
            grids.SetCellOccupied(nextMove.x, nextMove.y, true);
        }
    }

    /**
     * Returns a 2d grid with how many turns it will take to get to every spot on the map.
     * -1 means unreachable
     */
    private int[,] GetGridWithDistances()
    {
        int[,] distanceGrid = new int[grids.columns, grids.rows];
        
        // Initialize distances to -1 (unreachable)
        for (int i = 0; i < grids.columns; i++)
        {
            for (int j = 0; j < grids.rows; j++)
            {
                distanceGrid[i, j] = -1; // -1 means unvisited
            }
        }
        
        Vector2Int position = new Vector2Int((int)transform.position.x, (int)transform.position.y);

        // Set the starting position distance to 0
        distanceGrid[position.x, position.y] = 0;

        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        queue.Enqueue(position);

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();
            int currentDistance = distanceGrid[current.x, current.y];

            foreach (var direction in _directions)
            {
                Vector2Int neighbor = current + direction;

                if (IsPositionValid(neighbor) && distanceGrid[neighbor.x, neighbor.y] == -1)
                {
                    distanceGrid[neighbor.x, neighbor.y] = currentDistance + 1;
                    queue.Enqueue(neighbor);
                }
            }
        }
        
        // PrintGrid(distanceGrid);
        return distanceGrid;
    }

    private bool IsPositionValid(Vector2Int position)
    {
        // Check bounds and whether it's an obstacle
        return position.x >= 0 && position.x < grids.columns &&
               position.y >= 0 && position.y < grids.rows &&
               !grids.IsCellOccupied(position.x, position.y); // Check for obstacles
    }
    
    /**
     * Returns a List of coordinates that depicts the steps to take to get to the player.
     * The first item is the current position
     */
    private List<Vector2Int> GetPathToPlayer(int[,] distanceGrid)
    {
        var (playerX, playerY) = GetPlayerPosition();
        Vector2Int targetPosition = new Vector2Int(playerX, playerY);
    
        List<Vector2Int> path = new List<Vector2Int>();

        // Start from the target position
        Vector2Int currentPosition = targetPosition;

        // While we haven't reached the starting position
        while (distanceGrid[currentPosition.x, currentPosition.y] != 0)
        {
            path.Add(currentPosition);

            Vector2Int nextStep = Vector2Int.zero;
            int minDistance = int.MaxValue;

            // Check all four possible directions
            foreach (var direction in _directions)
            {
                Vector2Int neighbor = currentPosition + direction;

                // Ensure we don't go out of bounds
                if (neighbor.x >= 0 && neighbor.x < grids.columns && 
                    neighbor.y >= 0 && neighbor.y < grids.rows)
                {
                    int neighborDistance = distanceGrid[neighbor.x, neighbor.y];

                    // Only consider valid neighbors (distance not -1)
                    if (neighborDistance >= 0 && neighborDistance < minDistance)
                    {
                        minDistance = neighborDistance;
                        nextStep = neighbor;
                    }
                }
            }

            // If no next step is found, break (should not happen if the grid is correctly filled)
            if (nextStep == Vector2Int.zero)
            {
                Debug.LogWarning("No valid next step found, ending path tracing.");
                break;
            }

            currentPosition = nextStep;
        }

        // Add the starting position to the path (the enemy's current position)
        path.Add(new Vector2Int((int)transform.position.x, (int)transform.position.y));

        // Reverse the path to start from the initial position
        path.Reverse();
        return path;
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
