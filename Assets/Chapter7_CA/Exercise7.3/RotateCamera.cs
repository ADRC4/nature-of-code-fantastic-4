using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour {

    public GameObject box;
    public GUISkin UISkin;
    string _text = "Hello world.";
    Rect _rectangle = new Rect(20, 20, 400, 80);

    void Start()
    {
        var farm = new Farm();

        farm.AddAnimal(new Dog());
        var cow = new Animal(4, "Moo.");
        cow.Legs = -1;


        farm.MakeAllSounds();
        // _text = $"Value of b: {b}";
    }


    void Update()
    {
        float angle = 15 * Time.deltaTime;
        this.transform.RotateAround(Vector3.zero, Vector3.up, angle);

        
    }
}
