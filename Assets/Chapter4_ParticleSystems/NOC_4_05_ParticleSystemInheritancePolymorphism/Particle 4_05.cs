using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle4_05 : MonoBehaviour
{

    Vector2 location;
    Vector2 velocity;
    Vector2 acceleration;
    float lifespan;
    //
    Texture2D image;
    int width;
    int height;

    Particle4_05 p;


    public Particle4_05(Vector2 l)
    {
        float rx = Random.Range(-10, 10);
        float ry = Random.Range(-5, 5);
        acceleration = new Vector2(0, -2.0f);
        velocity = new Vector2(rx, ry);
        location = l;
        lifespan = 1.0f;


    }

    bool ΙsDead()
    {
        if (p.lifespan < 0.0)
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

        p = new Particle4_05(new Vector2(width / 2, height * 4 / 5));
    }


    // Update is called once per frame
    void Update()
    {


        p.velocity += p.acceleration;
        p.location += p.velocity;
        p.lifespan -= 0.05f;

        DrawCircle();



    }

    void DrawCircle()
    {



        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float dx = p.location.x - x;
                float dy = p.location.y - y;
                float distance = Mathf.Sqrt(dx * dx + dy * dy);
                Color color;

                if (distance < 10)
                {
                    color = Color.gray * p.lifespan;
                }
                else if (distance > 10 && distance < 12)
                {
                    color = Color.black * p.lifespan;
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
