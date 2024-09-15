using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] levelText;

    public void OpenScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    private void Update()
    {
        if(PlayerPrefs.GetString("ResetLevel") == "true")
        {
            levelText[0].text = "Tutorial";
            levelText[1].text = "Level 1";
            levelText[2].text = "Level 2";
            levelText[3].text = "Level 3";
            levelText[4].text = "Level 4";
            levelText[5].text = "Level 5";
            levelText[6].text = "Level 6";
            levelText[7].text = "Level 7";
            levelText[8].text = "Level 8";
            levelText[9].text = "Level 9";
            levelText[10].text = "Level 10";
            PlayerPrefs.SetString("Tutorial", "None");
            PlayerPrefs.SetString("Level1", "None");
            PlayerPrefs.SetString("Level2", "None");
            PlayerPrefs.SetString("Level3", "None");
            PlayerPrefs.SetString("Level4", "None");
            PlayerPrefs.SetString("Level5", "None");
            PlayerPrefs.SetString("Level6", "None");
            PlayerPrefs.SetString("Level7", "None");
            PlayerPrefs.SetString("Level8", "None");
            PlayerPrefs.SetString("Level9", "None");
            PlayerPrefs.SetString("Level10", "None");
        } 
        else if(PlayerPrefs.GetString("ResetLevel") == "false")
        {
            if (PlayerPrefs.GetString("Tutorial") == "Tutorial")
            {
                levelText[0].text = "Done";
            }

            if (PlayerPrefs.GetString("Level1") == "1")
            {
                levelText[1].text = "Done";
            }

            if (PlayerPrefs.GetString("Level2") == "2")
            {
                levelText[2].text = "Done";
            }

            if (PlayerPrefs.GetString("Level3") == "3")
            {
                levelText[3].text = "Done";
            }

            if (PlayerPrefs.GetString("Level4") == "4")
            {
                levelText[4].text = "Done";
            }

            if (PlayerPrefs.GetString("Level5") == "5")
            {
                levelText[5].text = "Done";
            }

            if (PlayerPrefs.GetString("Level6") == "6")
            {
                levelText[6].text = "Done";
            }

            if (PlayerPrefs.GetString("Level7") == "7" )
            {
                levelText[7].text = "Done";
            }

            if (PlayerPrefs.GetString("Level8") == "8")
            {
                levelText[8].text = "Done";
            }

            if (PlayerPrefs.GetString("Level9") == "9")
            {
                levelText[9].text = "Done";
            }

            if (PlayerPrefs.GetString("Level10") == "10")
            {
                levelText[10].text = "Done";
            }
        }
    }
}
