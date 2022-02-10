using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nodes : MonoBehaviour
{
    public Nodes[] neighbors;
    public Vector2[] validDirections;

    void Start()
    {
        validDirections = new Vector2[neighbors.Length];

        for (int i = 0; i < neighbors.Length; i++)
        {
            //automatically set validDirections
            Nodes neighbor = neighbors[i];
            Vector2 tempVector = neighbor.transform.position - transform.position;

            validDirections[i] = tempVector.normalized;
        }
    }
}