using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject playMenu;

    public void PlayButton()
    {
        playMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
