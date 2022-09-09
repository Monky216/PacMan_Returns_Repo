using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    private static int boardWidth = 28;
    private static int boardHeight = 36;

    public AudioClip backgroundAudioNormal;
    public AudioClip backgroundAudioFrightened;

    public GameObject[,] board = new GameObject[boardWidth, boardHeight];

    //gather all objects into the board to use later
    //like pieces put into the game box
    void Start()
    {
        Object[] objects = GameObject.FindObjectsOfType(typeof(GameObject));

        foreach (GameObject o in objects)
        {
            Vector2 pos = o.transform.position;

            if (o.name != "PacMan" && o.tag != "Pellet" && o.tag != "Ghost" && o.tag != "GhostHome")
            {
                board[(int)pos.x, (int)pos.y] = o;
            }
        }
    }

    public void Restart()
    {
        GameObject pacMan = GameObject.Find("PacMan");
        pacMan.GetComponent<PacMan>().Restart();

        GameObject[] o = GameObject.FindGameObjectsWithTag("Ghost");

        foreach (GameObject ghost in o)
        {
            ghost.transform.GetComponent<Ghost>().Restart();
        }
    }
}
