using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellManager : MonoBehaviour //class one the setup
{

    public float timeStep = 0.1f; //time delay : or the cell will disapear too fast

    public int mapwidth = 50, mapheight = 50;  //the size of the grid (start)
    public GameObject cellPrefab;
    public Dictionary<Vector3, GameObject> cells = new Dictionary<Vector3, GameObject>(); //with a box collider, add a floor

    void Start()
    {
        for (int x = 0; x < mapwidth; x++) //infi loof if it's --, add cell 
        {
            for (int y = 0; y < mapheight; y++) // the cell
            {
                GameObject localcell = Instantiate(cellPrefab, new Vector3(x, y), Quaternion.identity); //This quaternion corresponds to "no rotation" - the object is perfectly aligned with the world or parent axes.
                localcell.AddComponent<Cell>().cellManager = this; //this inside this myMethod it(`this`) will refer to myInstance  !!!myInstance!!!
                cells.Add(new Vector3(x, y), localcell); //add the cell(i mean remain it, or else it'll disapear right after i clik it!!!) i messed up the first time with this.
            }
        }
    }
}

public class Cell : MonoBehaviour //how does the cell behavior
{
    public CellManager cellManager;

    public bool alive; //state 0 or 1(dead or alive)
    SpriteRenderer spRend; //change the cell state
    private void Awake()
    {
        spRend = GetComponent<SpriteRenderer>();

    }

    void OnMouseDown() //press
    {
        alive = true; //press to true(alive)
        for (int i = 0; i < 2; i++) //the valume of the initial cell, 20 cell //2 to make it square 5 to make it a cross
        {
            GameObject _cell;
            if (cellManager.cells.TryGetValue(MatePosition(i), out _cell)) //detect the position of the neighbor TGetValue : Item property boolean type : 0/1 false/true.
            {
                _cell.GetComponent<Cell>().alive = true;
            }
        }
    }


    float timer; // timer like up there.
    void Update()
    {
        
        timer += Time.deltaTime;
        if (timer >= cellManager.timeStep)//the time that stay on the grid
        {


            if (!alive)
            {
                spRend.color = Color.white; //dead or alive
            }
            else
            {
                spRend.color = Color.black;//dead or alive
            }
            if (alive && GetAliveMates() < 2)  //rule of dead or alive //2
            {
                alive = false;
            }
            else if (alive && GetAliveMates() <= 3) //des wrap the cell //3
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
            timer = 0;//run time too fast
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
                    count = count + 1; //stay on the grid
                }
            }
        }
        return count;
    }
    Vector3 MatePosition(int i) //all about the neighbors, I'm using a really bad way to do this as it almost crashed my laptop.
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
            return new Vector3(0, 0, 0); //Vector3.zero
        }
    }
}




