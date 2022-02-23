using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacMan : MonoBehaviour
{
    public float speed;

    private Vector2 direction = Vector2.left;
    private Nodes currentNode;
    
    void Start()
    {
        Nodes node = GetNodeAtPosition(transform.localPosition);

        if (node != null)
        {
            currentNode = node;
            Debug.Log(currentNode);
        }
    }
    
    void Update()
    {
        //in order to use named methods, you must call them in a Unity preset method
        CheckInput();
        Move();
        UpdateOrientation();
    }

    void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            direction = Vector2.left;
        }

        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            direction = Vector2.up;
        }

        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            direction = Vector2.right;
        }

        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            direction = Vector2.down;
        }
    }

    void Move()
    {
        //with a Vector2, both x and y will be multiplied by speed and time
        //d is already a Vector2 of direction
        transform.position += (Vector3)(direction * speed) * Time.deltaTime;
    }

    void UpdateOrientation()
    {
        if (direction == Vector2.left)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 180);
        }

        else if (direction == Vector2.up)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 90);
        }

        else if (direction == Vector2.right)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        else if (direction == Vector2.down)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 270);
        }
    }

    Nodes GetNodeAtPosition (Vector2 pos)
    {
        GameObject tile = GameObject.Find("-- Game --").GetComponent<GameBoard>().board[(int)pos.x, (int)pos.y];

        if (tile != null)
        {
            return tile.GetComponent<Nodes>();
        }

        return null;
    }
}