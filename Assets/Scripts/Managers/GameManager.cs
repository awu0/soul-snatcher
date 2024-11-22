using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;

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

    public GameObject PrefabStairs;
    private GameObject stairs;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) // Replace 'R' with any key you prefer
        {
            StartNextLevel();
        }
        if (Input.GetKeyDown(KeyCode.T)) // Replace 'R' with any key you prefer
        {
            ResetGame();
        }
        if (Input.GetKeyDown(KeyCode.Q)) // Replace 'R' with any key you prefer
        {   
            if (player.actionCount >= 1) {
                player.actionCount -= 1;
            }
        }
    }

    private void StartLevel() 
    {
        int width = grids.columns;
        int height = grids.rows;
        bool[] map = GenerateMap.Generate(width, height); //GENERATE MAP

        //SPAWN PLAYER
        Vector2Int playerSpawn = grids.RandomValidSpawnPosition(map, width, height);
        _playerStartX = playerSpawn.x;
        _playerStartY = playerSpawn.y;
        player.MoveTo(_playerStartX, _playerStartY);

        //SPAWN STAIRS
        stairsPos = grids.FindRandomDistantPosition(map, width, height, playerSpawn, 5);
        stairs = Instantiate(PrefabStairs, new Vector3(grids.leftBottomLocation.x + stairsPos.x * grids.scale, grids.leftBottomLocation.y + stairsPos.y * grids.scale, 0), Quaternion.identity);
        stairs.GetComponent<SpriteRenderer>().sortingOrder = 1;
        Debug.Log($"Stairs spawned at: {stairsPos}");

        //CONNECT CLOSED AREAS
        GenerateMap.ConnectOpenSpaces(map, width, height, playerSpawn);
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (map[x + y * width])
                {
                    entityManager.SpawnWall(x, y); 
                }
            }
        }
        
        //SPAWN ENEMIES
        List<Type> enemiesToSpawn = GenerateEnemyList();

        for (int i = 0; i < enemiesToSpawn.Count; i++)
        {
            Type enemyType = enemiesToSpawn[i];
            Vector2Int enemySpawn = grids.FindRandomDistantPosition(map, width, height, playerSpawn, 4);
            //grids.RandomValidSpawnPosition(map, width, height);

            // Use reflection to call SpawnEnemy<T>
            typeof(EntityManager).GetMethod("SpawnEnemy")
                .MakeGenericMethod(enemyType)
                .Invoke(entityManager, new object[] { enemySpawn.x, enemySpawn.y });
        }
    }
    
    private List<Type> GenerateEnemyList()
    {
        List<Type> allEnemyTypes = new List<Type>
        {
            typeof(EvilEye),
            typeof(GiantPillbug),
            typeof(StoneGolem),
            typeof(Snake)
        };

        int enemiesAmt = 3;//Mathf.Clamp(player.level * 2, 3, 10);

        List<Type> enemiesToSpawn = new List<Type>();

        // Randomly select enemy types to spawn
        for (int i = 0; i < enemiesAmt; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, allEnemyTypes.Count);
            enemiesToSpawn.Add(allEnemyTypes[randomIndex]);
        }

        return enemiesToSpawn;
    }

    public void StartNextLevel()
    {
        player.Reset();
        state = STATES.ROUND_START;

        grids.DeleteGridPrefabs();
        entityManager.DeleteEntityPrefabs();
        Destroy(stairs.gameObject);
        StartLevel();
        grids.GenerateGrid();
    }

    public void ResetGame()
    {
        Scene currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        UnityEngine.SceneManagement.SceneManager.LoadScene(currentScene.name);
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
                    if (player.locX == stairsPos.x && player.locY == stairsPos.y)
                    {
                        StartNextLevel(); 
                        break;
                    }
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
