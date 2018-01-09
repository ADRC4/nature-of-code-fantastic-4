using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    BoardManager boardScript;
    float time;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        boardScript = GetComponent<BoardManager>();
    }

    void Start()
    {
        boardScript.SetupScene();
    }

    void Update()
    {
        NextGeneration();
    }

    void NextGeneration()
    {
        for (int i = 0; i < boardScript.columns; i++)
        {
            for (int j = 0; j < boardScript.rows; j++)
            {
                boardScript.cellMatrix[i, j].GetComponent<CellScript>().ScanNeighbours();
            }
        }
        for (int i = 0; i < boardScript.columns; i++)
        {
            for (int j = 0; j < boardScript.rows; j++)
            {
                boardScript.cellMatrix[i, j].GetComponent<CellScript>().Determine();
            }
        }
    }
}
