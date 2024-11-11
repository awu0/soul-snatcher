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
}
