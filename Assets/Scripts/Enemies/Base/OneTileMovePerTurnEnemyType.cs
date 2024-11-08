using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemies that inherit this class will have the GetGridWithDistances() method.
/// This grid will contain how many moves it will take to reach every point on the grid.
/// This can be used to calculate certain moves the enemies can make.
/// </summary>
public abstract class OneTileMovePerTurnEnemyType : Enemy
{
    
    /// <summary>
    /// Returns a 2d grid with how many turns it will take to get to every spot on the map.
    /// -1 means unreachable
    /// </summary>
    /// <returns></returns>
    protected int[,] GetGridWithDistances(Vector2Int startingPosition)
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
        
        // Set the starting position distance to 0
        distanceGrid[startingPosition.x, startingPosition.y] = 0;

        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        queue.Enqueue(startingPosition);

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();
            int currentDistance = distanceGrid[current.x, current.y];

            foreach (var direction in Directions)
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
    
    /// <summary>
    /// Returns a List of coordinates that depicts the steps to take to get to a specific point.
    /// The first item is the current position
    /// </summary>
    /// <param name="distanceGrid">2d array of how far each tile on the map is</param>
    /// <param name="targetPosition">The target position you want to reach</param>
    /// <returns></returns>
    protected List<Vector2Int> GetPathToPoint(int[,] distanceGrid, Vector2Int targetPosition)
    {
        var (playerX, playerY) = GetPlayerPosition();

        List<Vector2Int> path = new List<Vector2Int>();

        // Start from the target position
        Vector2Int currentPosition = targetPosition;

        // While we haven't reached the starting position
        while (distanceGrid[currentPosition.x, currentPosition.y] != 0)
        {
            path.Add(currentPosition);

            Vector2Int? nextStep = null;
            int minDistance = int.MaxValue;

            // Check all four possible directions
            foreach (var direction in Directions)
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
            if (nextStep == null)
            {
                Debug.LogWarning("No valid next step found, ending path tracing.");
                break;
            }

            currentPosition = nextStep.Value;
        }

        // Add the starting position to the path (the enemy's current position)
        path.Add(new Vector2Int((int)transform.position.x, (int)transform.position.y));

        // Reverse the path to start from the initial position
        path.Reverse();
        return path;
    }

    private bool IsPositionValid(Vector2Int position)
    {
        // Check bounds and whether it's an obstacle
        return position.x >= 0 && position.x < grids.columns &&
               position.y >= 0 && position.y < grids.rows &&
               !grids.IsCellOccupied(position.x, position.y); // Check for obstacles
    }
    
    protected void PrintGrid(int[,] grid)
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
