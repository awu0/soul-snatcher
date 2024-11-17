using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackRangeIO : MonoBehaviour
{
    public Player player;
    public GameObject rangeUIPrefab;
    public GameManager manager;
    private int gridX;
    private int gridY;

    private List<GameObject> rangeIndicators = new List<GameObject>();

    private void Start()
    {
        if (player == null || rangeUIPrefab == null) return;

        Vector3 playerPosition = player.transform.position;

        rangeIndicators.Add(CreateRangeIndicator(new Vector3(-1, 0, 0) + playerPosition));
        rangeIndicators.Add(CreateRangeIndicator(new Vector3(1, 0, 0) + playerPosition));
        rangeIndicators.Add(CreateRangeIndicator(new Vector3(0, 1, 0) + playerPosition));
        rangeIndicators.Add(CreateRangeIndicator(new Vector3(0, -1, 0) + playerPosition));
    }

    private void Update()
    {
        if (player != null) {
            bool showAttackRange = player.selectedAction == Player.SELECTED.ATTACK && manager.state == GameManager.STATES.PLAYER_ROUND;
            SetRangeIndicatorActivate(showAttackRange);
        }    
    }

    private GameObject CreateRangeIndicator(Vector3 position) { 
        GameObject rangedIndicator = Instantiate(rangeUIPrefab, position, Quaternion.identity);
        rangedIndicator.transform.SetParent(player.transform);
        return rangedIndicator;
    }

    private void SetRangeIndicatorActivate(bool isActive) {
        foreach (GameObject indicator in rangeIndicators) {
            if (indicator != null) { 
                indicator.SetActive(isActive);
            }
        }
    }

}
