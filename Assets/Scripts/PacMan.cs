using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacMan : MonoBehaviour
{
    public float speed;

    private Vector2 direction = Vector2.left;
    private Node currentNode, moveToNode, targetNode;
    
    void Start()
    {
        Node node = GetNodeAtPosition(transform.localPosition);

        if (node != null)
        {
            currentNode = node;
        }
    }
    
    void Update()
    {
        //in order to use named methods, you must call them in a Unity preset method
        
        CheckInput();
        //Move();
        UpdateOrientation();
    }

    void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            direction = Vector2.left;
            MoveToNode(direction);
        }

        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            direction = Vector2.up;
            MoveToNode(direction);
        }

        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            direction = Vector2.right;
            MoveToNode(direction);
        }

        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            direction = Vector2.down;
            MoveToNode(direction);
        }
    }

    void Move()
    {
        //with a Vector2, both x and y will be multiplied by speed and time
        //d is already a Vector2 of direction
        transform.position += (Vector3)(direction * speed) * Time.deltaTime;
    }

    void MoveToNode (Vector2 d)
    {
        Node moveToNode = CanMove(d);

        if (moveToNode != null)
        {
            transform.position = moveToNode.transform.position;
            currentNode = moveToNode;
        }
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

    Node GetNodeAtPosition (Vector2 pos)
    {
        GameObject tile = GameObject.Find("-- Game --").GetComponent<GameBoard>().board[(int)pos.x, (int)pos.y];

        if (tile != null)
        {
            return tile.GetComponent<Node>();
        }
        return null;
    }

    Node CanMove (Vector2 d)
    {
        //check to see if the direction the player inputed is valid
        Node moveToNode = null;

        for (int i = 0; i < currentNode.neighbors.Length; i++)
        {
            if (currentNode.validDirections [i] == d)
            {
                moveToNode = currentNode.neighbors[i];
                break;
            }
        }
        return moveToNode;
    }
}