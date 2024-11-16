using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public enum STATES
    {
        ROUND_START,
        PLAYER_ROUND,
        ENEMY_ROUND,
        ROUND_END,
    }
    public Player player;
    public Vector2Int stairsPos;

    private int _playerStartX = 1;
    private int _playerStartY = 1;
    
    public STATES state = STATES.ROUND_START;
    [NonSerialized] public float pauseDuration = 0.25f;

    public EntityManager entityManager;

    public Grids grids;

    public void Awake()
    {
        var gridObject = GameObject.FindGameObjectWithTag("Game Board");
        if (gridObject != null)
        {
            grids = gridObject.GetComponent<Grids>();
        } 
    }

    private void Start()
    {
        StartLevel();
        StartCoroutine(RunTurnManager());

    }

    private void StartLevel() 
    {
        int width = grids.columns;
        int height = grids.rows;

        bool[] map = GenerateMap.Generate(width, height);
        Vector2Int playerSpawn = grids.RandomValidSpawnPosition(map, width, height);
        _playerStartX = playerSpawn.x;
        _playerStartY = playerSpawn.y;
        player.MoveTo(_playerStartX, _playerStartY);

        GenerateMap.ConnectOpenSpaces(map, width, height, playerSpawn);
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (map[x + y * width])
                {
                    entityManager.SpawnObstacle<Rock>(x, y); //Replace with proper walls later
                }
            }
        }
        
        List<Type> enemiesToSpawn = GenerateEnemyList();

        for (int i = 0; i < enemiesToSpawn.Count; i++)
        {
            Type enemyType = enemiesToSpawn[i];
            Vector2Int enemySpawn = grids.RandomValidSpawnPosition(map, width, height);

            // Use reflection to call SpawnEnemy<T>
            typeof(EntityManager).GetMethod("SpawnEnemy")
                .MakeGenericMethod(enemyType)
                .Invoke(entityManager, new object[] { enemySpawn.x, enemySpawn.y });
        }
    }

    private List<Type> GenerateEnemyList()
    {
        List<Type> enemiesToSpawn = new List<Type>
        {
            typeof(EvilEye),
            typeof(GiantPillbug),
            typeof(StoneGolem),
            typeof(Snake)
        }; 

        return enemiesToSpawn;
    }

    private IEnumerator RunTurnManager()
    {
        while (true)
        {
            switch (state)
            {
                case STATES.ROUND_START:
                    Debug.Log("ROUND START");
                    // refill action count
                    player.actionCount = player.maxActionCount;
                    
                    // reduce buffs/debuffs/status effects duration by 1 turn
                    player.TickDownStatusEffectsAndBuffs();
                    
                    state = STATES.PLAYER_ROUND;
                    
                    Debug.Log("PLAYER ROUND");
                    break;

                case STATES.PLAYER_ROUND:
                    //go to next round if player can't action anymore
                    if (player != null)
                    {
                        if (player.actionCount <= 0)
                        {
                            state = STATES.ENEMY_ROUND;
                        }
                    }
                    else {
                        state = STATES.ENEMY_ROUND;
                    }
                    yield return new WaitForSeconds(pauseDuration);
                    break;

                case STATES.ENEMY_ROUND:
                    Debug.Log("ENEMY ROUND");

                    foreach (var enemy in entityManager.enemies)
                    {
                        if (enemy != null)
                        {
                            // reduce buffs/debuffs/status effects duration by 1 turn
                            enemy.TickDownStatusEffectsAndBuffs(); 
                            
                            enemy.DetermineNextMove();
                            yield return new WaitForSeconds(pauseDuration);   
                        }
                    }
                    
                    state = STATES.ROUND_END;
                    break;

                case STATES.ROUND_END:
                    Debug.Log("ROUND END");
                    
                    entityManager.RemoveDeadEnemies();
                    entityManager.RemoveDeadObstacles();
                    
                    state = STATES.ROUND_START;
                    break;
            }
        }
    }

}
