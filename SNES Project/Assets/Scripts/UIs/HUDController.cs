using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUDController : MonoBehaviour
{
    //[SerializeField] private CameraMovement cam;
    [SerializeField] private CameraMovement miniCam;
    [SerializeField] private CarController[] carControllers;
    [SerializeField] private GameObject itemManager;
    [SerializeField] private GameObject HUDUI;
    [SerializeField] private string levelIndex;

    private void Start()
    {
        for (int i = 0; i < carControllers.Length; i++)
        {
            carControllers[i].enabled = false;
        }

        PlayerPrefs.SetString(levelIndex, (SceneManager.GetActiveScene().buildIndex - 2).ToString());
    }

    public void StartButton()
    {
        Debug.Log("Start");
        //cam.cameraState = CameraMovement.CameraState.FollowPlayer;
        miniCam.cameraState = CameraMovement.CameraState.FollowPlayer;

        for (int i = 0; i < carControllers.Length; i++)
        {
            carControllers[i].enabled = true;
        }

        itemManager.SetActive(true);
        HUDUI.SetActive(false);
    }
}
