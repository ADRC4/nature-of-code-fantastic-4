using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CA_2D
{
    public bool[] Cells;
    Texture2D _image;

    Rect _rectangle;
    int _size;
    Color[] _colors;
    int cells;
    int _width;
    int _height;
    void Start()
    {
        int scale = 20;//scale up or it would be too small
        _width = 400 / scale;
        _height = 400 / scale;
        _size = _width * _height;//a box
        _image = new Texture2D(_width, _height);
        _image.filterMode = FilterMode.Point;
        _rectangle = new Rect(20, Screen.height - (_width * scale) - 30, _width * scale, _height * scale);//hope this would make the box center of the screen
        _colors = new Color[_size];

        for (int i = 0; i < _height; i++)
        {
            if (cells[i] == 0)
            {
                _colors[i] = Color.white;
            }
            else
            {
                _colors[i] = Color.gray;
            }
        }

        _image.SetPixels(_colors);
        _image.Apply();
    }
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
    void Update()
    {
       
    }
private void OnGUI()
{
    GUI.DrawTexture(_rectangle, _image);
}
}

