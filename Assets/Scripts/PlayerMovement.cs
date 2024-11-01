using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float stepPause = 0.3f;

    public GameObject grids;
    public List<GameObject> path;
    void Update()
    {
        if (grids.GetComponent<GridBehavior>().findDistance) {
            path = grids.GetComponent<GridBehavior>().path;
            if (path.Count != 0) {
                StartCoroutine(MoveAlongPath());
            }
        }
    }

    IEnumerator MoveAlongPath()
    {
        for (int i = path.Count - 1; i > -1 ; i--) { 
            UnityEngine.Vector3 targetPosition = path[i].transform.position;
            transform.position = targetPosition;
            yield return new WaitForSeconds(stepPause);
        }
    }
}
