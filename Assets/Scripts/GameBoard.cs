using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    private static int boardWidth = 28;
    private static int boardHeight = 36;

    public GameObject[,] board = new GameObject[boardWidth, boardHeight];

    //gather all objects into the board to use later
    //like pieces put into the game box
    void Start()
    {
        Object[] objects = GameObject.FindObjectsOfType(typeof(GameObject));

        foreach (GameObject o in objects)
        {
            Vector2 pos = o.transform.position;

            if (o.name != "PacMan" && o.tag != "Pellet" && o.tag != "Ghost")
            {
                board[(int)pos.x, (int)pos.y] = o;
            }
        }
    }
}
