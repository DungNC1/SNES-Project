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
    [SerializeField] private CameraState cameraState;
    [SerializeField] private Transform player;

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
        if(Input.GetMouseButtonDown(0))
        {
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition); 
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
            cam.transform.position += difference;
        }
    }

    private void FollowPlayer()
    {
        transform.position = new Vector3(player.position.x, player.position.y, -10f);
    }
}