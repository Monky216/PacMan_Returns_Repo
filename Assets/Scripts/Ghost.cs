using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public float moveSpeed = 9.4;
    public Node startingPosition;

    public int scatterModeTimer1 = 7;
    public int chaseModeTimer1 = 20;
    public int scatterModeTimer2 = 7;
    public int chaseModeTimer2 = 20;
    public int scatterModeTimer3 = 5;
    public int chaseModeTimer3 = 20;
    public int scatterModeTimer4 = 5;
    public int chaseModeTimer4 = 20;

    private int modeChangeIteration = 1;
    private float modeChangeTimer = 0;

    public enum Mode
    {
        Chase,
        Scatter,
        Frightened
    }

    Mode currentMode = Mode.Scatter;
    Mode previousMode;

    private GameObject pacMan;
    private Node currentNode, targetNode, previousNode;
    private Vector2 direction, nextDirection;

    //review later
    //Tutorial 9 - 21:17
    void ModeUpdate()
    {
        if (currentMode != Mode.Frightened)
        {
            modeChangeTimer += Time.deltaTime;
            if (modeChangeInteration == 1)
            {
                if (currentMode == Mode.Scatter && modeChangeTimmer > scatterModeTimer1)
                {
                    ChangeMode(Mode.Chase);
                    modeChangeTimer = 0;
                }
                if (currentNode == Mode.Chase && modeChangeTimer > scatterModeTimer1)
                {
                    modeChangeIteration = 2;
                    ChangeMode(Mode.Scatter);
                    modeChangeTimer = 0;
                }
            }
        }
    }
    
    void ChangeMode(Mode m)
    {
        currentMode = m;
    }
    
    Node GetNodeAtPosition(Vector2 pos)
    {
        GameObject tile = GameObject.Find("-- Game --").GetComponent<GameBoard>().board[(int)pos.x, (int)pos.y];
        if (tile != null)
        {
            if (tile.GetComponent<Node> != null)
            {
                return tile.GetComponent<Node>();
            }
        }
        return null;
    }
}
