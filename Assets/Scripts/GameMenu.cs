using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public bool isOnePlayerGame = true;
    public Text playerText1, playerText2, playerSelector;

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            if (!isOnePlayerGame)
            {
                isOnePlayerGame = true;
                playerSelector.transform.localPosition = new Vector3(playerSelector.transform.localPosition.x, playerText1.transform.localPosition.y, playerSelector.transform.localPosition.z);
            }
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            if (isOnePlayerGame)
            {
                isOnePlayerGame = false;
                playerSelector.transform.localPosition = new Vector3(playerSelector.transform.localPosition.x, playerText2.transform.localPosition.y, playerSelector.transform.localPosition.z);
            }
        }
        else if (Input.GetKeyUp(KeyCode.Return))
        {
            SceneManager.LoadScene("Level1");
        }
    }

    //tutorial 18 - 19:39
}
