using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellularAutomata1d : MonoBehaviour
{

    public int[] Cells;
    int[] ruleset = new int[8];

    public CellularAutomata1d()
    {

        Cells = new int[41];
        Cells[20] = 1;

       
        for (int r = 0; r < 8; r++)
        {
            ruleset[r] = Random.Range(0, 2);
        }
        //for (int i = 0; i < Cells.Length; i++)
        //{
        //    Cells[i] = Random.Range(0, 2) == 0 ? false : true;
        //}

    }

    public void NextGeneration()
    {
        int l = Cells.Length;
        int[] temp = new int[l];

        for (int i = 0; i < l; i++)
        {
            
            temp[i] = NextCell(i);
        }

        Cells = temp;

    }

    

    int NextCell(int i)
    {
        

        int l = Cells.Length;
        int me = Cells[i];
        int left = (i == 0) ? Cells[l - 1] : Cells[i - 1];
        int right = (i == l - 1) ? Cells[0] : Cells[i + 1];

        if (left == 1 && me == 1 && right == 1) return ruleset[0];
        if (left == 1 && me == 1 && right == 0) return ruleset[1];
        if (left == 1 && me == 0 && right == 1) return ruleset[2];
        if (left == 1 && me == 0 && right == 0) return ruleset[3];
        if (left == 0 && me == 1 && right == 1) return ruleset[4];
        if (left == 0 && me == 1 && right == 0) return ruleset[5];
        if (left == 0 && me == 0 && right == 1) return ruleset[6];
        if (left == 0 && me == 0 && right == 0) return ruleset[7];

        

        return me;
    }

    
}
