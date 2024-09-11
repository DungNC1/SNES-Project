using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    [SerializeField] private CameraMovement cam;
    [SerializeField] private CameraMovement miniCam;
    [SerializeField] private CarController[] carControllers;
    [SerializeField] private GameObject PlayerField;
    [SerializeField] private GameObject itemManager;
    [SerializeField] private GameObject HUDUI;

    private void Start()
    {
        for (int i = 0; i < carControllers.Length; i++)
        {
            carControllers[i].enabled = false;
        }
    }

    public void StartButton()
    {
        Debug.Log("Start");
        cam.cameraState = CameraMovement.CameraState.FollowPlayer;
        miniCam.cameraState = CameraMovement.CameraState.FollowPlayer;

        for (int i = 0; i < carControllers.Length; i++)
        {
            carControllers[i].enabled = true;
        }

        PlayerField.SetActive(false);
        itemManager.SetActive(false);
        HUDUI.SetActive(false);
    }
}
