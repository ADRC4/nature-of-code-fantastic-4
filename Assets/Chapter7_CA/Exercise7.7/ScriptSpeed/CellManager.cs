using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellManager : MonoBehaviour
{

    public float timeStep = 0.1f;

    public int mapwidth = 50, mapheight = 50;
    public GameObject cellPrefab;
    public Dictionary<Vector3, GameObject> cells = new Dictionary<Vector3, GameObject>();

    void Start()
    {
        for (int x = 0; x < mapwidth; x++)
        {
            for (int y = 0; y < mapheight; y++)
            {
                GameObject localcell = Instantiate(cellPrefab, new Vector3(x, y), Quaternion.identity);
                localcell.AddComponent<Cell>().cellManager = this;
                cells.Add(new Vector3(x, y), localcell);
            }
        }
    }
}

public class Cell : MonoBehaviour
{
    public CellManager cellManager;

    public bool alive;
    SpriteRenderer spRend;
    private void Awake()
    {
        spRend = GetComponent<SpriteRenderer>();

    }

    void OnMouseDown()
    {
        alive = true;
        for (int i = 0; i < 2; i++)
        {
            GameObject _cell;
            if (cellManager.cells.TryGetValue(MatePosition(i), out _cell))
            {
                _cell.GetComponent<Cell>().alive = true;
            }
        }
    }


    float timer;
    void Update()
    {
        
        timer += Time.deltaTime;
        if (timer >= cellManager.timeStep)
        {


            if (!alive)
            {
                spRend.color = Color.white;
            }
            else
            {
                spRend.color = Color.black;
            }
            if (alive && GetAliveMates() < 2)
            {
                alive = false;
            }
            else if (alive && GetAliveMates() <= 3)
            {
                alive = true;
            }
            else if (alive && GetAliveMates() > 3)
            {
                alive = false;
            }

            if (!alive && GetAliveMates() == 3)
            {
                alive = true;
            }
            timer = 0;
        }
    }

    GameObject cell;
    public int GetAliveMates()
    {
        int count = 0;
        for (int i = 0; i < 8; i++)
        {
            if (cellManager.cells.TryGetValue(MatePosition(i), out cell))
            {
                if (cell.GetComponent<Cell>().alive)
                {
                    count += 1;
                }
            }
        }
        return count;
    }
    Vector3 MatePosition(int i)
    {
        if (i == 0)
        {
            return transform.position + new Vector3(1, 0, 0);

        }
        else if (i == 1)
        {
            return transform.position + new Vector3(1, -1, 0);
        }
        else if (i == 2)
        {
            return transform.position + new Vector3(0, -1, 0);
        }
        else if (i == 3)
        {
            return transform.position + new Vector3(-1, -1, 0);
        }
        else if (i == 4)
        {
            return transform.position + new Vector3(-1, 0, 0);
        }
        else if (i == 5)
        {
            return transform.position + new Vector3(-1, 1, 0);
        }
        else if (i == 6)
        {
            return transform.position + new Vector3(0, 1, 0);
        }
        else if (i == 7)
        {
            return transform.position + new Vector3(1, 1, 0);
        }
        else
        {
            return Vector3.zero;
        }
    }
}


