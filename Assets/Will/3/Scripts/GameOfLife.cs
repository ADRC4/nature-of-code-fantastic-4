using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameOfLife : MonoBehaviour
{
    //public Button startStopButton;     
    public int gridSize;                
    public float updateDelay;           
    public GameObject cellPrefab;

    GameObject[,] cells; 
    bool[,] cellState;         
    bool update;               
    bool updateOnlyOnes;        
    GameObject cellParent;      
    Vector3 dragOrigin;         
    float dragSpeed = 2;        


    void Start ()
    {
        
        InstantiateCells(); //create all the cells.

        
        StartCoroutine("UpdateCells"); //start the update function.
	}
	
    void Update()
    {
       
        Camera.main.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * gridSize;

        
        if (Camera.main.orthographicSize < 1)
            Camera.main.orthographicSize = 1;

        
        if (Input.GetMouseButton(0) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
        {
            //find the mouse position in the screen.
            Vector3 pos = Input.mousePosition;
            pos.z = 20;
            pos = Camera.main.ScreenToWorldPoint(pos);

            int _x = Mathf.FloorToInt(+.5f + pos.x);
            int _y = Mathf.FloorToInt(+.5f + pos.y);

            //check if the position that was calculated is inside of the arrays.
            if (_x > -1 && _x < gridSize)
            {
                if (_y > -1 && _y < gridSize)
                {
                    //set cell state to off (white).
                    cellState[_x, _y] = false;

                    //redraw all of the cells.
                    RedrawCells();
                }
            }
        }
        //if only the left mouse button is clicked.
        else if (Input.GetMouseButton(0))
        {
            //find the mouse position in the screen.
            Vector3 pos = Input.mousePosition;
            pos.z = 20;
            pos = Camera.main.ScreenToWorldPoint(pos);


           
            int _x = Mathf.FloorToInt(+.5f + pos.x);
            int _y = Mathf.FloorToInt(+.5f + pos.y);

            //check if the position that was calculated is inside of the arrays.
            if (_x > -1 && _x < gridSize)
            {
                if (_y > -1 && _y < gridSize)
                {
                    //set cell state to on (black).
                    cellState[_x, _y] = true;

                    //redraw all of the cells.
                    RedrawCells();
                }
            }
        }
    }

    void LateUpdate()
    {
        // the right mouse button is clicked.
        if (Input.GetMouseButtonDown(1))
        {
            //reset the drag origin position.
            dragOrigin = Input.mousePosition;
            return;
        }

        //if the right mouse is not being used don't read the rest of the code.
        if (!Input.GetMouseButton(1)) return;

        //get the mouse position.
        Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        //get the direction the camera need to move.
        Vector3 move = new Vector3(pos.x * dragSpeed, pos.y * dragSpeed, 0);

        //move the camera.
        transform.Translate(move, Space.World);
        //transform.Translate(move, );
    }

    void InstantiateCells()
    {
        
        cellState = new bool[gridSize, gridSize];
        cells = new GameObject[gridSize, gridSize];

        
        if (cellParent)
            Destroy(cellParent);

        
        cellParent = new GameObject("root");

        
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                //create a new cell and put it into the array containing all the cell gameobjects.
                cells[x, y] = Instantiate(cellPrefab, new Vector3(x, y, 0), Quaternion.identity) as GameObject;

                //set the cell parnet to "cellParent".
                cells[x, y].transform.parent = cellParent.transform;
            }
        }

       
        Camera.main.gameObject.transform.position = new Vector3(gridSize/2, gridSize/2, -1);
        
        Camera.main.gameObject.GetComponent<Camera>().orthographicSize = gridSize;
    }
    
    IEnumerator UpdateCells()
    {
       //loop
        while (true)
        {
            
            yield return new WaitForSeconds(updateDelay);

           
            if (update)
            {
                //create array for the new cell state.
                bool[,] _tmp = new bool[gridSize, gridSize];

                //fill the array with the current cell state.
                for (int x = 0; x < gridSize; x++)
                {
                    for (int y = 0; y < gridSize; y++)
                    {
                        _tmp[x, y] = cellState[x, y];
                    }
                }

                //go through the cell state array.
                for (int x = 0; x < gridSize; x++)
                {
                    for (int y = 0; y < gridSize; y++)
                    {
                        //get the current state.
                        bool on = cellState[x, y];
                        int neighbors = Neighbors(x, y);

                        //if the state is on (black).
                        if (on)
                        {
                            //if the cell have less than 2 neighbors set its state to off.
                            if (neighbors < 2)
                            {
                                on = false;
                            }
                            //if the cell have 2 or 3 neighbors set its state to on.
                            else if (neighbors == 2 || neighbors == 3)
                            {
                                on = true;
                            }
                            //if the cell have more than 3 neighbors set its state to off.
                            else if (neighbors > 3)
                            {
                                on = false;
                            }
                        }
                        //if the state is off (white).
                        else
                        {
                            //if the cell have 3 neighbors set its state to on.
                            if (neighbors == 3)
                            {
                                on = true;
                            }
                        }

                        //update the new state in the temporary array.
                        _tmp[x, y] = on;
                    }
                }

                //replace the current cell array with the temporary array.
                cellState = _tmp;

                //redraw the cells.
                RedrawCells();

                //if the update function only needs to run ones.
                if (updateOnlyOnes)
                {
                    //stop the update function.
                    updateOnlyOnes = false;
                    StopUpdate();
                }
            }
        }
    }

    void RedrawCells()
    {
        
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                //if the cell state os off.
                if (!cellState[x, y])
                {
                    //set the cell color to white.
                    cells[x, y].GetComponent<SpriteRenderer>().color = Color.white;
                }
                //if the cell state is on.
                else
                {
                    //set the cell color to black.
                    cells[x, y].GetComponent<SpriteRenderer>().color = Color.black;
                }
            }
        }
    }

    private int Neighbors(int _x, int _y)
    {
        int count = 0;

        //check cell on the right.
        if (_x != gridSize - 1)
            if (cellState[_x + 1, _y])
                count++;

        //check cell on the bottom right.
        if (_x != gridSize - 1 && _y != gridSize - 1)
            if (cellState[_x + 1, _y + 1])
                count++;

        //check cell on the bottom.
        if (_y != gridSize - 1)
            if (cellState[_x, _y + 1])
                count++;

        //check cell on the bottom left.
        if (_x != 0 && _y != gridSize - 1)
            if (cellState[_x - 1, _y + 1])
                count++;

        //check cell on the left.
        if (_x != 0)
            if (cellState[_x - 1, _y])
                count++;

        //check cell on the top left.
        if (_x != 0 && _y != 0)
            if (cellState[_x - 1, _y - 1])
                count++;

        //check cell on the top.
        if (_y != 0)
            if (cellState[_x, _y - 1])
                count++;

        //check cell on the top right.
        if (_x != gridSize - 1 && _y != 0)
            if (cellState[_x + 1, _y - 1])
                count++;

        //return the amount of neighbors of the cell
        return count;
    }

    private void StartUpdate()
    {
        //change the start stop button text
        //startStopButton.transform.Find("Text").GetComponent<Text>().text = "Stop";

        //start the update function
        update = true;
    }
    private void StopUpdate()
    {
        //change the start stop button text
        //startStopButton.transform.Find("Text").GetComponent<Text>().text = "Start";

        //stop the update function
        update = false;
    }

    //functions called by the UI
    public void UIStartStop()
    {
        //if update is true set it to false. If update is false set it to true
        update = !update;
        if (update)
            StartUpdate();
        else
            StopUpdate();
    }
    public void UINext()
    {
        //start the update function
        StartUpdate();

        //make the update function onyl run ones
        updateOnlyOnes = true;
    }
    public void UIClear()
    {
        //stop the update function
        StopUpdate();
        InstantiateCells();
    }
    public void UIE1()
    {
        //stop the update function
        StopUpdate();

        //fill the grid with example 1
        gridSize = 90;
        InstantiateCells();
        for (int i = 0; i < 50; i++)
            cellState[20 + i, 45] = true;
        RedrawCells();
    }
    public void UIE2()
    {
        //stop the update function
        StopUpdate();

        //fill the grid with example 2
        gridSize = 100;
        InstantiateCells();
        int _x = 14;
        int _y = 70;
        cellState[_x + 0, _y + 3] = true;
        cellState[_x + 0, _y + 4] = true;
        cellState[_x + 1, _y + 3] = true;
        cellState[_x + 1, _y + 4] = true;
        cellState[_x + 3, _y + 4] = true;
        cellState[_x + 6, _y + 4] = true;
        cellState[_x + 5, _y + 5] = true;
        cellState[_x + 4, _y + 5] = true;
        cellState[_x + 5, _y + 3] = true;
        cellState[_x + 4, _y + 3] = true;
        cellState[_x + 9, _y + 4] = true;
        cellState[_x + 9, _y + 6] = true;
        cellState[_x + 9, _y + 7] = true;
        cellState[_x + 10, _y + 7] = true;
        cellState[_x + 11, _y + 6] = true;
        cellState[_x + 12, _y + 5] = true;
        cellState[_x + 9, _y + 2] = true;
        cellState[_x + 9, _y + 1] = true;
        cellState[_x + 10, _y + 1] = true;
        cellState[_x + 11, _y + 2] = true;
        cellState[_x + 12, _y + 3] = true;
        cellState[_x + 12, _y + 4] = true;
        cellState[_x + 26, _y + 4] = true;
        cellState[_x + 26, _y + 5] = true;
        cellState[_x + 26, _y + 6] = true;
        cellState[_x + 26, _y + 7] = true;
        cellState[_x + 26, _y + 8] = true;
        cellState[_x + 27, _y + 9] = true;
        cellState[_x + 27, _y + 3] = true;
        cellState[_x + 28, _y + 8] = true;
        cellState[_x + 28, _y + 4] = true;
        cellState[_x + 29, _y + 5] = true;
        cellState[_x + 29, _y + 6] = true;
        cellState[_x + 29, _y + 7] = true;
        cellState[_x + 30, _y + 6] = true;
        cellState[_x + 27, _y + 5] = true;
        cellState[_x + 27, _y + 6] = true;
        cellState[_x + 27, _y + 7] = true;
        cellState[_x + 34, _y + 6] = true;
        cellState[_x + 35, _y + 6] = true;
        cellState[_x + 34, _y + 5] = true;
        cellState[_x + 35, _y + 5] = true;
        RedrawCells();
    }
    public void UIFast()
    {
        //set update dalay to 0 (fast)
        updateDelay = 0;
    }
    public void UINormal()
    {
        //set update dalay to 0.04 (normal)
        updateDelay = 0.04f;
    }
    public void UISlow()
    {
        //set update dalay to 0.1 (fast)
        updateDelay = 0.1f;
    }
    public void UIGridSize(int value)
    {
        //update grid size
        gridSize = value;

        //recreate all the cells
        InstantiateCells();
    }
}
