using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public enum STATES
    {
        ROUND_START,
        PLAYER_ROUND,
        PLAYER_ACTION,
        ENEMY_ROUND,
        ROUND_END,
    }
    public GameObject Player;
    public STATES state = STATES.ROUND_START;
    public float pauseDuration = 1f;

    public EnemyManager enemyManager;

    private void Start()
    {
        StartCoroutine(RunTurnManager());
        
        enemyManager.SpawnEnemy<WeaklingEnemy>(1, 9);
        enemyManager.SpawnEnemy<WeaklingEnemy>(9, 1);
        enemyManager.SpawnEnemy<WeaklingEnemy>(9, 9);
    }

    private IEnumerator RunTurnManager()
    {
        while (true)
        {
            switch (state)
            {
                case STATES.ROUND_START:
                    //refill action count
                    Player.GetComponent<PlayerMovement>().actionCount = Player.GetComponent<PlayerMovement>().maxActionCount;
                    Debug.Log("ROUND START");
                    state = STATES.PLAYER_ROUND;
                    yield return new WaitForSeconds(pauseDuration);
                    break;

                case STATES.PLAYER_ROUND:
                    Debug.Log("PLAYER ROUND");
                    //go to next round if player can't action anymore
                    if (Player != null)
                    {
                        if (Player.GetComponent<PlayerMovement>().actionCount <= 0)
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
                    yield return new WaitForSeconds(pauseDuration);
                    break;

                case STATES.ROUND_END:
                    Debug.Log("ROUND END");
                    state = STATES.ROUND_START;
                    yield return new WaitForSeconds(pauseDuration);
                    break;
            }
        }
    }
}
