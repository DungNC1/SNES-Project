using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    [SerializeField] private CameraMovement cam;
    [SerializeField] private CarController[] carControllers;
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject[] itemButtons;

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


        for (int i = 0; i < carControllers.Length; i++)
        {
            carControllers[i].enabled = true;
        }
        
        startButton.SetActive(false);
    }
}