using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CA_Persent : MonoBehaviour {


    public GameObject GridBox;
    public GameObject TileBox;

    public int width;
    public int height;
    public string seed;
    public bool useRandomSeed;

    [Range(0, 100)] //the range
    public int randomFillPercent; //filling the map(how much)




    int[,] map; //grid of int, any == 0 will be empty, any == 1, a tile of wall

    void Start()
    {
        GenerateMap();//call void generate map
    }

    void Update()
    {

        {
            if (Input.GetKeyDown(KeyCode.R))
                Application.LoadLevel("Scene");
        }
    }
    void GenerateMap()
    {
        map = new int[width, height]; //size cells
        RandomFillMap();
        for (int i = 0; i < 2; i++)
        {
            SmoothMap();
        }

    }

    void RandomFillMap() //work based on the seed to the same map -- string seed, bool use random seed
    {
        if (useRandomSeed)
        {
            seed = Time.time.ToString();
        }

        System.Random pseudoRandom = new System.Random(seed.GetHashCode());//int seed, seed gethashcde-random code for the seed
        for (int x = 0; x < width; x++) //start grid loop for the map
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)//or, 
                {
                    map[x, y] = 1; //0/1
                }
                else
                {
                    map[x, y] = pseudoRandom.Next(0, 100) < (randomFillPercent) ? 1 : 0; //min and max <add wall, 
                }

            }
        }

    }

    void SmoothMap() //persentage 
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighbourWallTiles = GetSurroundingWallCount(x, y);

                if (neighbourWallTiles > 4) //modify the persnt
                    map[x, y] = 1; //wall
                else if (neighbourWallTiles < 4) 
                    map[x, y] = 0; //empty
            }
        }
    }
    int GetSurroundingWallCount(int gridX, int gridY) //CA rules which tile we want the info
    {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++) //nei X  x+=y x=x+y
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++) //nei Y
            {
                if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height)//looping 3 by 3,,, safe inside, do calculate
                {
                    if (neighbourX != gridX || neighbourY != gridY)
                    {
                        wallCount += map[neighbourX, neighbourY];    //wall count in, wall count no effect
                    }


                    else
                    {
                        wallCount++; //edge
                    }
                }
            }

        }
        return wallCount;

    }

    void OnDrawGizmos()
    {

        if (map != null)
        {

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Gizmos.color = (map[x, y] == 1) ? Color.white : Color.gray;
                    Vector3 pos = new Vector3(-width / 2 + x + 0.5f, 0, -height / 2 + y + 0.5f); //position of the Giz
                    Gizmos.DrawCube(pos, Vector3.one);
                }
            }
        }
    }
}
