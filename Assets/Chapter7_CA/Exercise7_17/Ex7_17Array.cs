using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ex7_17Array : MonoBehaviour {

    Texture2D _image;

    Rect _rectangle;
    int _size;
    Color[] _colors;

    int _width;
    int _height;
    int[] cells = { 1, 0, 1, 0, 0, 0, 0, 1, 0, 1, 1, 1, 0, 0, 0, 1, 1, 1, 0, 0 }; //the initial statement of the cell
	void Start () {
        //
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
            if(cells[i] == 0){
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
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnGUI()
    {
        GUI.DrawTexture(_rectangle, _image);
    }
}
