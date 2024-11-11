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
        PLAYER_ACTION,
        ENEMY_ROUND,
        ROUND_END,
    }
    public Player player;

    private const int _playerStartX = 1;
    private const int _playerStartY = 1;
    
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
        player.MoveTo(_playerStartX, _playerStartY);
        
        entityManager.SpawnEnemy<EvilEye>(9, 1);
        entityManager.SpawnEnemy<GiantPillbug>(9, 9);
        entityManager.SpawnEnemy<StoneGolem>(5, 5);
        entityManager.SpawnEnemy<Snake>(3, 3);
        
        entityManager.SpawnObstacle<Rock>(8, 1);
        entityManager.SpawnObstacle<Rock>(9, 2);
        entityManager.SpawnObstacle<Rock>(1, 3);
        entityManager.SpawnObstacle<Rock>(5, 0);
        entityManager.SpawnObstacle<Rock>(7, 7);
        
        StartCoroutine(RunTurnManager());
    }

    private IEnumerator RunTurnManager()
    {
        while (true)
        {
            switch (state)
            {
                case STATES.ROUND_START:
                    // refill action count
                    player.actionCount = player.maxActionCount;
                    
                    Debug.Log("ROUND START");
                    state = STATES.PLAYER_ROUND;
                    break;

                case STATES.PLAYER_ROUND:
                    // Debug.Log("PLAYER ROUND");
                    //go to next round if player can't action anymore
                    if (player != null)
                    {
                        // reduce buffs/debuffs/status effects duration by 1 turn
                        player.TickDownStatusEffectsAndBuffs();
                        
                        if (player.actionCount <= 0)
                        {
                            state = STATES.PLAYER_ACTION;
                        }
                    }
                    else {
                        state = STATES.PLAYER_ACTION;
                    }
                    yield return new WaitForSeconds(pauseDuration);
                    break;

                case STATES.PLAYER_ACTION:
                    Debug.Log("PLAYER ACTION");
                    state = STATES.ENEMY_ROUND;
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
