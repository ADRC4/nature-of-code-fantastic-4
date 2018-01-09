using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IAnimal
{
    void MakeSound();
    void GetNumberLegs();
}


class Animal : IAnimal
{
    private int _legs;

    public int Legs
    {
        get
        {
            return _legs;
        }

        set
        {
            if (value > 0)
                _legs = value;
            else
                _legs = 0;
        }
    }

    public void SetLegs(int legs)
    {
        _legs = legs;
    }

    string _sound;

    public Animal(int legs, string sound)
    {
        _legs = legs;
        _sound = sound;
    }

    public void MakeSound()
    {
        Debug.Log($"Sound: {_sound}");
    }

    public void GetNumberLegs()
    {
        Debug.Log($"Legs: {_legs}");
    }
}

class Dog : Animal
{
    public Dog() : base(4, "Bark.")
    {
    }
}

class Cat : Animal
{
    public Cat() : base(4, "Miau.")
    {
    }
}

class Farm
{
    List<IAnimal> animals = new List<IAnimal>();

    public Farm()
    {
        animals.Add(new Dog());
        animals.Add(new Cat());
        animals.Add(new Dog());
    }

    public void AddAnimal(Animal animal)
    {
        animals.Add(animal);
    }

    public void MakeAllSounds()
    {
        foreach (var animal in animals)
        {
            animal.MakeSound();
        }
    }
}


class Vector3d
{
    float x, y, z;

    public Vector3d(float _x, float _y, float _z)
    {
        x = _x;
        y = _y;
        z = _z;
    }

    public float GetLength()
    {
        return Mathf.Sqrt(x * x + y * y + z * z);
    }

    public void Normalize()
    {
        float length = GetLength();
        x /= length;
        y /= length;
        z /= length;
    }
}
