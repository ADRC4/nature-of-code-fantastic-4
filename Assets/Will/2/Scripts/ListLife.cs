using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ListLife : MonoBehaviour
{

    /**
    Helper structures
    */
    public class Cell
    {
        public int x;
        public int y;
        public int state;

        public Cell(int x, int y, int state)
        {
            this.x = x;
            this.y = y;
            this.state = state;
        }
    }

    public class Neighbours
    {
        public List<Cell> list;

        public Neighbours(List<Cell> list)
        {
            this.list = list;
        }

        public Neighbours add(int x, int y, int state)
        {
            list.Add(new Cell(x, y, state));
            return this;
        }

        public Cell getCell(int index)
        {
            return list[index];
        }
    }

    public GameManager gameManager;

    List<List<int>> actualState;
    List<Cell> redrawList;
    int topPointer, middlePointer, bottomPointer;

    void Initialize()
    {
        actualState = new List<List<int>>();
        redrawList = new List<Cell>();
        topPointer = middlePointer = bottomPointer = 1;
    }

    int NextGeneration()
    {
        int x, y, n, t1, t2, alive = 0;
        int neighbours;

        Cell key;
        Neighbours deadNeighbours = new Neighbours(new List<Cell>());
        Dictionary<Cell, int> allDeadNeighbours = new Dictionary<Cell, int>();
        List<List<int>> newState = new List<List<int>>();

        for (int i = 0; i < actualState.Count; i++)
        {
            topPointer = 1;
            bottomPointer = 1;
            for (int j = 1; j < actualState[i].Count; j++)
            {
                x = actualState[i][j];
                y = actualState[i][0];

                deadNeighbours.add(x - 1, y - 1, 1)
                    .add(x, y - 1, 1)
                    .add(x + 1, y - 1, 1)
                    .add(x - 1, y, 1)
                    .add(x + 1, y, 1)
                    .add(x - 1, y + 1, 1)
                    .add(x, y + 1, 1)
                    .add(x + 1, y + 1, 1);

                neighbours = GetNeighboursFromAlive(x, y, i, deadNeighbours);

                for (int m = 0; m < 8; m++)
                {
                    if (true) //TODO: if (deadNeighbours[m] !== undefined)
                    {
                        key = new Cell(deadNeighbours.getCell(m).x, deadNeighbours.getCell(m).y, 1);

                        if (!allDeadNeighbours.ContainsKey(key))
                        {
                            allDeadNeighbours.Add(key, 1);
                        }
                        else
                        {
                            allDeadNeighbours[key]++;
                        }
                    }
                }

                if (!(neighbours == 0 || neighbours == 1 || neighbours > 3))
                {
                    AddCell(x, y, newState);
                    alive++;
                    redrawList.Add(new Cell(x, y, 2));
                }
                else
                {
                    redrawList.Add(new Cell(x, y, 0));
                }
            }
        }

        //Process dead neighbours
        foreach (Cell keyCell in allDeadNeighbours.Keys)
        {
            if (allDeadNeighbours[keyCell] == 3)
            {
                AddCell(keyCell.x, keyCell.y, newState);
                alive++;
                redrawList.Add(new Cell(keyCell.x, keyCell.y, 1));
            }
        }

        actualState = newState;

        return alive;
    }

    int GetNeighboursFromAlive(int x, int y, int i, Neighbours possibleNeighbours)
    {
        int neighbourCount = 0;
        int k;

        //TOP
        if (actualState[i - 1] != null)
        {
            if (actualState[i - 1][0] == (y - 1))
            {
                for (k = topPointer; k < actualState[i - 1].Count; k++)
                {
                    if (actualState[i - 1][k] >= (x - 1))
                    {
                        if (actualState[i - 1][k] == (x - 1))
                        {
                            possibleNeighbours.list[0] = null;
                            topPointer = k + 1;
                            neighbourCount++;
                        }
                        if (actualState[i - 1][k] == x)
                        {
                            possibleNeighbours.list[1] = null;
                            topPointer = k;
                            neighbourCount++;
                        }
                        if (actualState[i - 1][k] == (x + 1))
                        {
                            possibleNeighbours.list[2] = null;

                            if (k == 1)
                            {
                                topPointer = 1;
                            }
                            else
                            {
                                topPointer = k - 1;
                            }

                            neighbourCount++;
                        }
                        if (actualState[i - 1][k] > (x + 1))
                        {
                            break;
                        }
                    }
                }
            }
        }

        //MIDDLE
        for (k = 1; k < actualState[i].Count; k++)
        {
            if (actualState[i][k] >= (x - 1))
            {
                if (actualState[i][k] == (x - 1))
                {
                    possibleNeighbours.list[3] = null;
                    neighbourCount++;
                }
                if (actualState[i][k] == (x + 1))
                {
                    possibleNeighbours.list[4] = null;
                    neighbourCount++;
                }
                if (actualState[i][k] > (x + 1))
                {
                    break;
                }
            }
        }

        //BOTTOM
        if (actualState[i + 1] != null)
        {
            if (actualState[i + 1][0] == (y + 1))
            {
                for (k = bottomPointer; k < actualState[i + 1].Count; k++)
                {
                    if (actualState[i + 1][k] >= (x - 1))
                    {
                        if (actualState[i + 1][k] == (x - 1))
                        {
                            possibleNeighbours.list[5] = null;
                            bottomPointer = k + 1;
                            neighbourCount++;
                        }
                        if (actualState[i + 1][k] == x)
                        {
                            possibleNeighbours.list[6] = null;
                            bottomPointer = k;
                            neighbourCount++;
                        }
                        if (actualState[i + 1][k] == (x + 1))
                        {
                            possibleNeighbours.list[7] = null;

                            if (k == 1)
                            {
                                bottomPointer = 1;
                            }
                            else
                            {
                                bottomPointer = k - 1;
                            }

                            neighbourCount++;
                        }
                        if (actualState[i + 1][k] > (x + 1))
                        {
                            break;
                        }
                    }
                }
            }
        }

        return neighbourCount;
    }

    bool IsAlive(int x, int y)
    {
        for (int i = 0; i < actualState.Count; i++)
        {
            if (actualState[i][0] == y)
            {
                for (int j = 0; j < actualState[i].Count; j++)
                {
                    if (actualState[i][j] == x)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    void RemoveCell(int x, int y, List<List<int>> state)
    {
        for (int i = 0; i < state.Count; i++)
        {
            if (state[i][0] == y)
            {
                if (state[i].Count == 2)
                {
                    state.RemoveAt(i);
                }
                else
                {
                    for (int j = 0; j < state[i].Count; j++)
                    {
                        if (state[i][j] == x)
                        {
                            state[i].RemoveAt(j);
                        }
                    }
                }
            }
        }
    }


    void AddCell(int x, int y, List<List<int>> state)
    {
        if (state.Count == 0)
        {
            List<int> newRow = new List<int>();
            newRow.Add(y);
            newRow.Add(x);
            state.Add(newRow);
            return;
        }

        int k, n, m, added;
        List<int> tempRow = new List<int>();
        List<List<int>> newState = new List<List<int>>();

        if (y < state[0][0])
        {
            //tempRow.Add
            //newState
        }
    }

}
