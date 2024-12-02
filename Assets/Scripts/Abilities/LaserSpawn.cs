using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSpawn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("LaserSpawn: Start method called.");
        StartCoroutine(DestroyLaserAfterTime(0.5f));
    }

    // Coroutine that waits for a specified time and then destroys the laser object
    private IEnumerator DestroyLaserAfterTime(float time)
    {
        Debug.Log("LaserSpawn: DestroyLaserAfterTime coroutine started. Waiting for " + time + " seconds.");
        yield return new WaitForSeconds(time);

        // Destroy the laser object
        Destroy(gameObject);
    }
}