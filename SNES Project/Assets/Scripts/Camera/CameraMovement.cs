using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public enum CameraState
    {
        Free,
        FollowPlayer
    }

    [SerializeField] private Camera cam;
    public CameraState cameraState;
    [SerializeField] private Transform player;

    [Header("Camera Boundaries")]
    public float minX = -10f;
    public float maxX = 10f;
    public float minY = -5f;
    public float maxY = 5f;

    private Vector3 dragOrigin;

    private void Update()
    {
        switch(cameraState)
        {
            case CameraState.Free:
                MoveCamera();
                break;
            case CameraState.FollowPlayer:
                FollowPlayer();
                break;
        }
    }

    private void MoveCamera()
    {
        if(Input.GetMouseButtonDown(2))
        {
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition); 
        }

        if (Input.GetMouseButton(2))
        {
            Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
            cam.transform.position += difference;

            cam.transform.position = new Vector3(
                Mathf.Clamp(cam.transform.position.x, minX, maxX),
                Mathf.Clamp(cam.transform.position.y, minY, maxY),
                cam.transform.position.z
            );
        }
    }

    private void FollowPlayer()
    {
        transform.position = new Vector3(player.position.x, player.position.y, -10f);
    }

    private void OnDrawGizmos()
    {
        // Set the color for the boundary box
        Gizmos.color = Color.yellow;

        // Draw a wireframe box representing the camera boundaries
        Gizmos.DrawWireCube(
            new Vector3((minX + maxX) / 2f, (minY + maxY) / 2f, 0f),
            new Vector3(maxX - minX, maxY - minY, 1f)
        );
    }
}