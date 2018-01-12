using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle4_05 : MonoBehaviour
{

    Vector2 location;
    Vector2 velocity;
    Vector2 acceleration;
    float lifespan;

    Texture2D image;
    int width;
    int height;

    //ΤΕΣΤ


    public Particle4_05(Vector2 l)
    {
        float rx = Random.Range(-1, 1);
        float ry = Random.Range(-2, 1);
        acceleration = new Vector2(0, 0.05f);
        velocity = new Vector2(rx, ry);
        location = l;
        lifespan = 255.0f;


    }

    bool ΙsDead()
    {
        if (lifespan < 0.0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // Use this for initialization
    void Start()
    {

        width = Camera.main.pixelWidth;
        height = Camera.main.pixelHeight;
        image = new Texture2D(width, height);
    }


    // Update is called once per frame
    void Update()
    {
        velocity += acceleration;
        location += velocity;
        lifespan -= 2;

        DrawCircle();



    }

    void DrawCircle()
    {
        


        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float dx = location.x - x;
                float dy = location.y - y;
                float distance = Mathf.Sqrt(dx * dx + dy * dy);
                Color color;

                if (distance < 200)
                {
                    color = Color.gray;
                }
                else if(distance>200 && distance <205)
                {
                    color = Color.black;
                }
                else
                {
                    color = Color.white;
                }

                image.SetPixel(x, y, color);


            }


        }



        image.Apply();
    }


    void OnGUI()
    {
        var rectangle = new Rect(0, 0, width, height);
        GUI.DrawTexture(rectangle, image);

        
    }

}
