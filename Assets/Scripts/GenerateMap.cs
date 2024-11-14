using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class GenerateMap
{   
    private static readonly System.Random random = new System.Random();

    public static bool[] Generate(int width, int height, int iterations = 4, int percentAreWalls = 40)
    {
        var map = new bool[width * height];

        RandomFill(map, width, height, percentAreWalls);
        
        for(var i = 0; i < iterations; i++)
            map = Step(map, width, height);

        return map;
    }
    
    private static void RandomFill(bool[] map, int width, int height, int percentAreWalls = 40)
    {
        var randomColumn = random.Next(4, width - 4);
        
        for(int y = 0; y < height; y++)
        {
            for(int x = 0; x < width; x++)
            {
                if(x == 0 || y == 0 || x == width - 1 || y == height - 1)
                    map[x + y * width] = true;
                else if(x != randomColumn && random.Next(100) < percentAreWalls)
                    map[x + y * width] = true;
            }
        }
    }

    private static bool[] Step(bool[] map, int width, int height)
    {
        var newMap = new bool[width * height];
        
        for(int y = 0; y < height; y++)
        {
            for(int x = 0; x < width; x++)
            {
                if(x == 0 || y == 0 || x == width - 1 || y == height - 1)
                    newMap[x + y * width] = true;
                else
                    newMap[x + y * width] = PlaceWallLogic(map, width, height, x, y);
            }
        }

        return newMap;
    }

    private static bool PlaceWallLogic(bool[] map, int width, int height, int x, int y) =>
        CountAdjacentWalls(map, width, height, x, y) >= 5 ||
        CountNearbyWalls(map, width, height, x, y) <= 2;

    private static int CountAdjacentWalls(bool[] map, int width, int height, int x, int y)
    {
        var walls = 0;
        
        for(var mapX = x - 1; mapX <= x + 1; mapX++)
        {
            for(var mapY = y - 1; mapY <= y + 1; mapY++)
            {
                if(map[mapX + mapY * width])
                    walls++;
            }
        }

        return walls;
    }
    
    private static int CountNearbyWalls(bool[] map, int width, int height, int x, int y)
    {
        var walls = 0;
        
        for(var mapX = x - 2; mapX <= x + 2; mapX++)
        {
            for(var mapY = y - 2; mapY <= y + 2; mapY++)
            {
                if(Math.Abs(mapX - x) == 2 && Math.Abs(mapY - y) == 2)
                    continue;
                
                if(mapX < 0 || mapY < 0 || mapX >= width || mapY >= height)
                    continue;
                        
                if(map[mapX + mapY * width])
                    walls++;
            }
        }

        return walls;
    }
}
