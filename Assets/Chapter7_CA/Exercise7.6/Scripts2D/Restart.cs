using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Restart : MonoBehaviour {

    // Use this for initialization
    public GameObject gameManager;
    void Start () {
        //if (Input.GetKeyDown(KeyCode.R))
        //    Application.LoadLevel("Level");
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.R))
            Application.LoadLevel("Level");
    }
}
