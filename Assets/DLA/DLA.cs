using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

public class DLA
{
    public int[,] Cells;
    public int GenerationCount = 0;

    private readonly Random random = new Random(42);
    private readonly int _width;
    private readonly int _height;
    private readonly int[] _randomValues;
    private int _randomIndex = 0;

    public DLA(int width, int height)
    {
        _width = width;
        _height = height;

        Cells = new int[_width, _height];

        for (int y = 0; y < _height; y++)
            for (int x = 0; x < _width; x++)
            {
                bool r = random.NextDouble() < 0.01;
                if (r) Cells[x, y] = 1;
            }

        Cells[_width / 2, _height / 2] = -1;

        int size = _width * _height * 2;
        _randomValues = new int[size];

        for (int i = 0; i < size; i++)
        {
            _randomValues[i] = random.Next(-1, 2);
        }
    }

    public void NextGeneration()
    {
        var temp = new int[_width, _height];
        int offset = random.Next(0, _randomValues.Length / 2);

        Parallel.For(0, _height, y =>
         {
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
                         int index = x + y * _width + offset;

                         if (index + count * 2 > _randomValues.Length - 1)                
                             index = index + count * 2 - _randomValues.Length;
                         
                         for (int i = 0; i < count; i++)
                         {
                             int nx = x + _randomValues[index + i];
                             int ny = y + _randomValues[index + i + 1];

                             nx = Wrap(nx, _width);
                             ny = Wrap(ny, _height);

                             temp[nx, ny] += 1;
                         }
                     }
                 }
             }
         });

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