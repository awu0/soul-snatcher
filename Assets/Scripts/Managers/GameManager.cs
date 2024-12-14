using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;
using TMPro;

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
  public static int level = 0;

  private int _playerStartX = 1;
  private int _playerStartY = 1;

  public List<Type> enemiesToSpawn;
  public int enemiesAmt = 0;
  private int enemiesLimit = 6;
  public Enemy lastEnemyTurn;

  public STATES state = STATES.ROUND_START;
  [NonSerialized] public float pauseDuration = 0.4f;

  public EntityManager entityManager;

  public Grids grids;

  public EnemyDisplay enemyDisplay;

  public TextMeshProUGUI levelText;

  public int tutorialLevel = 2;
  public int tutorialStep = 0;
  private int tutorialActionCount = 0;
  public TextMeshProUGUI tutorialStepText;
  public GameObject tutorialUI;
  public GameObject deathScreen;
  public Coroutine turnManagerCoroutine;
  public void Awake()
  {
    var gridObject = GameObject.FindGameObjectWithTag("Game Board");
    if (gridObject != null)
    {
      grids = gridObject.GetComponent<Grids>();
    }
    if (enemyDisplay == null)
    {
      enemyDisplay = FindObjectOfType<EnemyDisplay>();
      if (enemyDisplay == null)
      {
        Debug.LogError("EnemyDisplay not found in the scene!");
      }
    }

    if (SceneData.isTutorial)
    {
      tutorialUI.SetActive(true);
    }
    else
    {
      tutorialUI.SetActive(false);
    }
  }

  private void Start()
  {
    enemiesToSpawn = new List<Type>();
    GenerateEnemyList();
    StartLevel();
  }

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.R)) // Replace 'R' with any key you prefer
    {
      SceneData.isTutorial = false;
      StartNextLevel();
    }
    if (Input.GetKeyDown(KeyCode.T)) // Replace 'R' with any key you prefer
    {
      ResetGame();
    }
    if (Input.GetKeyDown(KeyCode.Q)) // Replace 'R' with any key you prefer
    {
      if (player.actionCount >= 1)
      {
        player.actionCount -= 1;
      }
    }

    // for tutorial
    if (SceneData.isTutorial)
    {
      tutorialStepText.text = TutorialData.TutorialStepText[tutorialStep];

      if (tutorialStep == 0 && Math.Abs(player.locX - stairsPos.x) < 5 && Math.Abs(player.locY - stairsPos.y) < 5)
      {
        tutorialStep = 1;
      }

      if (tutorialStep == 1 && tutorialLevel == 1)
      {
        tutorialStep = 2;
      }

      if (tutorialStep == 2 && tutorialActionCount == 2)
      {
        tutorialStep = 3;
        tutorialActionCount = 0;
      }

      // tutorial step 3 handled in Player.cs

      // tutorial step 4 handled in Player.cs

      if (tutorialStep == 5 && tutorialActionCount == 3)
      {
        tutorialStep = 6;
        tutorialActionCount = 0;
      }

      // tutorial step 6 handled in Player.cs

      // tutorial step 7 handled in StartNextTutorialLevel()

      // tutorial step 8 and 9 handled in Player.cs

      if (tutorialStep == 10 && tutorialActionCount == 3)
      {
        tutorialStep = 11;
      }

      if (tutorialStep == 11 && tutorialActionCount == 6)
      {
        tutorialStep = 12;
      }
    }
  }

  private void StartLevel()
  {
    int width = grids.columns;
    int height = grids.rows;

    // if not in tutorial (standard gameplay)
    //  1. generate random map
    //  2. spawn player randomly
    //  3. spawn stairs far away from player
    //  4. connect closed areas
    //  5. spawn enemies
    if (!SceneData.isTutorial)
    {
      bool[] map = GenerateMap.Generate(width, height); //GENERATE MAP

      //SPAWN PLAYER
      Vector2Int playerSpawn = grids.RandomValidSpawnPosition(map, width, height);
      _playerStartX = playerSpawn.x;
      _playerStartY = playerSpawn.y;
      player.PlaceEntity(_playerStartX, _playerStartY);

      //SPAWN STAIRS
      stairsPos = grids.FindRandomDistantPosition(map, width, height, playerSpawn, 3 + Mathf.FloorToInt(level / 3));
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
      for (int i = 0; i < enemiesToSpawn.Count; i++)
      {
        Type enemyType = enemiesToSpawn[i];
        Vector2Int enemySpawn = grids.FindRandomDistantPosition(map, width, height, playerSpawn, 4);
        //grids.RandomValidSpawnPosition(map, width, height);

        Debug.Log(enemyType);
        // Use reflection to call SpawnEnemy<T>
        typeof(EntityManager).GetMethod("SpawnEnemy")
            .MakeGenericMethod(enemyType)
            .Invoke(entityManager, new object[] { enemySpawn.x, enemySpawn.y });
      }

      //CREATE ENEMY LIST FOR NEXT LEVEL
      GenerateEnemyList();
      enemyDisplay.DisplayEnemies();
    }
    else
    {
      levelText.text = "Floor " + tutorialLevel.ToString();
      BuildTutorialLevel();
    }
    
    StopAllCoroutines();
    // if (turnManagerCoroutine != null)
    // {
    //   StopCoroutine(RunTurnManager());
    // }
    turnManagerCoroutine = StartCoroutine(RunTurnManager());
  }

  private void GenerateEnemyList()
  {
    // Define initial spawn rates for each enemy type
    enemiesToSpawn = new List<Type>();
    Dictionary<Type, float> enemySpawnRates = new Dictionary<Type, float>
    {
        { typeof(EvilEye), 1.0f },
        { typeof(GiantPillbug), 1.0f },
        { typeof(StoneGolem), 0.6f },
        { typeof(Snake), 1.0f },
        { typeof(Hooker), 1.0f }
    };

    float totalWeight = 0;
    foreach (var weight in enemySpawnRates.Values)
    {
      totalWeight += weight;
    }

    if (level % 2 == 0 && enemiesAmt < enemiesLimit)
    {
      enemiesAmt += 1;
    }

    for (int i = 0; i < enemiesAmt; i++)
    {
      float randomValue = UnityEngine.Random.Range(0f, totalWeight);
      float currWeight = 0;

      foreach (var enemyType in enemySpawnRates)
      {
        currWeight += enemyType.Value;
        if (randomValue <= currWeight)
        {
          enemiesToSpawn.Add(enemyType.Key);

          // Reduce the spawn weight of the selected enemy type
          enemySpawnRates[enemyType.Key] *= 0.5f;

          // Recalculate the total weight after reducing
          totalWeight = 0;
          foreach (var weight in enemySpawnRates.Values)
          {
            totalWeight += weight;
          }
          break;
        }
      }
    }
  }


  public void StartNextLevel()
  {
    player.Reset();
    state = STATES.ROUND_START;

    grids.DeleteGridPrefabs();
    entityManager.DeleteEntityPrefabs();
    Destroy(stairs.gameObject);

    level += 1;
    levelText.text = "Floor " + level.ToString();
    if (level % 2 == 0)
    {
      grids.rows += 1;
      grids.columns += 1;
    }

    grids.GenerateGrid();
    StartLevel();
  }

  public void ToggleDeathScreen(bool active)
  {
    deathScreen.SetActive(active);
  }

  public void StartNextTutorialLevel()
  {
    // if tutorial is complete, go back to start screen
    if (tutorialLevel == 2)
    {
      SceneData.isTutorial = false;
      SceneManager.LoadScene("StartScreen");
    }
    else if (tutorialLevel == 1)
    {
      tutorialStep = 8;
    }

    player.Reset();
    state = STATES.ROUND_START;

    grids.DeleteGridPrefabs();
    entityManager.DeleteEntityPrefabs();
    Destroy(stairs.gameObject);

    tutorialLevel += 1;
    levelText.text = "Floor " + tutorialLevel.ToString();

    grids.GenerateGrid();
    StartLevel();
  }

  public void ResetGame()
  {
    level = 0;
    levelText.text = "Floor " + level.ToString();
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
          lastEnemyTurn = null;
          // refill action count
          player.actionCount = player.maxActionCount;
          for (int i = entityManager.enemies.Count - 1; i >= 0; i--)
          {
            if (entityManager.enemies[i] != null)
            {
              entityManager.enemies[i].actionCount = entityManager.enemies[i].maxActionCount;
            }
          }
          // foreach (var enemy in entityManager.enemies)
          // {
          //   if (enemy != null)
          //   {
          //     enemy.actionCount = enemy.maxActionCount;
          //   }
          // }

          // reduce buffs/debuffs/status effects duration by 1 turn
          player.TickDownStatusEffectsAndBuffs();

          state = STATES.PLAYER_ROUND;

          Debug.Log("PLAYER ROUND");
          break;

        case STATES.PLAYER_ROUND:
          if (player.locX == stairsPos.x && player.locY == stairsPos.y)
          {
            if (SceneData.isTutorial)
            {
              StartNextTutorialLevel();
            }
            else
            {
              StartNextLevel();
            }

            break;
          }
          //go to next round if player can't action anymore
          if (player != null)
          {
            if (player.actionCount <= 0)
            {
              state = STATES.ENEMY_ROUND;

              if (SceneData.isTutorial)
              {
                if (tutorialLevel == 1 || tutorialLevel == 2)
                {
                  tutorialActionCount++;
                }

                if (tutorialStep == 4 || tutorialStep == 8 || tutorialStep == 9)
                {
                  tutorialActionCount = 0;
                }
              }
            }
          }
          else
          {
            state = STATES.ENEMY_ROUND;
          }
          yield return StartCoroutine(WaitForPlayerTurn(player));
          break;

        case STATES.ENEMY_ROUND:
          Debug.Log("ENEMY ROUND");

          for (int i = entityManager.enemies.Count - 1; i >= 0; i--)
          {
            if (entityManager.enemies[i] != null)
            {
              lastEnemyTurn = entityManager.enemies[i];
              // reduce buffs/debuffs/status effects duration by 1 turn
              entityManager.enemies[i].TickDownStatusEffectsAndBuffs();

              yield return StartCoroutine(WaitForEnemyTurn(entityManager.enemies[i]));
            }
          }
          
          // foreach (var enemy in entityManager.enemies)
          // {
          //   if (enemy != null)
          //   {
          //     lastEnemyTurn = enemy;
          //     // reduce buffs/debuffs/status effects duration by 1 turn
          //     enemy.TickDownStatusEffectsAndBuffs();

          //     yield return StartCoroutine(WaitForEnemyTurn(enemy));
          //   }
          // }

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

  private IEnumerator WaitForEnemyTurn(Enemy enemy)
  {
    if (enemy == null) {
      yield break;
    }
    enemy.TakeTurn();

    yield return new WaitForSeconds(pauseDuration);

    while (enemy.isMoving)
    {
      yield return null;
    }
  }

  private IEnumerator WaitForPlayerTurn(Player player)
  {
    yield return new WaitForSeconds(pauseDuration);

    while (player.isMoving)
    {
      yield return null;
    }
  }

  public void BuildTutorialLevel()
  {
    int[,] tutorialLevelData = new int[TutorialData.tutorialRows, TutorialData.tutorialColumns];

    if (tutorialLevel == 0)
    {
      tutorialLevelData = TutorialData.TutorialLevelOneData;
    }
    else if (tutorialLevel == 1)
    {
      tutorialLevelData = TutorialData.TutorialLevelTwoData;
    }
    else if (tutorialLevel == 2)
    {
      tutorialLevelData = TutorialData.TutorialLevelThreeData;
    }

    // The default size grid gets set at runtime.
    // Let's delete this default grid in the case of the tutorial
    grids.DeleteGridPrefabs();

    grids.columns = tutorialLevelData.GetLength(0);
    grids.rows = tutorialLevelData.GetLength(1);
    grids.GenerateGrid();

    for (int i = 0; i < tutorialLevelData.GetLength(0); i++)
    {
      for (int j = 0; j < tutorialLevelData.GetLength(1); j++)
      {
        if (tutorialLevelData[i, j] == 0)
        {
          continue;
        }
        else if (tutorialLevelData[i, j] == 1)
        {
          grids.SetWall(i, j, true);
          entityManager.SpawnWall(i, j);
        }
        else if (tutorialLevelData[i, j] == 2)
        {
          player.PlaceEntity(i, j);
        }
        else if (tutorialLevelData[i, j] == 3)
        {
          stairsPos.x = i;
          stairsPos.y = j;
          stairs = Instantiate(PrefabStairs, new Vector3(i, j, 0), Quaternion.identity);
          stairs.GetComponent<SpriteRenderer>().sortingOrder = 1;
        }
        else if (tutorialLevelData[i, j] == 4)
        {
          typeof(EntityManager).GetMethod("SpawnEnemy")
            .MakeGenericMethod(typeof(GiantPillbug))
            .Invoke(entityManager, new object[] { i, j });
        }
        else if (tutorialLevelData[i, j] == 5)
        {
          typeof(EntityManager).GetMethod("SpawnEnemy")
            .MakeGenericMethod(typeof(EvilEye))
            .Invoke(entityManager, new object[] { i, j });
        }
      }
    }
  }
}
