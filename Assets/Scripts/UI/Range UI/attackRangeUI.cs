using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackRangeIO : MonoBehaviour
{
    public Player player;
    public GameObject rangeUIPrefab;
    public GameManager manager;
    public Grids grid;

    int leftCloseWall = 0;
    int rightCloseWall = 0;
    int upCloseWall = 0;
    int downCloseWall = 0;
    bool check = true;

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
                    if (check) {
                        ShowAbilityRange();
                        WallIndicatorClear();
                        check = false;
                    }
                    if (player.abilities.Peek().GetType().Name == "SnakeBite" && rangeIndicators.Count != 4) {
                        ShowAttackRange();
                    }
                    if (player.abilities.Peek().GetType().Name == "PillbugRoll" ||
                        player.abilities.Peek().GetType().Name == "EyeLaser") {
                        /**
                        if (rangeIndicators.Count == 0 || rangeIndicators.Count > 4) { 
                            ShowAbilityRange();
                            //Debug.Log("Left: " + leftCloseWall);
                            //Debug.Log("Right: " + +rightCloseWall);
                            //Debug.Log("UP: " + upCloseWall);
                            //Debug.Log("Down: " + downCloseWall);
                            WallIndicatorClear();
                        }
                    }
                    else {
                        if (rangeIndicators.Count <= 4) {
                            ShowAbilityRange();
                            //Debug.Log("Left: " + leftCloseWall);
                            //Debug.Log("Right: " + +rightCloseWall);
                            //Debug.Log("UP: " + upCloseWall);
                            //Debug.Log("Down: " + downCloseWall);
                            WallIndicatorClear();
                        }
                        **/
                        if (rangeIndicators.Count <= 4) {
                            ShowAbilityRange();
                            WallIndicatorClear();
                        }
                    }
                    if (player.abilities.Peek().GetType().Name == "Guard") {
                      SetRangeIndicatorActivate(false);
                    }
                }
                else if (player.selectedAction == Player.SELECTED.RECENT_TRANSFORM) {
                  SetRangeIndicatorActivate(false);
                }
            }
            else {
                SetRangeIndicatorActivate(false);
                check = true;
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

        bool rightMostWallGap = true;
        bool upMostWallGap = true;

        for (int x = 0; x < grid.columns; x++) {
            if (x != gridX) {
                Vector3 position = new Vector3(x - gridX, 0, 0) + playerPosition;
                int positionX = (int)position.x;
                int positionY = (int)position.y;
                if (grid.IsWall(positionX, positionY)) { 
                    int gap = gridX - positionX;
                    if (gap > 0)
                    {
                        leftCloseWall = positionX;
                    }
                    if (gap < 0) {
                        if (rightMostWallGap) { 
                            rightCloseWall = positionX;
                            rightMostWallGap = false;
                        } 
                    }
                }
                rangeIndicators.Add(CreateRangeIndicator(position));
            }
        }

        for (int y = 0; y < grid.rows; y++) {
            if (y != gridY) {
                Vector3 position = new Vector3(0, y - gridY, 0) + playerPosition;
                int positionX = (int)position.x;
                int positionY = (int)position.y;
                if (grid.IsWall(positionX, positionY)) { 
                    int gap = gridY - positionY;
                    if (gap > 0) {
                        downCloseWall = positionY;
                    }
                    if (gap < 0) {
                        if (upMostWallGap) { 
                            upCloseWall = positionY;
                            upMostWallGap = false;
                        }
                    }
                }
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

    private void WallIndicatorClear()
    {
        float tolerance = 0.1f;

        foreach (GameObject indicator in rangeIndicators)
        {
            Vector3 position = indicator.transform.position;

            //Debug.Log($"Indicator Position: {position}, Left: {leftCloseWall}, Right: {rightCloseWall}, Up: {upCloseWall}, Down: {downCloseWall}");

            if (position.x < leftCloseWall - tolerance ||
                position.x > rightCloseWall + tolerance ||
                position.y < downCloseWall - tolerance ||
                position.y > upCloseWall + tolerance)
            {
                if (indicator != null)
                {
                    //Debug.Log($"Deactivating Indicator at {position}");
                    Destroy(indicator);
                }
            }
        }
    }
}
