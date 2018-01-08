using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayListParticles : MonoBehaviour
{
    Texture2D image;
    int width;
    int height;

    public List<float> centersX;
    public List<float> centersY;

    float length = 6;
    float count = 0;

    float angle;

    bool isDead = false;

    void Start()
    {
        width = Camera.main.pixelWidth;
        height = Camera.main.pixelHeight;
        image = new Texture2D(width, height);
        // centersX = new float[] { width * 0.5f };
        // centersY = new float[] { height * 0.9f };
        centersX = new List<float>();
        centersY = new List<float>();
        for (int i =0; i<length; i++)
        {
            centersX.Add(width * 0.5f);
            centersY.Add(height * 0.9f);
        }
    }

    void Update()
    {
        angle += Time.deltaTime;
        FallingParticle();
    }

    void FallingParticle()
    {
        for (int i = 0; i < length; i++)
        {
                centersX[i] -= Random.Range(-80,80) * Time.time * Mathf.Cos(angle); //ellipse
                centersY[i] -= 120 * Time.time * Mathf.Sin(angle);
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float distance = 100000;
                for (int i = 0; i < length; i++)
                {
                    float dx = Mathf.Abs(centersX[i] - x);
                    float dy = Mathf.Abs(centersY[i] - y);
                    float tempDistance = Mathf.Sqrt(dx * dx + dy * dy);
                    if (tempDistance < distance) distance = tempDistance;
                }
                float f = 1 - (float)y / (float)height;
                Color color = Color.HSVToRGB(0, 0, f); //represents lifespan
                if (distance < width * 0.02f)
                {
                    image.SetPixel(x, y, Color.white);
                }
                if (distance >= width * 0.02f && distance <= width * 0.04f)
                {
                    image.SetPixel(x, y, color);
                    //if (f == 1)
                    //{
                    //    isDead = true;
                    //}
                }
                if (distance > width * 0.04f)
                {
                    image.SetPixel(x, y, Color.white);
                }
            }
        }
        //if (isDead == true)
        //{
        //    for (int k = 0; k < width; k += 5)
        //    {
        //        for (int l = 0; l < height; l += 5)
        //        {
        //            image.SetPixel(l, k, Color.red);
        //        }
        //    }
        //}
        image.Apply();
    }

    void OnGUI()
    {
        var rectangle = new Rect(0, 0, width, height);
        GUI.DrawTexture(rectangle, image);
    }
}