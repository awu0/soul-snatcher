using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitText : MonoBehaviour
{
    public float destoryTime = 1.5f;
    public Vector3 offset = new Vector3(0, 3, 0);

    private void Start()
    {
        Destroy(gameObject, destoryTime);

        transform.localPosition += offset;
    }
}
