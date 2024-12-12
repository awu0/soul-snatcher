using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/**
 * Controls all the enemies and handles spawning and death 
 */
public class EntityManager : MonoBehaviour
{
    private List<Enemy> _enemies = new List<Enemy>();
    public IReadOnlyList<Enemy> enemies => _enemies.AsReadOnly();
    
    private List<Obstacle> _obstacles = new List<Obstacle>();
    public IReadOnlyList<Obstacle> obstacles => _obstacles.AsReadOnly();

    private List<Wall> _walls = new List<Wall>();
    public IReadOnlyList<Wall> walls => _walls.AsReadOnly();
    
    public Grids grids;
    
    public void SpawnEnemy<T>(int x, int y) where T : Enemy
    {
        // Check if the target position is free
        if (grids.IsCellOccupied(x, y))
        {
            Debug.LogWarning($"Cannot spawn enemy at ({x}, {y}): cell is occupied.");
            return;
        }
        
        GameObject prefab = GetPrefabForType<T>();

        Vector3 position = new Vector3(x, y, 0);
        GameObject newEnemyObject = Instantiate(prefab, position, Quaternion.identity);
        
        Enemy newEnemy = newEnemyObject.GetComponent<Enemy>();

        newEnemy.locX = x;
        newEnemy.locY = y;
        
        // get audio
        AudioClip damageSFXClip = Resources.Load<AudioClip>("Audio/Enemy Damage");
        if (damageSFXClip == null)
        {
            Debug.LogWarning("Failed to load damage audio clip from Resources/Audio/Enemy Damage.");
        }
        else
        {
            AudioSource audioSource = newEnemyObject.GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = newEnemyObject.AddComponent<AudioSource>();
            }
            audioSource.clip = damageSFXClip;
            newEnemy.damageSFX = audioSource;
        }
        
        _enemies.Add(newEnemy);
        
        grids.SetCellOccupied(x, y, newEnemy);
    }
    
    /**
     * Retrieve the prefab for a given entity type
     * In the folder: Assets/Resources/Prefabs
     */
    private GameObject GetPrefabForType<T>() where T : Entity
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/" + typeof(T).Name);
        if (prefab == null)
        {
            Debug.LogError($"Prefab not found for {typeof(T).Name}");
        }
        return prefab;
    }

    public void SpawnObstacle<T>(int x, int y) where T : Obstacle
    {
        // Check if the target position is free
        if (grids.IsCellOccupied(x, y))
        {
            Debug.LogWarning($"Cannot spawn obstacle at ({x}, {y}): cell is occupied.");
            return;
        }
        
        GameObject prefab = GetPrefabForType<T>();

        Vector3 position = new Vector3(x, y, 0);
        GameObject newObstacleObject = Instantiate(prefab, position, Quaternion.identity);
        Obstacle newObstacle = newObstacleObject.GetComponent<Obstacle>();

        newObstacle.locX = x;
        newObstacle.locY = y;
        
        _obstacles.Add(newObstacle);
        
        grids.SetCellOccupied(x, y, newObstacle);
    }
    
    /// <summary>
    /// Spawns a wall, which blocks line of sight
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void SpawnWall(int x, int y)
    {
        // Check if the target position is free
        if (grids.IsCellOccupied(x, y))
        {
            Debug.LogWarning($"Cannot spawn wall at ({x}, {y}): cell is occupied.");
            return;
        }
        
        string[] wallTypes = { "Wall", "Wall1", "Wall1", "Wall1" };
        string selectedWallType = wallTypes[UnityEngine.Random.Range(0, wallTypes.Length)];
        GameObject prefab = Resources.Load<GameObject>("Prefabs/" + selectedWallType);

        Vector3 position = new Vector3(x, y, 0);
        //Quaternion rotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 4) * 90);
        //GameObject newWallObject = Instantiate(prefab, position, rotation);
        GameObject newWallObject = Instantiate(prefab, position, Quaternion.identity);
        Wall newWall = newWallObject.GetComponent<Wall>();

        newWall.locX = x;
        newWall.locY = y;
        
        grids.SetCellOccupied(x, y, newWall);
        grids.SetWall(x, y, true);
        _walls.Add(newWall);
    }
    
    public int GetEnemiesCount()
    {
        return _enemies.Count;
    }

    public void RemoveDeadEnemies()
    {
        _enemies.RemoveAll(enemy => enemy == null);
    }
    
    public void RemoveDeadObstacles()
    {
        _obstacles.RemoveAll(obs => obs == null);
    }

    public void DeleteEntityPrefabs()
    {
        // Delete all enemy prefabs
        foreach (var enemy in _enemies)
        {
            if (enemy != null)
            {
                Destroy(enemy.gameObject); // Destroy the enemy GameObject
            }
        }
        _enemies.Clear(); // Clear the list after deletion

        // Delete all obstacle prefabs
        foreach (var obstacle in _obstacles)
        {
            if (obstacle != null)
            {
                Destroy(obstacle.gameObject); // Destroy the obstacle GameObject
            }
        }
        _obstacles.Clear(); // Clear the list after deletion

        // Delete all wall prefabs
        foreach (var wall in _walls)
        {
            if (wall != null)
            {
                Destroy(wall.gameObject); // Destroy the wall GameObject
            }
        }
        _obstacles.Clear(); // Clear the list after deletion

        // Optionally, clear entity references in the grid if needed
        for (int x = 0; x < grids.columns; x++)
        {
            for (int y = 0; y < grids.rows; y++)
            {
                // Clear the cell in the grid if it holds an entity
                Entity entity = grids.GetEntityAt(x, y);
                if (entity != null)
                {
                    grids.SetCellOccupied(x, y, null); // Clear the reference from the grid
                }
            }
        }

        Debug.Log("All enemy and obstacle prefabs deleted.");
    }
}
