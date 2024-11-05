using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public EnemyManager enemyManager;

    private void Start()
    {
        StartCoroutine(RunTurnManager());
        
        player.MoveTo(_playerStartX, _playerStartY);
        
        enemyManager.SpawnEnemy<WeaklingEnemy>(1, 9);
        enemyManager.SpawnEnemy<WeaklingEnemy>(9, 1);
        enemyManager.SpawnEnemy<GiantPillbug>(9, 9);
    }

    private IEnumerator RunTurnManager()
    {
        while (true)
        {
            switch (state)
            {
                case STATES.ROUND_START:
                    //refill action count
                    player.actionCount = player.maxActionCount;
                    Debug.Log("ROUND START");
                    state = STATES.PLAYER_ROUND;
                    break;

                case STATES.PLAYER_ROUND:
                    Debug.Log("PLAYER ROUND");
                    //go to next round if player can't action anymore
                    if (player != null)
                    {
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

                    foreach (var enemy in enemyManager.enemies)
                    {
                        enemy.DetermineNextMove();
                        yield return new WaitForSeconds(pauseDuration);
                    }
                    
                    state = STATES.ROUND_END;
                    break;

                case STATES.ROUND_END:
                    Debug.Log("ROUND END");
                    state = STATES.ROUND_START;
                    break;
            }
        }
    }
}
