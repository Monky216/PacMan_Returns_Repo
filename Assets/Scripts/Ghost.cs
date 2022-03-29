using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public float moveSpeed = 9.4f;

    public Node startingPosition;
    public Node homeNode;

    public float ghostReleaseTimer = 0;
    public int pinkReleaseTimer = 5;
    public int cyanReleaseTimer = 14;
    public int orangeReleaseTimer = 21;

    public bool isInGhostHouse = false;

    private int scatterModeTimer1 = 7;
    private int chaseModeTimer1 = 20;
    private int scatterModeTimer2 = 7;
    private int chaseModeTimer2 = 20;
    private int scatterModeTimer3 = 5;
    private int chaseModeTimer3 = 20;
    private int scatterModeTimer4 = 5;

    private int modeChangeIteration = 1;
    private float modeChangeTimer = 0f;

    public enum Mode
    {
        Chase,
        Scatter,
        Frightened
    }

    Mode currentMode = Mode.Scatter;
    Mode previousMode;

    public enum GhostType
    {
        Red,
        Pink,
        Cyan,
        Orange
    }

    public GhostType ghostType;

    private GameObject pacMan;
    private Node currentNode, targetNode, previousNode;
    private Vector2 direction, nextDirection;

    public Vector2 targetTile;

    void Start()
    {
        pacMan = GameObject.FindGameObjectWithTag("Player");
        Node node = GetNodeAtPosition(transform.localPosition);
        if (node != null)
        {
            currentNode = node;
        }

        previousNode = startingPosition;

        if (isInGhostHouse)
        {
            direction = Vector2.up;
            targetNode = currentNode.neighbors[0];
        }
        else
        {
            direction = Vector2.left;
            targetNode = ChooseNextNode();
        }
    }
    
    void Update()
    {
        ModeUpdate();
        Move();
        ReleaseGhosts();
    }

    void Move()
    {
        if (targetNode != currentNode && targetNode != null && !isInGhostHouse)
        {
            if (OverShotTarget())
            {
                currentNode = targetNode;
                transform.localPosition = currentNode.transform.localPosition;
                
                GameObject otherPortal = GetPortal(currentNode.transform.localPosition);
                if (otherPortal != null)
                {
                    transform.localPosition = otherPortal.transform.localPosition;
                    currentNode = otherPortal.GetComponent<Node>();
                }
                targetNode = ChooseNextNode();
                previousNode = currentNode;
                currentNode = null;
            }
            else
            {
                transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
            }
        }
    }
    
    void ModeUpdate()
    {
        if (currentMode != Mode.Frightened)
        {
            modeChangeTimer += Time.deltaTime;

            if (modeChangeIteration == 1)
            {
                if (currentMode == Mode.Scatter && modeChangeTimer > scatterModeTimer1)
                {
                    //leaves the box
                    ChangeMode(Mode.Chase);
                    modeChangeTimer = 0;
                }
                if (currentMode == Mode.Chase && modeChangeTimer > chaseModeTimer1)
                {
                    //begins the chase
                    modeChangeIteration = 2;
                    ChangeMode(Mode.Scatter);
                    modeChangeTimer = 0;
                }
            }
            else if (modeChangeIteration == 2)
            {
                if (currentMode == Mode.Scatter && modeChangeTimer > scatterModeTimer2)
                {
                    //leaves the box
                    ChangeMode(Mode.Chase);
                    modeChangeTimer = 0;
                }
                if (currentMode == Mode.Chase && modeChangeTimer > chaseModeTimer2)
                {
                    //begins the chase
                    modeChangeIteration = 2;
                    ChangeMode(Mode.Scatter);
                    modeChangeTimer = 0;
                }
            }
            else if (modeChangeIteration == 3)
            {
                if (currentMode == Mode.Scatter && modeChangeTimer > scatterModeTimer3)
                {
                    //leaves the box
                    ChangeMode(Mode.Chase);
                    modeChangeTimer = 0;
                }
                if (currentMode == Mode.Chase && modeChangeTimer > chaseModeTimer3)
                {
                    //begins the chase
                    modeChangeIteration = 2;
                    ChangeMode(Mode.Scatter);
                    modeChangeTimer = 0;
                }
            }
            else if (modeChangeIteration == 4)
            {
                if (currentMode == Mode.Scatter && modeChangeTimer > scatterModeTimer4)
                {
                    //leaves the box
                    ChangeMode(Mode.Chase);
                    modeChangeTimer = 0;
                }
            }
        }
        else if (currentMode == Mode.Frightened)
        {

        }
    }
    
    void ChangeMode(Mode m)
    {
        currentMode = m;
    }
    
    Vector2 GetRedGhostTargetTile()
    {
        Vector2 pacManPosition = pacMan.transform.position;
        targetTile = new Vector2(Mathf.RoundToInt(pacManPosition.x), Mathf.RoundToInt(pacManPosition.y));
        
        return targetTile;
    }

    Vector2 GetPinkGhostTargetTile()
    {
        Vector2 pacManPosition = pacMan.transform.position;
        Vector2 pacManOrientation = pacMan.GetComponent<PacMan>().orientation;

        int pacManPositionX = Mathf.RoundToInt(pacManPosition.x);
        int pacManPositionY = Mathf.RoundToInt(pacManPosition.y);

        Vector2 pacManTile = new Vector2(pacManPositionX, pacManPositionY);
        Vector2 targetTile = pacManTile + (4 * pacManOrientation);

        return targetTile;
    }

    Vector2 GetCyanGhostTargetTile()
    {
        return Vector2.zero;
    }

    Vector2 GetOrangeGhostTargetTile()
    {
        return Vector2.zero;
    }

    Vector2 GetTargetTile()
    {
        Vector2 targetTile = Vector2.zero;
        if (ghostType == GhostType.Red)
        {
            targetTile = GetRedGhostTargetTile();
        }
        else if (ghostType == GhostType.Pink)
        {
            targetTile = GetPinkGhostTargetTile();
        }
        else if (ghostTyoe == GhostType.Cyan)
        {
            targetTile = GetCyanGhostTargetTile();
        }
        else if (ghostType == GhostType.Orange)
        {
            targetTile = GetOrangeGhostTargetTile();
        }
        return targetTile;
    }

    void ReleasePinkGhost()
    {
        if (ghostType == GhostType.Pink && isInGhostHouse)
        {
            isInGhostHouse = false;
        }
    }

    void ReleaseCyanGhost()
    {
        if (ghostType == GhostType.Cyan && isInGhostHouse)
        {
            isInGhostHouse = false;
        }
    }

    void ReleaseOrangeGhost()
    {
        if (ghostType == GhostType.Orange && isInGhostHouse)
        {
            isInGhostHouse = false;
        }
    }

    void ReleaseGhosts()
    {
        ghostReleaseTimer += Time.deltaTime;
        if (ghostReleaseTimer > pinkReleaseTimer)
        {
            ReleasePinkGhost();
        }
        if (ghostReleaseTimer > cyanReleaseTimer)
        {
            ReleaseCyanGhost();
        }
        if (ghostReleaseTimer > orangeReleaseTimer)
        {
            ReleaseOrangeGhost();
        }
    }

    Node ChooseNextNode()
    {
        if (currentMode == Mode.Chase)
        {
            targetTile = GetTargetTile();
        }
        else if (currentMode == Mode.Scatter)
        {
            targetTile = homeNode.transform.position;
        }

        Node moveToNode = null;
        Node[] foundNodes = new Node[4];
        Vector2[] foundNodesDirection = new Vector2[4];
        int nodeCounter = 0;

        for (int i = 0; i < currentNode.neighbors.Length; i++)
        {
            if (currentNode.validDirections [i] != direction * -1)
            {
                foundNodes[nodeCounter] = currentNode.neighbors[i];
                foundNodesDirection[nodeCounter] = currentNode.validDirections[i];
                nodeCounter++;
            }
        }
        if (foundNodes.Length == 1)
        {
            moveToNode = foundNodes[0];
            direction = foundNodesDirection[0];
        }
        if (foundNodes.Length > 1)
        {
            float leastDistance = 69420f;
            for (int i = 0; i < foundNodes.Length; i++)
            {
                if (foundNodesDirection[i] != Vector2.zero)
                {
                    float distance = GetDistance(foundNodes[i].transform.position, targetTile);
                    if (distance < leastDistance)
                    {
                        leastDistance = distance;
                        moveToNode = foundNodes[i];
                        direction = foundNodesDirection[i];
                    }
                }
            }
        }
        return moveToNode;
    }

    Node GetNodeAtPosition(Vector2 pos)
    {
        GameObject tile = GameObject.Find("-- Game --").GetComponent<GameBoard>().board[(int)pos.x, (int)pos.y];
        if (tile != null)
        {
            if (tile.GetComponent<Node>() != null)
            {
                return tile.GetComponent<Node>();
            }
        }
        return null;
    }

    GameObject GetPortal(Vector2 pos)
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

    float LengthFromNode (Vector2 targetPosition)
    {
        Vector2 vec = targetPosition - (Vector2)previousNode.transform.localPosition;
        return vec.sqrMagnitude;
    }

    bool OverShotTarget()
    {
        float nodeToTarget = LengthFromNode(targetNode.transform.localPosition);
        float nodeToSelf = LengthFromNode(transform.localPosition);
        return nodeToSelf > nodeToTarget;
    }

    float GetDistance (Vector2 posA, Vector2 posB)
    {
        float dx = posA.x - posB.x;
        float dy = posA.y - posB.y;
        float distance = Mathf.Sqrt(dx * dx + dy * dy);
        return distance;
    }
}
