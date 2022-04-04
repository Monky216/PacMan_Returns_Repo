using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacMan : MonoBehaviour
{
    public Vector2 orientation;
    public float speed;
    public Sprite idleSprite;

    public AudioClip chomp1;
    public AudioClip chomp2;

    private Vector2 direction = Vector2.zero;
    private Vector2 nextDirection;

    private Node currentNode, targetNode, previousNode;

    private int pelletsConsumed = 0;

    private bool playedChomp1 = false;
    private AudioSource audio;

    void Start()
    {
        audio = transform.GetComponent<AudioSource>();
        
        Node node = GetNodeAtPosition(transform.localPosition);

        if (node != null)
        {
            currentNode = node;
        }

        direction = Vector2.left;
        orientation = Vector2.left;
        ChangePosition(direction);
    }
    
    void Update()
    {
        //in order to use named methods, you must call them in a Unity preset method
        CheckInput();
        Move();
        UpdateOrientation();
        UpdateAnimationState();

        //two pellets do not get consumed because of script delay
        //decrease PacMan's speed and it will work
        ConsumePellet();
    }

    void PlayChompSound()
    {
        if (playedChomp1)
        {
            audio.PlayOneShot(chomp2);
            playedChomp1 = false;
        }
        else
        {
            audio.PlayOneShot(chomp1);
            playedChomp1 = true;
        }
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
            //turning around between nodes
            if (nextDirection == direction * -1)
            {
                direction *= -1;

                Node tempNode = targetNode;
                targetNode = previousNode;
                previousNode = tempNode;
            }
            
            if (OverShotTarget())
            {
                //sets the game's boundaries
                currentNode = targetNode;
                transform.localPosition = currentNode.transform.position;

                GameObject otherPortal = GetPortal(currentNode.transform.position);
                if (otherPortal != null)
                {
                    transform.localPosition = otherPortal.transform.position;
                    currentNode = otherPortal.GetComponent<Node>();
                }

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
            orientation = Vector2.left;
        }

        else if (direction == Vector2.up)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 90);
            orientation = Vector2.up;
        }

        else if (direction == Vector2.right)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            orientation = Vector2.right;
        }

        else if (direction == Vector2.down)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 270);
            orientation = Vector2.down;
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

    void ConsumePellet()
    {
        GameObject c = GetTileAtPosition(transform.position);
        if (c != null)
        {
            Tile tile = c.GetComponent<Tile>();
            if (tile != null)
            {
                //if didConsume is false and either isPellet or isSuperPellet is true
                //parentheses work the same as in math
                if (!tile.didConsume && (tile.isPellet || tile.isSuperPellet))
                {
                    c.GetComponent<SpriteRenderer>().enabled = false;
                    tile.didConsume = true;
                    GameObject.Find("-- Game --").GetComponent<Pellet>().score += 1;
                    pelletsConsumed++;
                    PlayChompSound();
                }
            }
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

    GameObject GetTileAtPosition (Vector2 pos)
    {
        int tileX = Mathf.RoundToInt(pos.x);
        int tileY = Mathf.RoundToInt(pos.y);
        GameObject tile = GameObject.Find("-- Game --").GetComponent<Pellet>().board[tileX, tileY];
        if (tile != null)
        {
            return tile;
        }
        return null;
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

    GameObject GetPortal (Vector2 pos)
    {
        //checks to see if node is portal
        GameObject tile = GameObject.Find("-- Game --").GetComponent<GameBoard>().board[(int)pos.x, (int)pos.y];

        if (tile != null)
        {
            if (tile.GetComponent<Tile>() != null)
            {
                if (tile.GetComponent<Tile>().isPortal)
                {
                    GameObject otherPortal = tile.GetComponent<Tile>().portalReceiver;
                    return otherPortal;
                }
            }
        }
        return null;
    }
}