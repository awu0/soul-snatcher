using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPlayer : MonoBehaviour
{
    public GameObject laserPrefab;
    public GameObject hookPrefab;
    public GameObject chainPrefab;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ShootLaser(int startX, int startY, Vector2Int direction, int length)
    {
        if (laserPrefab == null)
        {
            Debug.LogError("laserPrefab is not assigned!");
            return;
        }

        Vector3 currentPosition = new Vector3(startX, startY, 0f);
        Quaternion rotation = (direction.y != 0) // Vertical shot
            ? Quaternion.Euler(0, 0, 90)
            : Quaternion.identity; // Horizontal or default

        for (int i = 0; i < length+1; i++)
        {
            Instantiate(laserPrefab, currentPosition, rotation);
            currentPosition += new Vector3(direction.x, direction.y, -1f);
            Debug.Log($"laserPrefab created at {currentPosition}");
        }
    }

    public void ThrowHook(int startX, int startY, Vector2Int direction, int length)
    {
        if (hookPrefab == null)
        {
            Debug.LogError("hookPrefab is not assigned!");
            return;
        }
    }
}
