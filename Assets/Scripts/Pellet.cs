using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pellet : MonoBehaviour
{
    private static int boardWidth = 28;
    private static int boardHeight = 36;

    public GameObject[,] board = new GameObject[boardWidth, boardHeight];

    void Start()
    {
        Object[] objects = GameObject.FindObjectsOfType(typeof(GameObject));

        foreach (GameObject o in objects)
        {
            Vector2 pos = o.transform.position;

            if (o.tag == "Pellet")
            {
                board[(int)pos.x, (int)pos.y] = o;
            }
        }
    }
}
