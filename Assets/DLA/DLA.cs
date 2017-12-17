using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DLA
{
    public int[,] Cells;
    public int GenerationCount = 0;
    private int _width;
    private int _height;

    public DLA(int width, int height)
    {
        _width = width;
        _height = height;

        Cells = new int[_width, _height];

        for (int y = 0; y < _height; y++)
            for (int x = 0; x < _width; x++)
            {
                bool r = Random.value < 0.01f;
                if (r) Cells[x, y] = 1;
            }

        Cells[_width / 2, _height / 2] = -1;
    }

    public void NextGeneration()
    {
        var temp = new int[_width, _height];

        for (int y = 0; y < _height; y++)
            for (int x = 0; x < _width; x++)
            {
                int count = Cells[x, y];

                if (count < 0)
                {
                    temp[x, y] = count - 1;
                }
                else if (count > 0)
                {
                    if (HasStaticNeighbour(x, y))
                    {
                        temp[x, y] = -1;
                    }
                    else
                    {
                        for (int i = 0; i < count; i++)
                        {
                            int nx = x + Random.Range(-1, 2);
                            int ny = y + Random.Range(-1, 2);

                            nx = Wrap(nx, _width);
                            ny = Wrap(ny, _height);

                            temp[nx, ny] += 1;
                        }
                    }
                }
            }

        Cells = temp;
        GenerationCount++;
    }

    int Wrap(int i, int size)
    {
        if (i < 0) return size - 1;
        if (i > size - 1) return 0;
        return i;
    }

    bool HasStaticNeighbour(int x, int y)
    {
        for (int xi = -1; xi < 2; xi++)
            for (int yi = -1; yi < 2; yi++)
            {
                if (xi == 0 && yi == 0) continue;

                int nx = x + xi;
                int ny = y + yi;

                nx = Wrap(nx, _width);
                ny = Wrap(ny, _height);

                int count = Cells[nx, ny];
                if (count < 0) return true;
            }

        return false;
    }
}