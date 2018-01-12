using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CA_2D
{
    public bool[] Cells;

    public CA_2D()
    {
        Cells = new bool[41];
        Cells[20] = true;
    }

    public void NextGeneration()
    {
        int l = Cells.Length;
        bool[] temp = new bool[l];

        for (int i = 0; i < l; i++)
        {
            temp[i] = NextCell(i);
        }

        Cells = temp;
    }

    bool NextCell(int i)
    {
        int l = Cells.Length;
        bool cell = Cells[i];
        bool left = (i == 0) ? Cells[l - 1] : Cells[i - 1];
        bool right = (i == l - 1) ? Cells[0] : Cells[i + 1];

        if (left && right) return false;
        if (!cell && (left || right)) return true;
        if (cell && left) return false;

        return cell;
    }
}

