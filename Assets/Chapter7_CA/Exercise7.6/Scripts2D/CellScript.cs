using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CellScript : MonoBehaviour
{

    public bool isAlive;
    public Sprite aliveSprite;
    public Sprite deadSprite;

    private BoardManager boardScript;
    private GameObject[,] cellMatrix;
    private SpriteRenderer spriteRenderer;

    private int aliveNeighbours;
    private int x;
    private int y;
    private bool isSpriteSet;

    void Awake()
    {
        boardScript = GameManager.instance.GetComponent<BoardManager>();
        cellMatrix = boardScript.cellMatrix;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        isSpriteSet = false;
    }

    void Update()
    {
        if (!isSpriteSet)
        {
            if (isAlive)
            {
                spriteRenderer.sprite = aliveSprite;
            }
            else
            {
                spriteRenderer.sprite = deadSprite;
            }
            isSpriteSet = true;
        }
        
    }

    public void ScanNeighbours()
    {
        aliveNeighbours = 0;

        for (int i = x - 1; i <= x + 1; i++)
        {
            for (int j = y - 1; j <= y + 1; j++)
            {
                if (i == x && j == y) continue;
                if (i >= 0 && i < boardScript.columns && j >= 0 && j < boardScript.rows)
                {
                    if (cellMatrix[i, j].GetComponent<CellScript>().isAlive)
                        aliveNeighbours++;
                }
            }
        }
    }

    public void Determine()
    {
        if (isAlive && (aliveNeighbours < 2 || aliveNeighbours > 3))
        {
            isAlive = false;
        }
        else if (isAlive && (aliveNeighbours == 2 || aliveNeighbours == 3))
        {
            isAlive = true;
        }
        else if (!isAlive && aliveNeighbours == 3)
        {
            isAlive = true;
        }
        isSpriteSet = false;
    }

    public void SetPosition(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}
