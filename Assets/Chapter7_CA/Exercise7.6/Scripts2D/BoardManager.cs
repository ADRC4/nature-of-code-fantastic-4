using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour
{

    public GameObject mainCamera;
    public GameObject cellPrefab;
    public int rows;
    public int columns;
    public float spawnChance;
    public GameObject[,] cellMatrix;

    Transform gridHolder;

    void Awake()
    {
        cellMatrix = new GameObject[columns, rows];
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    public void SetupScene()
    {
        gridHolder = new GameObject("Grid").transform;

        mainCamera.transform.position = new Vector3(columns / 2, rows / 2, -10f);
        mainCamera.GetComponent<Camera>().orthographicSize = (columns > rows ? (columns / 2 + 1) : (rows / 2 + 1));

        Vector3 cellPosition = new Vector3(0f, 0f, 0f);
        CellScript cellScript;

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                cellPosition.x = x;
                cellPosition.y = y;
                GameObject cellInstance = Instantiate(cellPrefab, cellPosition, Quaternion.identity) as GameObject;
                cellInstance.transform.SetParent(gridHolder);

                //Set the cell alive according with the chance of "spawnChance"
                cellScript = cellInstance.GetComponent<CellScript>();
                cellScript.isAlive = (Random.value <= spawnChance ? true : false);

                cellScript.SetPosition(x, y);
                cellMatrix[x, y] = cellInstance;
            }
        }
    }
}
