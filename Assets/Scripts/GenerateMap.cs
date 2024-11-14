using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class GenerateMap
{   
    // Random number generator for map generation
    private static readonly System.Random random = new System.Random();

    // Main method to generate the map using cellular automata
    public static bool[] Generate(int width, int height, int iterations = 4, int percentAreWalls = 45)
    {
        // Create an array representing the map
        var map = new bool[width * height];

        // Fill the map randomly based on the percentage of walls
        RandomFill(map, width, height, percentAreWalls);
        
        // Apply cellular automata rules for a given number of iterations
        for (var i = 0; i < iterations; i++)
            map = Step(map, width, height);

        // Return the final processed map
        return map;
    }
    
    // Method to randomly fill the map with walls and open spaces
    private static void RandomFill(bool[] map, int width, int height, int percentAreWalls = 40)
    {
        // Select a random column to leave open as a potential pathway
        var randomColumn = random.Next(4, width - 4);
        
        // Loop through each cell in the map
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // Ensure the border of the map is always walls
                if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                    map[x + y * width] = true; // Mark as wall
                // Randomly set cells as walls based on the given percentage
                else if (x != randomColumn && random.Next(100) < percentAreWalls)
                    map[x + y * width] = true; // Mark as wall
            }
        }
    }

    // Method to apply cellular automata rules and create the next step of the map
    private static bool[] Step(bool[] map, int width, int height)
    {
        // Create a new map array to store the results of this step
        var newMap = new bool[width * height];
        
        // Iterate through each cell in the map
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // Keep the borders as walls
                if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                    newMap[x + y * width] = true; // Mark as wall
                else
                    // Apply logic to determine if a cell should be a wall based on neighbors
                    newMap[x + y * width] = PlaceWallLogic(map, width, height, x, y);
            }
        }

        // Return the updated map after this step
        return newMap;
    }

    // Logic to determine if a cell should be a wall based on surrounding walls
    private static bool PlaceWallLogic(bool[] map, int width, int height, int x, int y) =>
        CountAdjacentWalls(map, width, height, x, y) >= 5 || // If 5 or more adjacent cells are walls, make this cell a wall
        CountNearbyWalls(map, width, height, x, y) <= 2; // If 2 or fewer walls are nearby, make this cell a wall

    // Count the number of walls in the adjacent 3x3 grid around a cell
    private static int CountAdjacentWalls(bool[] map, int width, int height, int x, int y)
    {
        var walls = 0;

        // Iterate through the 3x3 grid centered at (x, y)
        for (var mapX = x - 1; mapX <= x + 1; mapX++)
        {
            for (var mapY = y - 1; mapY <= y + 1; mapY++)
            {
                // Increment count if the cell is a wall
                if (map[mapX + mapY * width])
                    walls++;
            }
        }

        // Return the count of adjacent walls
        return walls;
    }

    // Count the number of walls in a 5x5 grid around a cell, excluding corners
    private static int CountNearbyWalls(bool[] map, int width, int height, int x, int y)
    {
        var walls = 0;

        // Iterate through the 5x5 grid centered at (x, y)
        for (var mapX = x - 2; mapX <= x + 2; mapX++)
        {
            for (var mapY = y - 2; mapY <= y + 2; mapY++)
            {
                // Skip the corners of the 5x5 grid
                if (Math.Abs(mapX - x) == 2 && Math.Abs(mapY - y) == 2)
                    continue;

                // Skip cells that are out of map bounds
                if (mapX < 0 || mapY < 0 || mapX >= width || mapY >= height)
                    continue;

                // Increment count if the cell is a wall
                if (map[mapX + mapY * width])
                    walls++;
            }
        }

        // Return the count of nearby walls
        return walls;
    }


    public static void ConnectOpenSpaces(bool[] map, int width, int height, Vector2Int playerPos)
    {
        bool[] visited = new bool[map.Length];
        List<Vector2Int> mainRegion = GetRegionTiles(map, width, height, playerPos, visited);

        // Traverse the map and connect any isolated regions to the main region
        for (int y = 1; y < height - 1; y++)
        {
            for (int x = 1; x < width - 1; x++)
            {
                if (!map[x + y * width] && !visited[x + y * width]) // If the cell is open and not visited
                {
                    List<Vector2Int> newRegion = GetRegionTiles(map, width, height, new Vector2Int(x, y), visited);

                    // Find the closest point in the main region to the new region
                    Vector2Int closestPointInMain = FindClosestPoint(newRegion, mainRegion);
                    Vector2Int closestPointInNew = FindClosestPoint(mainRegion, newRegion);

                    // Create a path between the closest points
                    CreatePath(map, closestPointInMain, closestPointInNew, width);
                }
            }
        }
    }

    // Finds the closest point between two sets of points
    private static Vector2Int FindClosestPoint(List<Vector2Int> region1, List<Vector2Int> region2)
    {
        Vector2Int closestPoint = region1[0];
        int minDistance = int.MaxValue;

        foreach (Vector2Int point1 in region1)
        {
            foreach (Vector2Int point2 in region2)
            {
                int distance = Math.Abs(point1.x - point2.x) + Math.Abs(point1.y - point2.y);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestPoint = point1;
                }
            }
        }

        return closestPoint;
    }

    private static void CreatePath(bool[] map, Vector2Int start, Vector2Int end, int width)
    {
        Vector2Int current = start;

        // First, move horizontally to align the x-coordinate
        while (current.x != end.x)
        {
            map[current.x + current.y * width] = false; // Mark as open space

            if (current.x < end.x) current.x++;
            else if (current.x > end.x) current.x--;
        }

        // Then, move vertically to align the y-coordinate
        while (current.y != end.y)
        {
            map[current.x + current.y * width] = false; // Mark as open space

            if (current.y < end.y) current.y++;
            else if (current.y > end.y) current.y--;
        }
    }

    // Flood-fill algorithm to get all tiles in a region
    private static List<Vector2Int> GetRegionTiles(bool[] map, int width, int height, Vector2Int start, bool[] visited)
    {
        List<Vector2Int> region = new List<Vector2Int>();
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        queue.Enqueue(start);
        visited[start.x + start.y * width] = true;

        while (queue.Count > 0)
        {
            Vector2Int tile = queue.Dequeue();
            region.Add(tile);

            // Check all four cardinal directions
            foreach (Vector2Int dir in new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right })
            {
                Vector2Int neighbor = new Vector2Int(tile.x + dir.x, tile.y + dir.y);

                // Ensure the neighbor is within bounds and is open space
                if (neighbor.x >= 1 && neighbor.x < width - 1 && neighbor.y >= 1 && neighbor.y < height - 1)
                {
                    if (!map[neighbor.x + neighbor.y * width] && !visited[neighbor.x + neighbor.y * width])
                    {
                        visited[neighbor.x + neighbor.y * width] = true;
                        queue.Enqueue(neighbor);
                    }
                }
            }
        }

        return region;
    }
}