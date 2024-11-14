using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class GenerateMap
{   
    // Random number generator for map generation
    private static readonly System.Random random = new System.Random();

    // Main method to generate the map using cellular automata
    public static bool[] Generate(int width, int height, int iterations = 4, int percentAreWalls = 40)
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
}