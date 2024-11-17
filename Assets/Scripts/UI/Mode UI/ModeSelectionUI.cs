using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRangeIO : MonoBehaviour
{
    public Player player;
    public GameObject rangeUIPrefab;

    private List<GameObject> rangeIndicators = new List<GameObject>();

    private void Start()
    {
        if (player == null || rangeUIPrefab == null)
        {
            Debug.LogError("Player or RangeUIPrefab is not assigned!");
            return;
        }

        Vector3 playerPosition = player.transform.position;

        // Instantiate range indicators and parent them to the player
        rangeIndicators.Add(CreateRangeIndicator(new Vector3(-1, 0, 0) + playerPosition));
        rangeIndicators.Add(CreateRangeIndicator(new Vector3(1, 0, 0) + playerPosition));
        rangeIndicators.Add(CreateRangeIndicator(new Vector3(0, 1, 0) + playerPosition));
        rangeIndicators.Add(CreateRangeIndicator(new Vector3(0, -1, 0) + playerPosition));

        // Initially hide the range indicators
        SetRangeIndicatorsActive(false);
    }

    private void Update()
    {
        if (player != null)
        {
            // Display range indicators only when the selected action is ATTACK
            bool showRange = player.selectedAction == Player.SELECTED.ATTACK;
            SetRangeIndicatorsActive(showRange);
        }
    }

    private GameObject CreateRangeIndicator(Vector3 position)
    {
        GameObject rangeIndicator = Instantiate(rangeUIPrefab, position, Quaternion.identity);
        rangeIndicator.transform.SetParent(player.transform);
        return rangeIndicator;
    }

    private void SetRangeIndicatorsActive(bool isActive)
    {
        foreach (GameObject indicator in rangeIndicators)
        {
            if (indicator != null)
            {
                indicator.SetActive(isActive);
            }
        }
    }
}
