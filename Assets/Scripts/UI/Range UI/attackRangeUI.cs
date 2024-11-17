using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackRangeIO : MonoBehaviour
{
    public Player player;
    public GameObject rangeUIPrefab;
    public GameManager manager;
    public Grids grid;

    private List<GameObject> rangeIndicators = new List<GameObject>();

    private void Start()
    {
        if (player == null || rangeUIPrefab == null) return;
    }

    private void Update()
    {
        if (player != null) {
            if (manager.state == GameManager.STATES.PLAYER_ROUND) {
                SetRangeIndicatorActivate(true);
                if (player.selectedAction == Player.SELECTED.ATTACK)
                {
                    if (rangeIndicators.Count == 0 || rangeIndicators.Count > 4)
                    {
                        ShowAttackRange();
                    }
                }
                else if (player.selectedAction == Player.SELECTED.ABILITY) {
                    if (player.GetComponent<SnakeBite>() != null) {
                        if (rangeIndicators.Count == 0 || rangeIndicators.Count > 4) { 
                            ShowAbilityRange();
                        }
                    }
                    else {
                        if (rangeIndicators.Count <= 4) {
                            ShowAbilityRange();
                        }    
                    }
                }
            }
            else {
                SetRangeIndicatorActivate(false);
            }
        }    
    }

    private void ShowAttackRange() { 
        ClearRangeIndicators();

        Vector3 playerPosition = player.transform.position;

        rangeIndicators.Add(CreateRangeIndicator(new Vector3(-1, 0, 0) + playerPosition));
        rangeIndicators.Add(CreateRangeIndicator(new Vector3(1, 0, 0) + playerPosition));
        rangeIndicators.Add(CreateRangeIndicator(new Vector3(0, 1, 0) + playerPosition));
        rangeIndicators.Add(CreateRangeIndicator(new Vector3(0, -1, 0) + playerPosition));
    }

    private void ShowAbilityRange() {
        ClearRangeIndicators();

        (int gridX, int gridY) = player.GetCurrentPosition();

        Vector3 playerPosition = player.transform.position;

        for (int x = 0; x < grid.columns; x++) {
            if (x != gridX) {
                Vector3 position = new Vector3(x - gridX, 0, 0) + playerPosition;
                rangeIndicators.Add(CreateRangeIndicator(position));
            }
        }

        for (int y = 0; y < grid.rows; y++) {
            if (y != gridY) {
                Vector3 position = new Vector3(0, y - gridY, 0) + playerPosition;
                rangeIndicators.Add(CreateRangeIndicator(position));
            }
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

    private void ClearRangeIndicators() { 
        foreach (GameObject indicator in rangeIndicators)
        {
            if (indicator != null) { 
                Destroy(indicator);
            }
        }
        rangeIndicators.Clear();
    }

}
