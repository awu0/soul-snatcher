using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float dragSpeed = 2.0f;
    private Vector3 dragOrigin;

    public float minZoom = 5f;      
    public float maxZoom = 20f;     
    public float scrollSpeed = 10f; 

    void Update()
    {
        // Check if the right mouse button is pressed
        if (Input.GetMouseButtonDown(1))
        {
            dragOrigin = Input.mousePosition;
            return;
        }

        // If the right mouse button is held down
        if (Input.GetMouseButton(1))
        {
            Vector3 currentMousePosition = Input.mousePosition;
            Vector3 difference = dragOrigin - currentMousePosition;

            Vector3 move = new Vector3(difference.x * dragSpeed * Time.deltaTime, difference.y * dragSpeed * Time.deltaTime, 0);
            transform.position += move;

            dragOrigin = currentMousePosition;
        }

        HandleZoom();
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - scroll * scrollSpeed, minZoom, maxZoom);
    }
}
