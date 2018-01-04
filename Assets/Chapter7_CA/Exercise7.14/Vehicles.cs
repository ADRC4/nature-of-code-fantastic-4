using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicles : MonoBehaviour
{
    List<GameObject> _Vehicles = new List<GameObject>();

    Vector2 location;
    Vector2 velocity;
    Vector2 acceleration;
    float r;
    float maxforce;
    float maxspeed;

    //public [] _Vehicles;

    //public Vehicles()
    //    {
    //    }

    

    public Vehicles()
    {
        //location = new Vector2( x, y);
        r = 12f;
        maxspeed = 3;
        maxforce = 0.2f;
        acceleration = new Vector2(0, 0);
        velocity = new Vector2(0, 0);
    }

    //void ApplyForce(Vector2 force)
    //{
    //    acceleration.Add(force, force);
    //}

    void Separate ()
    {
        foreach (var Vehicle in _Vehicles)
        {
            float desiredsepartion = r * 2;
            Vector2 sum = new Vector2(0, 0);
            int count = 0;

            for (int i =0; i < _Vehicles.Capacity ; i++)
            {

            }
        }
    }
}
