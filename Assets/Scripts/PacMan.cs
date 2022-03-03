using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacMan : MonoBehaviour
{
    public float speed;
    public Sprite idleSprite;

    private Vector2 direction = Vector2.zero;
    private Vector2 nextDirection;
    private Node currentNode, targetNode, previousNode;
    
    void Start()
    {
        Node node = GetNodeAtPosition(transform.localPosition);

        if (node != null)
        {
            currentNode = node;
        }

        direction = Vector2.left;
        ChangePosition(direction);
    }
    
    void Update()
    {
        //in order to use named methods, you must call them in a Unity preset method
        
        CheckInput();
        Move();
        UpdateOrientation();
        UpdateAnimationState();
    }

    void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangePosition(Vector2.left);
        }

        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ChangePosition(Vector2.up);
        }

        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangePosition(Vector2.right);
        }

        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ChangePosition(Vector2.down);
        }
    }

    void ChangePosition(Vector2 d)
    {
        //check if the node selected is an option
        if (d != direction)
        {
            nextDirection = d;
        }

        if (currentNode != null)
        {
            Node moveToNode = CanMove(d);

            if (moveToNode != null)
            {
                //actively moving
                direction = d;
                targetNode = moveToNode;
                previousNode = currentNode;
                currentNode = null;
            }
        }
    }

    void Move()
    {
        if (targetNode != currentNode && targetNode != null)
        {
            if (OverShotTarget())
            {
                //sets the game's boundaries
                currentNode = targetNode;
                transform.localPosition = currentNode.transform.position;
                Node moveToNode = CanMove(nextDirection);
                if (moveToNode != null)
                {
                    direction = nextDirection;
                }
                if (moveToNode == null)
                {
                    moveToNode = CanMove(direction);
                }
                if (moveToNode != null)
                {
                    targetNode = moveToNode;
                    previousNode = currentNode;
                    currentNode = null;
                }
                else
                {
                    //hits wall
                    direction = Vector2.zero;
                }
            }
            else
            {
                transform.position += (Vector3)(direction * speed) * Time.deltaTime;
            }
        }
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

    void UpdateAnimationState()
    {
        if (direction == Vector2.zero)
        {
            GetComponent<Animator>().enabled = false;
            GetComponent<SpriteRenderer> ().sprite = idleSprite;
        }
        else
        {
            GetComponent<Animator>().enabled = true;
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

    bool OverShotTarget()
    {
        float nodeToTarget = LengthFromNode(targetNode.transform.position);
        float nodeToSelf = LengthFromNode(transform.localPosition);

        //a true or false statement return
            //if this statement is true, it'll return the values; else it does nothing
        return nodeToSelf > nodeToTarget;
    }

    float LengthFromNode (Vector2 targetPosition)
    {
        Vector2 vec = targetPosition - (Vector2)previousNode.transform.position;
        return vec.sqrMagnitude;
    }
}