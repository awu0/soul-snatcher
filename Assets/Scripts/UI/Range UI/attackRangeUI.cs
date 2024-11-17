using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackRangeIO : MonoBehaviour
{
    public Player player;
    public GameObject rangeUIPrefab;
    private int gridX;
    private int gridY;
    private void Start()
    {
        if (player == null || rangeUIPrefab == null) return;

        Vector3 playerPosition = player.transform.position;
        (gridX, gridY) = player.GetCurrentPosition();

        GameObject rangeLeft = Instantiate(rangeUIPrefab, new Vector3(-1, 0, 0) + playerPosition, Quaternion.identity);
        rangeLeft.transform.SetParent(player.transform);
        GameObject rangeRight = Instantiate(rangeUIPrefab, new Vector3(1, 0, 0) + playerPosition, Quaternion.identity);
        rangeRight.transform.SetParent(player.transform);
        GameObject rangeUp = Instantiate(rangeUIPrefab, new Vector3(0, 1, 0) + playerPosition, Quaternion.identity);
        rangeUp.transform.SetParent(player.transform);  
        GameObject rangeDown = Instantiate(rangeUIPrefab, new Vector3(0, -1, 0) + playerPosition, Quaternion.identity);
        rangeDown.transform.SetParent(player.transform);
    }

}
