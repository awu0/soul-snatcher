using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPlayer : MonoBehaviour
{
    public GameObject laserPrefab;
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

        for (int i = 0; i < length; i++)
        {
            Instantiate(laserPrefab, currentPosition, Quaternion.identity);
            currentPosition += new Vector3(direction.x, direction.y, 0f);
        }
    }
}
