using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/**
 * Controls all the enemies and handles spawning and death 
 */
public class EnemyManager : MonoBehaviour
{
    private List<Enemy> _enemies = new List<Enemy>();
    
    public IReadOnlyList<Enemy> enemies => _enemies.AsReadOnly();
    
    public Grids grids;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
        
        _enemies.Add(newEnemy);
        
        grids.SetCellOccupied(x, y, true);
    }
    
    /**
     * Retrieve the prefab for a given enemy type
     * In the folder: Assets/Resources/Prefabs
     */
    private GameObject GetPrefabForType<T>() where T : Enemy
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/" + typeof(T).Name);
        if (prefab == null)
        {
            Debug.LogError($"Prefab not found for {typeof(T).Name}");
        }
        return prefab;
    }

    public int GetEnemiesCount()
    {
        return _enemies.Count;
    }
}