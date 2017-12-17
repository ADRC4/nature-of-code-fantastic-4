using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawImage : MonoBehaviour
{
    [SerializeField]
    private GUISkin _skin;

    private Rect _rectangle;
    private int _width;
    private int _height;
    private int _size;
    private int _generationsPerFrame = 5;

    private Texture2D _image;
    private Color[] _colors;
    private DLA _dla;

    private void Start()
    {
        Random.InitState(42);
        int scale = 2;
        _width = Screen.width / scale;
        _height = Screen.height / scale;
        _size = _width * _height;

        _image = new Texture2D(_width, _height)
        {
            filterMode = FilterMode.Point
        };

        _rectangle = new Rect(0, 0, Screen.width, Screen.height);
        _colors = new Color[_size];
        _dla = new DLA(_width, _height);
    }

    private void Update()
    {
        for (int i = 0; i < _size; i++)
        {
            int x = i % _width;
            int y = i / _width;

            if (_dla.Cells[x, y] > 0)
            {
                _colors[i] = Color.white;
            }
            else if (_dla.Cells[x, y] < 0)
            {
                float f = -_dla.Cells[x, y] / (_dla.GenerationCount + 1f);
                f = Mathf.Clamp01(f) * 0.25f;
                _colors[i] = Color.HSVToRGB(f, 1, 1);
            }
            else
            {
                _colors[i] *= 0.9f;
                _colors[i].a = 1f;
            }
        }

        for (int i = 0; i < _generationsPerFrame; i++)
        {
            _dla.NextGeneration();
        }

        _image.SetPixels(_colors);
        _image.Apply();
    }

    private void OnGUI()
    {
        GUI.skin = _skin;
        GUI.DrawTexture(_rectangle, _image);
        GUILayout.Label("Generations per frame: ");
        _generationsPerFrame = (int)GUILayout.HorizontalSlider(_generationsPerFrame, 1, 100);
    }
}