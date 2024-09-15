using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    private void Start()
    {
        Save();
    }

    public void Save()
    {
        int savedScene = SceneManager.GetActiveScene().buildIndex;

        PlayerPrefs.SetInt("LevelSaved", savedScene);

        Debug.Log(PlayerPrefs.GetInt("LevelSaved"));
        PlayerPrefs.SetInt("IsLevelSaved", 1);
        gameObject.SetActive(false);
    }
}
