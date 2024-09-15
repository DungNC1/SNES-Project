using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayMenu : MonoBehaviour
{
    public void NewGameButton()
    {
        PlayerPrefs.SetString("ResetLevel", "true");
        SceneManager.LoadScene(1);
    }

    public void LoadGameButton()
    {
        if(PlayerPrefs.HasKey("LevelSaved") && PlayerPrefs.GetInt("IsLevelSaved") == 1)
        {
            PlayerPrefs.SetString("ResetLevel", "false");
            SceneManager.LoadScene(1);
            //1 = true and 0 = false
        }
    }
}
