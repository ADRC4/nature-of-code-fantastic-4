using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleGOL : MonoBehaviour {

    public int startColumns = 64;
    public int startRows = 64;
    public float timeStep = 0.1f;
    public Material material;

    int m_columns;
    int m_rows;

    Texture2D m_texture;

    int[,] m_current;
    int[,] m_next;

    void Awake()
    {
        m_columns = startColumns;
        m_rows = startRows;

        m_texture = new Texture2D(m_columns, m_rows);
        m_texture.filterMode = FilterMode.Point;

        m_current = new int[m_columns, m_rows];
        m_next = new int[m_columns, m_rows];

        for (int y = 0; y < m_rows; y++)
        {
            for (int x = 0; x < m_columns; x++)
            {
                var state = Random.Range(0, 2);
                m_current[x, y] = state;
                m_next[x, y] = state;
            }
        }
    }
    float timer;
    int GetCellState(int x, int y)
    {
        if (y < 0 || y >= m_rows)
            return 0;

        if (x < 0 || x >= m_columns)
            return 0;

        return m_current[x, y];
    }

    int GetNeighborsState(int x, int y)
    {
        int total = 0;
        for (int j = -1; j <= 1; j++)
        {
            for (int i = -1; i <= 1; i++)
            {
                total += GetCellState(x + i, y + j);
            }
        }
        return total;
    }

    void UpdateCell(int x, int y)

    {
        timer += Time.deltaTime;
        var neighbors = GetNeighborsState(x, y);
        var state = m_current[x, y];
        var next = state;
        if (state == 1)
        {
            if (neighbors < 2 || neighbors > 3)
            {
                next = 0; // Death
            }
        }
        else
        {
            if (neighbors == 3)
            {
                next = 1; // Birth
            }
        }

        m_next[x, y] = next;
    }

    void UpdateAllCells()
    {
        for (int y = 0; y < m_rows; y++)
        {
            for (int x = 0; x < m_columns; x++)
            {
                UpdateCell(x, y);
            }
        }

        var pixels = new Color32[m_columns * m_rows];
        for (int y = 0; y < m_rows; y++)
        {
            for (int x = 0; x < m_columns; x++)
            {
                var i = y * m_columns + x;

                // carry next to current
                var state = m_next[x, y];
                m_current[x, y] = state;

                if (state != 0)
                {
                    pixels[i] = new Color32(255, 0, 0, 255);
                }
                else
                {
                    pixels[i] = new Color32(0, 0, 0, 255);
                }
            }
        }

        // update texture
        m_texture.SetPixels32(pixels);
        m_texture.Apply();

        if (material)
        {
            material.SetTexture("_MainTex", m_texture);
        }
    }

    void Update()
    {
        UpdateAllCells();
    }

    private void OnDestroy()
    {
        if (m_texture)
        {
            Object.Destroy(m_texture);
            m_texture = null;
        }
    }
}
