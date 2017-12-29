using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class returnyield : MonoBehaviour {

    static void Main()
    {
        Zoo theZoo = new Zoo();

        theZoo.AddMammal("Whale");
        theZoo.AddMammal("Rhinoceros");
        theZoo.AddBird("Penguin");
        theZoo.AddBird("Warbler");

        foreach (string name in theZoo)
        {
            Console.Write(name + " ");
        }
        Console.WriteLine();
        // Output: Whale Rhinoceros Penguin Warbler  

        foreach (string name in theZoo.Birds)
        {
            Console.Write(name + " ");
        }
        Console.WriteLine();
        // Output: Penguin Warbler  

        foreach (string name in theZoo.Mammals)
        {
            Console.Write(name + " ");
        }
        Console.WriteLine();
        // Output: Whale Rhinoceros  

        Console.ReadKey();
    }

    public class Zoo : IEnumerable
    {
        // Private members.  
        private List<Animal> animals = new List<Animal>();

        // Public methods.  
        public void AddMammal(string name)
        {
            animals.Add(new Animal { Name = name, Type = Animal.TypeEnum.Mammal });
        }

        public void AddBird(string name)
        {
            animals.Add(new Animal { Name = name, Type = Animal.TypeEnum.Bird });
        }

        public IEnumerator GetEnumerator()
        {
            foreach (Animal theAnimal in animals)
            {
                yield return theAnimal.Name;
            }
        }

        // Public members.  
        public IEnumerable Mammals
        {
            get { return AnimalsForType(Animal.TypeEnum.Mammal); }
        }

        public IEnumerable Birds
        {
            get { return AnimalsForType(Animal.TypeEnum.Bird); }
        }

        // Private methods.  
        private IEnumerable AnimalsForType(Animal.TypeEnum type)
        {
            foreach (Animal theAnimal in animals)
            {
                if (theAnimal.Type == type)
                {
                    yield return theAnimal.Name;
                }
            }
        }

        // Private class.  
        private class Animal
        {
            public enum TypeEnum { Bird, Mammal }

            public string Name { get; set; }
            public TypeEnum Type { get; set; }
        }
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
